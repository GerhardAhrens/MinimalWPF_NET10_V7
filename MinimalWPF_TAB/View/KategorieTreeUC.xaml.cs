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
    using System.Windows;
    using System.Windows.Controls;

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
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public ObservableCollection<AdvancedTreeNode> Nodes { get; private set; }

        private ChangeViewEventArgs CurrentCtorArgs { get; set; }
        private MessageBase Message { get; } = new MessageBase();
        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Nodes = this.CreateDemoData();

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
        #endregion Command Events


        private ObservableCollection<AdvancedTreeNode> CreateDemoData()
        {
            ObservableCollection<AdvancedTreeNode>  nodes = new ObservableCollection<AdvancedTreeNode>
            {
                new AdvancedTreeNode("Kunden")
                {
                    Children =
                    {
                        new AdvancedTreeNode("Müller")
                        {
                            Children =
                            {
                                new AdvancedTreeNode("Rechnungen"),
                                new AdvancedTreeNode("Aufträge")
                            }},
                        new AdvancedTreeNode("Meier"),
                        new AdvancedTreeNode("Schmidt")
                    }
                },

                new AdvancedTreeNode("Projekte")
                {
                    Children =
                    {
                        new AdvancedTreeNode("Projekt A"),
                        new AdvancedTreeNode("Projekt B")
                    }
                },

                new AdvancedTreeNode("Einstellungen")
            };

            return nodes;
        }
    }
}
