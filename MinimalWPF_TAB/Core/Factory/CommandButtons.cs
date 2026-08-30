namespace MinimalWPF.Core
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
        [Description("Neuer Eintrag erstellen")]
        NewEntry = 5,
        [Description("Eintrag löschen")]
        DeleteEntry = 6,
        [Description("Eintrag kopieren")]
        CopyEntry = 7,
        [Description("Artikelliste")]
        Artikelliste = 10,
        [Description("Kategorien")]
        Kategorien = 11,
        [Description("Control Demo")]
        ControlDemo = 12,
        [Description("Informationen")]
        InformationPopup = 20,
        [Description("Einstellungen")]
        SettingsPopup = 21,
    }
}
