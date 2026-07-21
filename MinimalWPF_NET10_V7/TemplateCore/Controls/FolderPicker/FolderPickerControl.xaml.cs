namespace System.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaction logic for FolderPicker.xaml
    /// </summary>
    public partial class FolderPickerControl : UserControl, INotifyPropertyChanged
    {
        private const string EmptyItemName = "Leer";
        private const string NewFolderName = "Neues Verzeichnis";
        private const int MaxNewFolderSuffix = 10000;

        private FPTreeItem root;
        private FPTreeItem selectedItem;
        private string initialPath;
        private Style itemContainerStyle;

        #region Properties

        public FPTreeItem Root
        {
            get
            {
                return root;
            }
            private set
            {
                this.root = value;
                this.NotifyPropertyChanged(() => Root);
            }
        }

        public FPTreeItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            private set
            {
                this.selectedItem = value;
                this.NotifyPropertyChanged(() => SelectedItem);
            }
        }

        public string SelectedPath { get; private set; }

        public string InitialPath
        {
            get
            {
                return this.initialPath;
            }
            set
            {
                this.initialPath = value;
                this.UpdateInitialPathUI();
            }
        }

        public Style ItemContainerStyle
        {
            get
            {
                return this.itemContainerStyle;
            }
            set
            {
                this.itemContainerStyle = value;
                this.OnPropertyChanged();
            }
        }

        #endregion

        public FolderPickerControl()
        {
            this.InitializeComponent();
            this.Init();
        }

        public void CreateNewFolder()
        {
            CreateNewFolderImpl(SelectedItem);
        }

        public void RefreshTree()
        {
            this.Root = null;
            this.Init();
        }

        #region INotifyPropertyChanged Members

        public void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else memberExpression = (MemberExpression)lambda.Body;
            OnPropertyChanged(memberExpression.Member.Name);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Private methods

        private void Init()
        {
            root = new FPTreeItem("root", null);
            var systemDrives = DriveInfo.GetDrives();

            foreach (DriveInfo sd in systemDrives)
            {
                var item = new DriveTreeItem(sd.Name, sd.DriveType, root);
                item.Childs.Add(new FPTreeItem(EmptyItemName, item));

                root.Childs.Add(item);
            }

            Root = root;
        }

        private void TreeView_Selected(object sender, RoutedEventArgs e)
        {
            var tvi = e.OriginalSource as TreeViewItem;
            if (tvi != null)
            {
                SelectedItem = tvi.DataContext as FPTreeItem;
                SelectedPath = SelectedItem.GetFullPath();
            }
        }

        private void TreeView_Expanded(object sender, RoutedEventArgs e)
        {
            var tvi = e.OriginalSource as TreeViewItem;
            var treeItem = tvi.DataContext as FPTreeItem;

            if (treeItem != null)
            {
                if (!treeItem.IsFullyLoaded)
                {
                    treeItem.Childs.Clear();

                    string path = treeItem.GetFullPath();

                    DirectoryInfo dir = new DirectoryInfo(path);

                    try
                    {
                        var subDirs = dir.GetDirectories();
                        foreach (var sd in subDirs)
                        {
                            FPTreeItem item = new FPTreeItem(sd.Name, treeItem);
                            item.Childs.Add(new FPTreeItem(EmptyItemName, item));

                            treeItem.Childs.Add(item);
                        }
                    }
                    catch { }

                    treeItem.IsFullyLoaded = true;
                }
            }
            else
            {
                throw new IOException();
            }
        }

        private void UpdateInitialPathUI()
        {
            if (!Directory.Exists(InitialPath))
                return;

            var initialDir = new DirectoryInfo(InitialPath);

            if (!initialDir.Exists)
                return;

            var stack = TraverseUpToRoot(initialDir);
            var containerGenerator = TreeView.ItemContainerGenerator;
            var uiContext = TaskScheduler.FromCurrentSynchronizationContext();
            DirectoryInfo currentDir = null;
            var dirContainer = Root;

            AutoResetEvent waitEvent = new AutoResetEvent(true);

            Task processStackTask = Task.Factory.StartNew(() =>
                {
                    while (stack.Count > 0)
                    {
                        waitEvent.WaitOne();

                        currentDir = stack.Pop();

                        Task waitGeneratorTask = Task.Factory.StartNew(() =>
                        {
                            if (containerGenerator == null)
                                return;

                            while (containerGenerator.Status != GeneratorStatus.ContainersGenerated)
                                Thread.Sleep(50);
                        }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);

                        Task updateUiTask = waitGeneratorTask.ContinueWith((r) =>
                        {
                            try
                            {
                                var childItem = dirContainer.Childs.Where(c => c.Name == currentDir.Name).FirstOrDefault();
                                var tvi = containerGenerator.ContainerFromItem(childItem) as TreeViewItem;
                                dirContainer = tvi.DataContext as FPTreeItem;
                                tvi.IsExpanded = true;

                                tvi.Focus();

                                containerGenerator = tvi.ItemContainerGenerator;
                            }
                            catch { }

                            waitEvent.Set();
                        }, uiContext);
                    }

                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
        }

        private static Stack<DirectoryInfo> TraverseUpToRoot(DirectoryInfo child)
        {
            if (child == null)
                return null;

            if (!child.Exists)
                return null;

            Stack<DirectoryInfo> queue = new Stack<DirectoryInfo>();
            queue.Push(child);
            DirectoryInfo ti = child.Parent;

            while (ti != null)
            {
                queue.Push(ti);
                ti = ti.Parent;
            }

            return queue;
        }

        private static void CreateNewFolderImpl(FPTreeItem parent)
        {
            try
            {
                if (parent == null)
                    return;

                var parentPath = parent.GetFullPath();
                var newDirName = GenerateNewFolderName(parentPath);
                var newPath = Path.Combine(parentPath, newDirName);

                Directory.CreateDirectory(newPath);

                var childs = parent.Childs;
                var newChild = new FPTreeItem(newDirName, parent);
                childs.Add(newChild);
                parent.Childs = childs.OrderBy(c => c.Name).ToObservableCollection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler bei der Erstellung eines Verzeichnis '{ex.Message}'","Neues Verzeichnis", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private static string GenerateNewFolderName(string parentPath)
        {
            string result = NewFolderName;

            if (Directory.Exists(Path.Combine(parentPath, result)))
            {
                for (int i = 1; i < MaxNewFolderSuffix; ++i)
                {
                    var nameWithIndex = string.Format(CultureInfo.CurrentCulture, NewFolderName + " {0}", i);

                    if (Directory.Exists(Path.Combine(parentPath, nameWithIndex)) == false)
                    {
                        result = nameWithIndex;
                        break;
                    }
                }
            }

            return result;
        }

        private void CreateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            if (item != null)
            {
                var context = item.DataContext as FPTreeItem;
                CreateNewFolderImpl(context);
            }
        }

        private void RenameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = sender as MenuItem;
                if (item != null)
                {
                    var context = item.DataContext as FPTreeItem;
                    if (context != null && !(context is DriveTreeItem))
                    {
                        var dialog = new FPInputDialog()
                        {
                            Message = $"Soll das Verzeichnis umbenannt werden '{context.Name}'?",
                            InputText = context.Name,
                            Title = "Umbenennen Verzeichnis"
                        };

                        if (dialog.ShowDialog() == true)
                        {
                            var newFolderName = dialog.InputText;

                            /*
                             * „Parent“ ist im Kontext immer != null, da wir keine Änderung des Namens von „DriveTreeItem“ zulassen.
                             */
                            string newFolderFullPath = Path.Combine(context.Parent.GetFullPath(), newFolderName);
                            if (Directory.Exists(newFolderFullPath))
                            {
                                MessageBox.Show($"Verzeichnis existiert bereits: {newFolderFullPath}", "Umbenennen Verzeichnis", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                            }
                            else
                            {
                                Directory.Move(context.GetFullPath(), newFolderFullPath);
                                context.Name = newFolderName;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Verzeichnis kann nicht umbenannt werden: {ex.Message}", "Umbenennen Verzeichnis", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = sender as MenuItem;
                if (item != null)
                {
                    var context = item.DataContext as FPTreeItem;
                    if (context != null && !(context is DriveTreeItem))
                    {
                        var confirmed =
                            MessageBox.Show($"Möchten Sie das Verzeichnis '{context.Name}' wirklich löschen?", "Bestätigung der Verzeichnislöschung", MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.No);

                        if (confirmed == MessageBoxResult.Yes)
                        {
                            Directory.Delete(context.GetFullPath());
                            var parent = context.Parent;
                            parent.Childs.Remove(context);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Verzeichnis kann nicht gelöscht werden: {ex.Message}", "Verzeichnis löschen", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        #endregion
    }

    public sealed class DriveIconConverter : IValueConverter
    {
        private static BitmapImage removable;
        private static BitmapImage drive;
        private static BitmapImage netDrive;
        private static BitmapImage cdrom;
        private static BitmapImage ram;
        private static BitmapImage folder;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is FrameworkElement element)
            {
                if (element.FindResource("UsbDriveIcon") is DrawingImage imageRemovable)
                {
                    if (removable == null)
                    {
                        removable = ConvertDrawingImageToBitmapImage(imageRemovable);
                    }
                }

                if (element.FindResource("DriveIcon") is DrawingImage imageDrive)
                {
                    if (drive == null)
                    {
                        drive = ConvertDrawingImageToBitmapImage(imageDrive);
                    }
                }

                if (element.FindResource("NetworkDriveIcon") is DrawingImage imageNetDrive)
                {
                    if (netDrive == null)
                    {
                        netDrive = ConvertDrawingImageToBitmapImage(imageNetDrive);
                    }
                }

                if (element.FindResource("CdRomDriveIcon") is DrawingImage imageCDRom)
                {
                    if (cdrom == null)
                    {
                        cdrom = ConvertDrawingImageToBitmapImage(imageCDRom);
                    }
                }

                if (element.FindResource("RamDriveIcon") is DrawingImage imageRam)
                {
                    if (ram == null)
                    {
                        ram = ConvertDrawingImageToBitmapImage(imageRam);
                    }
                }

                if (element.FindResource("IconOffice_Folder_64") is DrawingImage imageFolder)
                {
                    if (folder == null)
                    {
                        folder = ConvertDrawingImageToBitmapImage(imageFolder);
                    }
                }
            }

            var treeItem = value as FPTreeItem;
            if (treeItem == null)
            {
                throw new ArgumentException("Illegal item type");
            }

            if (treeItem is DriveTreeItem)
            {
                DriveTreeItem driveItem = treeItem as DriveTreeItem;
                switch (driveItem.DriveType)
                {
                    case DriveType.CDRom:
                        return cdrom;
                    case DriveType.Fixed:
                        return drive;
                    case DriveType.Network:
                        return netDrive;
                    case DriveType.NoRootDirectory:
                        return drive;
                    case DriveType.Ram:
                        return ram;
                    case DriveType.Removable:
                        return removable;
                    case DriveType.Unknown:
                        return drive;
                }
            }
            else
            {
                return folder;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static BitmapImage ConvertDrawingImageToBitmapImage(DrawingImage drawingImage)
        {
            if (drawingImage == null)
            {
                return null;
            }

            // Abmessungen des Ausgangsbildes ermitteln
            int width = (int)drawingImage.Width;
            int height = (int)drawingImage.Height;

            // 1. RenderTargetBitmap für die Pixeldaten erstellen
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                width, height, 96, 96, PixelFormats.Pbgra32);

            // Drawing visualisieren und rendern
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext context = drawingVisual.RenderOpen())
            {
                context.DrawImage(drawingImage, new Rect(0, 0, width, height));
            }
            renderTargetBitmap.Render(drawingVisual);

            // 2. Den PngBitmapEncoder verwenden, um das Bild in Bytes zu konvertieren
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                encoder.Save(memoryStream);
                memoryStream.Position = 0;

                // 3. Das BitmapImage aus dem Stream aufbauen
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Freeze für bessere Performance und Thread-Sicherheit

                return bitmapImage;
            }
        }

        #endregion
    }

    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public static class LinqExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            var result = new ObservableCollection<T>();

            foreach (var ci in source)
            {
                result.Add(ci);
            }

            return result;
        }
    }
}
