namespace System.Windows
{
    using System;

    public sealed class LoginEventArgs
    {
        public LoginEventArgs(string benutzerName,string passwort)
        {
            this.Id = Guid.CreateVersion7();
            this.Benutzername = benutzerName;
            this.Passwort = passwort;
        }

        public LoginEventArgs(int pin)
        {
            this.Id = Guid.CreateVersion7();
            this.Pin = pin;
        }

        public static void SetDatabasePath(string databasePath)
        {
            DatabasePath = databasePath;
        }

        public Guid Id { get; private set; }
        public string Benutzername { get; private set; }
        public string Passwort { get; private set; }
        public int Pin { get; private set; }
        public static string DatabasePath { get; private set; }
    }
}
