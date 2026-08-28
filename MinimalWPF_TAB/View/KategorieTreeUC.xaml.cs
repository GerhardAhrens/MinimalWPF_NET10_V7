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
    using System.Data;
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
            this.SelectionChangedCommand = new CommandBase(commandParam => this.OnTabSelectionChanged(commandParam), () => true);
            this.NodeSelectedCommand = new CommandBase(commandParam => this.OnNodeSelected(commandParam), () => true);
            this.NodeDoubleClickedCommand = new CommandBase(commandParam => this.OnNodeDoubleClicked(commandParam), () => true);
            this.NodeContextMenueEditCommand = new CommandBase(commandParam => this.OnNodeContextMenueEdit(commandParam), () => true);
            this.NodeContextMenueDeleteCommand = new CommandBase(commandParam => this.OnNodeContextMenueDelete(commandParam), () => true);
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase SelectionChangedCommand { get; private set; }
        public CommandBase NodeSelectedCommand { get; private set; }
        public CommandBase NodeDoubleClickedCommand { get; private set; }
        public CommandBase NodeContextMenueEditCommand { get; private set; }
        public CommandBase NodeContextMenueDeleteCommand { get; private set; }

        public ObservableCollection<AdvancedTreeNode> Nodes { get; private set; } = new();

        public AdvancedTreeNode SelectedNode
        {
            get => base.GetValue<AdvancedTreeNode>();
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
            /*
            var adapter = new AdvancedTreeItemAdapter<Device>(device => device.Name);
            foreach (var device in Devices)
            {
                var n = adapter.Convert(device);
                this.Nodes.Add(n);
            }
            */
            this.IsLoading = true;

            // 1. Daten asynchron laden
            await this.RefeshDataSourceAsync();
            //this.Nodes = this.CreateDemoData();

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

        private void OnTabSelectionChanged(object commandParam)
        {
            if (commandParam is SelectionChangedEventArgs tabControl)
            {
                AdvancedTabControl tab = (AdvancedTabControl)tabControl.Source;
                AdvancedTabItem selectedTab = tab.SelectedItem as AdvancedTabItem;
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

        private void OnNodeSelected(object commandParam)
        {
            AdvancedTreeNode node = commandParam as AdvancedTreeNode;
        }

        private void OnNodeDoubleClicked(object commandParam)
        {
            AdvancedTreeNode node = commandParam as AdvancedTreeNode;
            this.Message.Hinweis("Auswahl Node", $"{node.Text}");
        }

        #endregion Command Events


        private async Task RefeshDataSourceAsync()
        {
            TimeSpan t = Performance.Measure(() =>
            {
                this.Nodes = this.CreateDemoData();
            });

            if (App.EventAgg.IsSubscription<StatusEvent>() == true)
            {
                await App.EventAgg.PublishAsync(new StatusEvent($"Bereit; {t.TotalMilliseconds:N1} ms"));
            }
        }

        private ObservableCollection<AdvancedTreeNode> CreateDemoData()
        {
            DrawingImage closedImage = Application.Current.TryFindResource("TreeFolderClosed") as DrawingImage;
            DrawingImage openImage = Application.Current.TryFindResource("TreeFolderOpen") as DrawingImage;

            DataTable table = DemoData.LadeArtikel();

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
