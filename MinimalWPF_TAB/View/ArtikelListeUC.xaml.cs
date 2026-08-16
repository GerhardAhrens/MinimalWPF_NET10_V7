//-----------------------------------------------------------------------
// <copyright file="ArtikellisteUC.cs" company="Lifeprojects.de">
//     Class: ArtikellisteUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>04.08.2026</date>
//
// <summary>
// Template für eine neues UserControl
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF.View
{
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    using MinimalWPF.Core;

    using static System.Windows.Forms.VisualStyles.VisualStyleElement;

    /// <summary>
    /// Interaktionslogik für Artikelliste.xaml
    /// </summary>
    public partial class ArtikellisteUC : UserControlBase
    {
        public ArtikellisteUC(ChangeViewEventArgs args) : base(typeof(ArtikellisteUC))
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.SelectDataRowCommand = new CommandBase(commandParam => this.OnSelectDataRow(commandParam), () => true);
            this.SelectDataRowClickCommand = new CommandBase(commandParam => this.OnSelectDataRowClick(commandParam), () => true);
            this.CloseTabCommand = new CommandBase(commandParam => this.OnCloseTab(commandParam), () => true);
            this.SelectionChangedCommand = new CommandBase(commandParam => this.OnSelectionChanged(commandParam), () => true);

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase CloseTabCommand { get; private set; }
        public CommandBase SelectDataRowCommand { get; private set; }
        public CommandBase SelectDataRowClickCommand { get; private set; }
        public CommandBase SelectionChangedCommand { get; private set; }

        public ICollectionView DataSource
        {
            get => base.GetValue<ICollectionView>();
            set => base.SetValue(value);
        }

        public DataRowView SelectedDataRow
        {
            get => base.GetValue<DataRowView>();
            set => base.SetValue(value);
        }

        public ID Id
        {
            get => base.GetValue<ID>();
            set => base.SetValue(value);
        }

        private ChangeViewEventArgs CurrentCtorArgs { get; set; }
        private MessageBase Message { get; } = new MessageBase();
        private bool IsModified { get; set; } = false;

        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            // 1. Daten asynchron laden
            await this.RefeshDataSourceAsync();

            // 2. Warten, bis der Dispatcher das UI-Layout und die Container im Hintergrund gerendert hat
            await Dispatcher.InvokeAsync(() =>
            {
                this.lvwMain.SelectedIndex = 0;

                var firstItem = lvwMain.ItemContainerGenerator.ContainerFromIndex(0) as ListViewItem;
                if (firstItem != null)
                {
                    firstItem.Focus();
                    Keyboard.Focus(firstItem);
                }
            }, System.Windows.Threading.DispatcherPriority.Background);
        }

        private async Task RefeshDataSourceAsync()
        {
            TimeSpan t = Performance.Measure(() =>
            {
                DataTable dt = LadeArtikel();
                this.DataSource = CollectionViewSource.GetDefaultView(dt);
                if (this.DataSource != null)
                {
                    try
                    {
                        this.DataSource.MoveCurrentToFirst();
                        int maxCount = this.DataSource.Cast<DataRowView>().Count();
                    }
                    catch (Exception ex)
                    {
                        string errorText = ex.Message;
                        throw;
                    }
                }

                dt.ColumnChanged += (s, e) =>
                {
                    this.IsModified = true;
                    if (App.EventAgg.IsSubscription<StatusEvent>() == true)
                    {
                        if (this.IsModified == true)
                        {
                            _ = App.EventAgg.PublishAsync(new StatusEvent("Geändert"));
                        }
                    }
                };
            });

            if (App.EventAgg.IsSubscription<StatusEvent>() == true)
            {
                await App.EventAgg.PublishAsync(new StatusEvent($"Bereit; {t.TotalMilliseconds} ms"));
            }
        }

        #endregion Windows Events

        #region Command Events
        private async void OnGoBack(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.GoBack)
                {
                    ChangeViewEventArgs args = new();
                    args.MenuButton = this.CurrentCtorArgs.FromPage;
                    args.FromPage = this.CurrentCtorArgs.MenuButton;
                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }

        private void OnSelectDataRow(object commandParam)
        {
            if (commandParam is DataRowView rowView)
            {
                this.Id = Convert.ToInt32(rowView["A"]);
            }
        }

        private async void OnSelectDataRowClick(object commandParam)
        {
            if (commandParam is DataRowView rowView)
            {

                AdvancedTabItem artikelTab = new AdvancedTabItem() 
                { 
                    Header = $"Artikel {this.Id}", 
                    HeaderImage = (ImageSource)Application.Current.FindResource("IconApplicationEnd"),
                };

                bool isTabFound = ArtikelTabControl.Items.OfType<AdvancedTabItem>().Any(tab => tab.Header?.ToString() == artikelTab.Header.ToString());
                if (isTabFound == false)
                {
                    this.SelectedDataRow.Row.AcceptChanges();
                    artikelTab.Tag = this.SelectedDataRow;
                    this.ArtikelTabControl.Items.Add(artikelTab);

                    if (App.EventAgg.IsSubscription<StatusEvent>() == true)
                    {
                        await App.EventAgg.PublishAsync(new StatusEvent($"Bereit"));
                    }
                }

                this.ArtikelTabControl.SelectedItem = artikelTab;
            }
        }

        private void OnSelectionChanged(object commandParam)
        {
            if (commandParam is SelectionChangedEventArgs tabControl)
            {
                AdvancedTabControl tab = (AdvancedTabControl)tabControl.Source;
                AdvancedTabItem selectedTab = tab.SelectedItem as AdvancedTabItem;

                if (selectedTab != null && selectedTab.Tag is DataRowView rowView)
                {
                    selectedTab.Content = new TabArtikelDetail(rowView);
                }
            }
        }

        private void OnTabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl)
            {
                if (tabControl.SelectedItem is TabItem selectedTab && selectedTab.Tag is DataRowView rowView)
                {
                    selectedTab.Content = new TabArtikelDetail(rowView);
                }
            }
        }

        private async void OnCloseTab(object commandParam)
        {
            if (((FrameworkElement)commandParam).Tag.ToString() == string.Empty)
            {
                return;
            }

            if (commandParam is TabItem tabItem)
            {
                DataRow rowView = ((TabArtikelDetail)tabItem.Content).CurrentRow.Row;

                if (this.HasChanges(rowView) == true)
                {
                    // Es existieren Änderungen
                    MessageBoxResult quesion = this.Message.CancelQuestion("Änderungen speichern", "Es existieren Änderungen an den Daten. Möchten Sie die Änderungen speichern?");
                    if (quesion == MessageBoxResult.Yes)
                    {
                        rowView.AcceptChanges();
                        this.ArtikelTabControl.Items.Remove(tabItem);
                        await this.RefeshDataSourceAsync();
                    }
                    else if (quesion == MessageBoxResult.No)
                    {
                        rowView.RejectChanges();
                        this.ArtikelTabControl.Items.Remove(tabItem);
                        await this.RefeshDataSourceAsync();
                    }
                    else if (quesion == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    this.ArtikelTabControl.Items.Remove(tabItem);
                }
            }
        }

        #endregion Command Events

        private static DataTable LadeArtikel()
        {
            DataTable table = new("Artikel");

            table.Columns.Add("A", typeof(int));         // Key
            table.Columns.Add("B", typeof(string));      // Artikelname
            table.Columns.Add("C", typeof(decimal));     // Preis

            table.PrimaryKey = new[] { table.Columns["A"] };

            table.Rows.Add(2001, "Kugelschreiber", 1.99m);
            table.Rows.Add(2002, "Bleistift", 0.79m);
            table.Rows.Add(2003, "Radiergummi", 1.29m);
            table.Rows.Add(2004, "Notizblock", 3.49m);
            table.Rows.Add(2005, "Ordner", 4.99m);
            table.Rows.Add(2006, "Locher", 8.95m);
            table.Rows.Add(2007, "Tacker", 12.50m);
            table.Rows.Add(2008, "Lineal", 2.19m);
            table.Rows.Add(2009, "Schere", 6.75m);
            table.Rows.Add(2010, "Marker", 2.99m);

            return table;
        }

        public bool HasChanges(DataRow row)
        {
            if (row.RowState != DataRowState.Modified)
            {
                return false;
            }

            foreach (DataColumn column in row.Table.Columns)
            {
                if (!Equals(row[column, DataRowVersion.Original], row[column, DataRowVersion.Current]))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public static class Performance
    {
        /// <summary>
        /// Zeit der ausführung einer Aktion messen
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <example>
        /// TimeSpan t = Performance.Measure(() =>
        /// {
        ///     Thread.Sleep(500);
        /// });
        /// 
        /// Console.WriteLine(t.TotalMilliseconds);
        /// </example>
        public static TimeSpan Measure(Action action)
        {
            var sw = Stopwatch.StartNew();

            action();

            sw.Stop();

            return sw.Elapsed;
        }

        /// <summary>
        /// Zeit der ausführung einer Funktion messen und das Ergebnis zurückgeben
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        /// <example>
        /// int sum = Performance.Measure(() =>
        /// {
        ///      return Enumerable.Range(1, 1000000).Sum();
        /// }, out TimeSpan duration);
        /// 
        /// Console.WriteLine(sum);
        /// Console.WriteLine(duration.TotalMilliseconds);
        /// </example>
        public static T Measure<T>(Func<T> func, out TimeSpan duration)
        {
            var sw = Stopwatch.StartNew();

            T result = func();

            sw.Stop();

            duration = sw.Elapsed;

            return result;
        }
    }
}
