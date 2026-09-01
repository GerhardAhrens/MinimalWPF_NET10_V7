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
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;

    using MinimalWPF.Core;

    using MinimalWPF_TAB.DemoData;

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
            this.NewEntryCommand = new CommandBase(commandParam => this.OnNewEntry(commandParam), () => true);
            this.CopyEntryCommand = new CommandBase(commandParam => this.OnCopyEntry(commandParam), () => true);
            this.SelectDataRowCommand = new CommandBase(commandParam => this.OnSelectDataRow(commandParam), () => true);
            this.RowDoubleClickCommand = new CommandBase(commandParam => this.OnSelectDataRowClick(commandParam), () => true);
            this.CloseTabCommand = new CommandBase(commandParam => this.OnCloseTab(commandParam), () => true);
            this.SelectionChangedCommand = new CommandBase(commandParam => this.OnSelectionChanged(commandParam), () => true);

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase NewEntryCommand { get; private set; }
        public CommandBase CopyEntryCommand { get; private set; }
        public CommandBase CloseTabCommand { get; private set; }
        public CommandBase SelectDataRowCommand { get; private set; }
        public CommandBase RowDoubleClickCommand { get; private set; }
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

        public bool IsLoading
        {
            get => base.GetValue<bool>();
            set => base.SetValue(value);
        }

        private ChangeViewEventArgs CurrentCtorArgs { get; set; }
        private MessageBase Message { get; } = new MessageBase();
        private bool IsModified { get; set; } = false;

        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.IsLoading = true;

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

            this.IsLoading = false;
        }

        private async Task RefeshDataSourceAsync()
        {
            TimeSpan t = Performance.Measure(() =>
            {
                if (File.Exists("artikel.json") == false)
                {
                    DataTable dtNew = DemoData.LadeArtikel();
                    DataTableJsonSerializer.Save(dtNew, "artikel.json");
                }

                DataTable dt = DataTableJsonSerializer.Load("artikel.json").ToSorting(ListSortDirection.Ascending,"A");
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
                await App.EventAgg.PublishAsync(new StatusEvent($"Bereit; {t.TotalMilliseconds:N1} ms"));
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

        private async void OnNewEntry(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.NewEntry)
                {
                    AdvancedTabItem artikelTab = new AdvancedTabItem()
                    {
                        Header = $"Neuer Artikel",
                        HeaderImage = (ImageSource)Application.Current.FindResource("IconApplicationEnd"),
                    };


                    ICollectionView view = (ICollectionView)this.DataSource;
                    if (view.SourceCollection is DataView dv)
                    {
                        DataTable originalTable = dv.Table;

                        if (App.EventAgg.IsSubscription<StatusEvent>() == true)
                        {
                            await App.EventAgg.PublishAsync(new StatusEvent($"Neuer Eintrag"));
                        }

                        this.ArtikelTabControl.Items.Add(artikelTab);
                        artikelTab.Content = new TabArtikelDetail(originalTable, DataRowAction.Add);
                        this.ArtikelTabControl.SelectedItem = artikelTab;
                    }
                }
            }
        }

        private async void OnCopyEntry(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.CopyEntry)
                {
                    AdvancedTabItem artikelTab = new AdvancedTabItem()
                    {
                        Header = $"Artikel {this.Id}",
                        HeaderImage = (ImageSource)Application.Current.FindResource("IconApplicationEnd"),
                    };

                    ICollectionView view = (ICollectionView)this.DataSource;
                    if (view.SourceCollection is DataView dv)
                    {
                        if (App.EventAgg.IsSubscription<StatusEvent>() == true)
                        {
                            await App.EventAgg.PublishAsync(new StatusEvent($"Kopieren Eintrag"));
                        }

                        this.ArtikelTabControl.Items.Add(artikelTab);
                        artikelTab.Content = new TabArtikelDetail(this.SelectedDataRow.Row,DataRowAction.ChangeOriginal);
                        this.ArtikelTabControl.SelectedItem = artikelTab;
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
                    selectedTab.Content = new TabArtikelDetail(rowView.Row,DataRowAction.Change);
                }
            }
        }

        private async void OnCloseTab(object commandParam)
        {
            if (commandParam is TabItem tabItem)
            {
                DataRow rowView = ((TabArtikelDetail)tabItem.Content).CurrentRow;
                DataRowAction rowAction = ((TabArtikelDetail)tabItem.Content).RowAction;

                if (rowAction == DataRowAction.Change && rowView.HasRowChanges() == true)
                {
                    // Es existieren Änderungen
                    MessageBoxResult quesion = this.Message.CancelQuestion("Änderungen speichern", "Es existieren Änderungen an den Daten. Möchten Sie die Änderungen speichern?");
                    if (quesion == MessageBoxResult.Yes)
                    {
                        rowView.AcceptChanges();
                        DataTableJsonSerializer.Save(rowView.Table, "artikel.json");
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
                else if (rowAction == DataRowAction.Add)
                {
                    int artikelNr = rowView.Field<int>("A");
                    if (rowView.Table.SelectCount(f => f.Field<int>("A") == artikelNr) == 0)
                    {
                        rowView.Table.Rows.Add(rowView);
                        rowView.AcceptChanges();
                        rowView.Table.AcceptChanges();
                        DataTableJsonSerializer.Save(rowView.Table, "artikel.json");
                        this.ArtikelTabControl.Items.Remove(tabItem);
                        await this.RefeshDataSourceAsync();
                        this.ArtikelTabControl.Items.Remove(tabItem);
                    }
                    else
                    {
                        this.Message.Hinweis("Artikelnummer", $"Die Artikelnummer '{rowView.Field<int>("A")}'");
                        return;
                    }
                }
                else if (rowAction == DataRowAction.Add)
                { 
                }
                else
                {
                    this.ArtikelTabControl.Items.Remove(tabItem);
                }
            }
        }

        #endregion Command Events

    }
}
