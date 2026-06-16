//-----------------------------------------------------------------------
// <copyright file="HtmlUC.cs" company="Lifeprojects.de">
//     Class: HtmlUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>16.06.2026</date>
//
// <summary>
// Template für eine neues UserControl
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF.Beispiel
{
    using MinimalWPF.Beispiele;

    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für HtmlUC.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public partial class HtmlUC : UserControlBase
    {
        public HtmlUC(ChangeViewEventArgs args) : base(typeof(HtmlUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.HtmlSourceCommand = new CommandBase(commandParam => this.OnHtmlSource(commandParam), () => true);

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase HtmlSourceCommand { get; private set; }
        private ChangeViewEventArgs CurrentCtorArgs { get; set; }

        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
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

        private void OnHtmlSource(object commandParam)
        {
            if (commandParam != null && commandParam.Equals("html_tabelle") == true)
            {
                this.TxTHtmlPlain.Text = $"<table>\n\t<tr>\n\t<td>\n\t\t<span style='color: blue'>&nbsp;top left</span>\n</td><td>&nbsp;top right</td></tr>" +
                                                  "<tr><td>&nbsp;middle left</a></td><td>&nbsp;middle right</td></tr>" +
                                                  "<tr><td><i>&nbsp;bottom left</i></td><td>\n<span style='font-size:18'>&nbsp;bottom right</span>\n\t</td>\n\t</tr>\n</table>";
            }
            else if (commandParam != null && commandParam.Equals("html_color") == true)
            {
                this.TxTHtmlPlain.Text = "<span style=\"color: red;\">Dieser Text ist rot.</span>\r\n";
            }
            else if (commandParam != null && commandParam.Equals("html_source") == true)
            {
                this.TxTHtmlPlain.Text = $"<span style=\"color: black; font-weight: bold; font-size: 14\">Beispiel in C#</span>" +
                    $"<pre style=\"border: 1px solid #ccc; padding: 10px; background-color: #f4f4f4; border-radius: 5px; overflow-x: auto;\">\r\n<code style=\"color: blue; tab-size: 4; font-weight: bold;\">\r\n" +
                    $"public void Beispiel()<br>" +
                    $"{{<br>" +
                    $"&Tab;console.log(\"Hallo Welt!\");" +
                    $"<br>}}" +
                    $"\n</code>" +
                    $"\n</pre>";
            }
            else if (commandParam != null && commandParam.Equals("html_link") == true)
            {
                this.TxTHtmlPlain.Text = "<p>Besuchen Sie unsere Webseite unter <a href=\"https://beispiel.de\">www.beispiel.de</a> für mehr Informationen.</p>\r\n";
            }
        }
        #endregion Command Events
    }
}
