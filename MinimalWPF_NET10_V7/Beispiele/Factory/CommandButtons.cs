namespace MinimalWPF.Beispiele
{
    using System.ComponentModel;

    public enum CommandButtons
    {
        [Description("Keine Auswahl")]
        None = 0,
        [Description("Anwendung beenden")]
        AppQuit = 1,
        [Description("Startseite")]
        Home = 2,
        [Description("Hilfe")]
        Help = 3,
        [Description("Zurück zur vorherigen Seite")]
        GoBack = 4,
        [Description("Demo zur Result<T> Klasse")]
        ShowResult = 5,
        [Description("Demo zur InputBox")]
        ShowInputBox = 6,
        [Description("Demo zur DialogService Klasse")]
        ShowDialogService = 7,
        [Description("Demo zur SourceGen Klasse")]
        ShowSourceGen = 8,
        [Description("Demo zu Localization von Texten")]
        Localization = 9,
        [Description("Demo zum Event Aggregator")]
        ShowEventAggregator = 10,
        [Description("Demo zum Factory Pattern")]
        ShowFactoryPattern = 11,
        [Description("Demo zur NotificationBox")]
        ShowNotificationBox = 12,
        [Description("Demo zu Applikation Settings")]
        ShowSettings = 13,
        [Description("Demo zu Singleton Pattern")]
        ShowSingletonPattern = 14,
        [Description("Demo zu Messenger Pattern")]
        ShowMessengerPattern = 15,
        [Description("Demo zu Html TextBlock")]
        ShowHtmlTextBlock = 16,
        [Description("Informationen")]
        InformationPopup = 20,
        [Description("Einstellungen")]
        SettingsPopup = 21,
        [Description("Eingabe prüfen")]
        CheckInput = 30,
    }
}
