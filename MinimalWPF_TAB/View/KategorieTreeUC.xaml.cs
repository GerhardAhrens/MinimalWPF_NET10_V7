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
            this.NodeContextMenueCommand = new CommandBase(commandParam => this.OnNodeContextMenue(commandParam), () => true);
        }

        private void OnNodeContextMenue(object commandParam)
        {
            AdvancedTreeNode node = commandParam as AdvancedTreeNode;
            this.Message.Hinweis("Kontextmenü Node", $"{node.Text}");
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

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase SelectionChangedCommand { get; private set; }
        public CommandBase NodeSelectedCommand { get; private set; }
        public CommandBase NodeDoubleClickedCommand { get; private set; }
        public CommandBase NodeContextMenueCommand { get; private set; }

        public ObservableCollection<AdvancedTreeNode> Nodes { get; private set; }

        public AdvancedTreeNode SelectedNode
        {
            get => base.GetValue<AdvancedTreeNode>();
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
            this.Nodes = this.CreateDemoData();

            //this.SelectedNode = Nodes[1];

            this.DataContext = this;

            if (App.EventAgg.IsSubscription<StatusEvent>() == true)
            {
                await App.EventAgg.PublishAsync(new StatusEvent("Bereit"));
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

        private void OnTabSelectionChanged(object commandParam)
        {
            if (commandParam is SelectionChangedEventArgs tabControl)
            {
                AdvancedTabControl tab = (AdvancedTabControl)tabControl.Source;
                AdvancedTabItem selectedTab = tab.SelectedItem as AdvancedTabItem;
            }
        }

        #endregion Command Events

        private ObservableCollection<AdvancedTreeNode> CreateDemoData()
        {
            DrawingImage closedImage = Application.Current.TryFindResource("TreeFolderClosed") as DrawingImage;
            DrawingImage openImage = Application.Current.TryFindResource("TreeFolderOpen") as DrawingImage;

            ObservableCollection<AdvancedTreeNode> nodes = new ObservableCollection<AdvancedTreeNode>
            {
                new AdvancedTreeNode(Guid.CreateVersion7(), "Kunden")
                {
                    OpenImage = closedImage,
                    ExpandedImage = openImage,
                    IsExpanded = false,
                    Children =
                    {
                        new AdvancedTreeNode(Guid.CreateVersion7(),"Müller")
                        {
                            Children =
                            {
                                new AdvancedTreeNode(Guid.CreateVersion7(),"Rechnungen")
                                {
                                },
                                new AdvancedTreeNode(Guid.CreateVersion7(),"Aufträge")
                                {
                                }
                            }},
                        new AdvancedTreeNode(Guid.CreateVersion7(),"Meier")
                        {
                            ContextMenuItems =
                            {
                                new AdvancedTreeMenuItem("Öffnen",this.NodeContextMenueCommand),
                                new AdvancedTreeMenuItem("Bearbeiten",this.NodeContextMenueCommand),
                                new AdvancedTreeMenuItem("Löschen",this.NodeContextMenueCommand)
                            }
                        },
                        new AdvancedTreeNode(Guid.CreateVersion7(),"Schmidt")
                        {
                            ContextMenuItems =
                            {
                                new AdvancedTreeMenuItem("Öffnen",this.NodeContextMenueCommand),
                                new AdvancedTreeMenuItem("Bearbeiten",this.NodeContextMenueCommand),
                                new AdvancedTreeMenuItem("Löschen",this.NodeContextMenueCommand)
                            }
                        }
                    }
                },

                new AdvancedTreeNode(Guid.CreateVersion7(),"Projekte")
                {
                    OpenImage = closedImage,
                    ExpandedImage = openImage,
                    Children =
                    {
                        new AdvancedTreeNode(Guid.CreateVersion7(),"Projekt A"),
                        new AdvancedTreeNode(Guid.CreateVersion7(),"Projekt B")
                    }
                },

                new AdvancedTreeNode(Guid.CreateVersion7(),"Einstellungen")
                {
                    OpenImage = closedImage,
                    ExpandedImage = openImage,
                }
            };

            return nodes;
        }
    }
}
