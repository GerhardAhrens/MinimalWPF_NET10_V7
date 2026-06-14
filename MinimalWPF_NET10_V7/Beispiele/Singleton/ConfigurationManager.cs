namespace MinimalWPF.Beispiele
{
    using System;
    using System.Diagnostics;
    using System.Windows;

    public class ConfigurationManager : SingletonBase<ConfigurationManager>, 
                                        ISingletonInitializable, 
                                        ISingletonReloadable
    {
        protected ConfigurationManager()
        {
        }

        public string ApplicationName { get; private set; } = string.Empty;
        public DateTime LastReload { get; private set; }

        public void Initialize()
        {
            // Beispielwerte erstellen
            this.LoadConfiguration();
        }

        public void ReloadContent()
        {
            this.LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            // Simuliert Laden aus Datei/DB/API
            ApplicationName = $"App geladen: {DateTime.Now:T}";
            LastReload = DateTime.Now;
        }
    }
}
