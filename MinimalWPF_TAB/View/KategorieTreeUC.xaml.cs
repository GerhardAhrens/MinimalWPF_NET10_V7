//-----------------------------------------------------------------------
// <copyright file="KategorieTreeUC.cs" company="Lifeprojects.de">
//     Class: KategorieTreeUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>23.08.2026</date>
//
// <summary>
// Template für eine neues UserControl
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF.View
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using MinimalWPF.Core;

    using MinimalWPF_TAB.DemoData;

    /// <summary>
    /// Interaktionslogik für KategorieTreeUC.xaml
    /// </summary>
    public partial class KategorieTreeUC : UserControlBase
    {
        public KategorieTreeUC(ChangeViewEventArgs args) : base(typeof(KategorieTreeUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.CloseTabCommand = new CommandBase(commandParam => this.OnCloseTab(commandParam), () => true);
            this.SelectionChangedCommand = new CommandBase(commandParam => this.OnTabSelectionChanged(commandParam), () => true);
            this.NodeSelectedCommand = new CommandBase(commandParam => this.OnNodeSelected(commandParam), () => true);
            this.NodeDoubleClickedCommand = new CommandBase(commandParam => this.OnNodeDoubleClicked(commandParam), () => true);
            this.NodeContextMenueEditCommand = new CommandBase(commandParam => this.OnNodeContextMenueEdit(commandParam), () => true);
            this.NodeContextMenueDeleteCommand = new CommandBase(commandParam => this.OnNodeContextMenueDelete(commandParam), () => true);
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase CloseTabCommand { get; private set; }
        public CommandBase SelectionChangedCommand { get; private set; }
        public CommandBase NodeSelectedCommand { get; private set; }
        public CommandBase NodeDoubleClickedCommand { get; private set; }
        public CommandBase NodeContextMenueEditCommand { get; private set; }
        public CommandBase NodeContextMenueDeleteCommand { get; private set; }

        public ObservableCollection<AdvancedTreeNode> Nodes { get; private set; } = new();

        public ID Id
        {
            get => base.GetValue<ID>();
            set => base.SetValue(value);
        }

        public AdvancedTreeNode SelectedNode
        {
            get => base.GetValue<AdvancedTreeNode>();
            set => base.SetValue(value);
        }

        public DataRowView SelectedDataRow
        {
            get => base.GetValue<DataRowView>();
            set => base.SetValue(value);
        }

        public bool IsLoading
        {
            get => base.GetValue<bool>();
            set => base.SetValue(value);
        }

        public string TreeViewFilter
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        private ChangeViewEventArgs CurrentCtorArgs { get; set; }
        private MessageBase Message { get; } = new MessageBase();
        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.IsLoading = true;

            // 1. Daten asynchron laden
            await this.RefeshDataSourceAsync();

            this.IsLoading = false;

            //this.SelectedNode = Nodes[1];
            this.DataContext = this;
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

        private async void OnCloseTab(object commandParam)
        {
            if (((FrameworkElement)commandParam).Tag.ToString() == string.Empty)
            {
                return;
            }

            if (commandParam is TabItem tabItem)
            {
                DataRow rowView = ((TabArtikelDetail)tabItem.Content).CurrentRow;
                if (rowView.HasRowChanges() == true)
                {
                    // Es existieren Änderungen
                    MessageBoxResult quesion = this.Message.CancelQuestion("Änderungen speichern", "Es existieren Änderungen an den Daten. Möchten Sie die Änderungen speichern?");
                    if (quesion == MessageBoxResult.Yes)
                    {
                        rowView.AcceptChanges();
                        this.KategorieTabControl.Items.Remove(tabItem);
                        await this.RefeshDataSourceAsync();
                    }
                    else if (quesion == MessageBoxResult.No)
                    {
                        rowView.RejectChanges();
                        this.KategorieTabControl.Items.Remove(tabItem);
                        await this.RefeshDataSourceAsync();
                    }
                    else if (quesion == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    this.KategorieTabControl.Items.Remove(tabItem);
                }
            }
        }

        private void OnTabSelectionChanged(object commandParam)
        {
            if (commandParam is SelectionChangedEventArgs tabControl)
            {
                AdvancedTabControl tab = (AdvancedTabControl)tabControl.Source;
                AdvancedTabItem selectedTab = tab.SelectedItem as AdvancedTabItem;
                if (selectedTab != null && selectedTab.Tag is DataRow rowView)
                {
                    selectedTab.Content = new TabArtikelDetail(rowView);
                }
            }
        }

        private void OnNodeSelected(object commandParam)
        {
            if (commandParam is AdvancedTreeNode treeNode)
            {
                DataRow currentRow = (DataRow)treeNode.SourceItem;
                this.Id = ((DataRow)treeNode.SourceItem).GetAs<int>("A");
            }
        }

        private async void OnNodeDoubleClicked(object commandParam)
        {
            if (commandParam is AdvancedTreeNode rowNode)
            {
                AdvancedTabItem kategorieTab = new AdvancedTabItem()
                {
                    Header = $"Artikel {((DataRow)rowNode.SourceItem).GetAs<int>("A")}",
                    HeaderImage = (ImageSource)Application.Current.FindResource("IconApplicationEnd"),
                };

                bool isTabFound = this.KategorieTree.Items.OfType<AdvancedTabItem>().Any(tab => tab.Header?.ToString() == kategorieTab.Header.ToString());
                if (isTabFound == false)
                {
                    DataRow currentRow = (DataRow)rowNode.SourceItem;
                    kategorieTab.Tag = currentRow;
                    currentRow.AcceptChanges();
                    this.KategorieTabControl.Items.Add(kategorieTab);

                    if (App.EventAgg.IsSubscription<StatusEvent>() == true)
                    {
                        await App.EventAgg.PublishAsync(new StatusEvent($"Bereit"));
                    }
                }

                this.KategorieTabControl.SelectedItem = kategorieTab;
            }
        }

        private void OnNodeContextMenueEdit(object commandParam)
        {
            AdvancedTreeNode node = commandParam as AdvancedTreeNode;
            this.Message.Hinweis("Kontextmenü Node Edit", $"{node.Text}");
        }

        private void OnNodeContextMenueDelete(object commandParam)
        {
            AdvancedTreeNode node = commandParam as AdvancedTreeNode;
            this.Message.Hinweis("Kontextmenü Node Delete", $"{node.Text}");
        }
        #endregion Command Events


        private async Task RefeshDataSourceAsync()
        {
            TimeSpan t = Performance.Measure(async () =>
            {
                this.Nodes = await this.CreateDemoDataAsync();

            });

            if (App.EventAgg.IsSubscription<StatusEvent>() == true)
            {
                await App.EventAgg.PublishAsync(new StatusEvent($"Bereit; {t.TotalMilliseconds:N1} ms"));
            }
        }

        private async Task<ObservableCollection<AdvancedTreeNode>> CreateDemoDataAsync()
        {
            DrawingImage closedImage = Application.Current.TryFindResource("TreeFolderClosed") as DrawingImage;
            DrawingImage openImage = Application.Current.TryFindResource("TreeFolderOpen") as DrawingImage;

            if (File.Exists("artikel.json") == false)
            {
                DataTable dtNew = DemoData.LadeArtikel();
                DataTableJsonSerializer.Save(dtNew, "artikel.json");
            }

            DataTable table = DataTableJsonSerializer.Load("artikel.json").ToSorting(ListSortDirection.Ascending, "A");

            Nodes.Clear();

            var groups = table.AsEnumerable().GroupBy(row => row.Field<string>("Warengruppe"));

            foreach (var group in groups)
            {
                AdvancedTreeNode groupNode = new AdvancedTreeNode(Guid.CreateVersion7(),group.Key);
                groupNode.OpenImage = closedImage;
                groupNode.ExpandedImage = openImage;
                groupNode.IsExpanded = false;

                foreach (var row in group)
                {
                    AdvancedTreeNode articleNode = new AdvancedTreeNode(Guid.CreateVersion7(), row.Field<string>("B"));
                    articleNode.OpenImage = closedImage;
                    articleNode.ExpandedImage = openImage;
                    articleNode.IsExpanded = false;

                    // Originale DataRow merken
                    articleNode.SourceItem = row;

                    // ContextMenu nur für Artikel
                    articleNode.ContextMenuItems.Add(new AdvancedTreeMenuItem("Bearbeiten")
                        {
                            Command = this.NodeContextMenueEditCommand
                    });

                    articleNode.ContextMenuItems.Add(new AdvancedTreeMenuItem("Löschen")
                        {
                            Command = this.NodeContextMenueDeleteCommand
                    });

                    groupNode.Children.Add(articleNode);
                }

                Nodes.Add(groupNode);
            }

            return Nodes;
        }
    }
}
