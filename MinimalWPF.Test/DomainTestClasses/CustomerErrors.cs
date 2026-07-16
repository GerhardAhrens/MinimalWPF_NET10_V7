namespace MinimalWPF.Test.Sample
{

    using System.Windows.Domain;

    public static class CustomerErrors
    {
        public static readonly ResultError NameEmpty =  new("Customer.Name.Empty", "Der Name darf nicht leer sein.");

        public static readonly ResultError InvalidEmail =new("Customer.Email.Invalid", "Die E-Mail-Adresse ist ungültig.");

        public static readonly ResultError AlreadyDeleted = new("Customer.AlreadyDeleted", "Der Kunde wurde bereits gelöscht.");

        public static readonly ResultError AdressCityEmpty = new("Adresse, Ort", "Der Ort darf nicht leer sein.");

        public static readonly ResultError AdressZipFalse = new("Adresse, Postleitzahl", "Die Postleitzahl ist leer oder falsch.");

        public static readonly ResultError AdressStreetEmpty = new("Adresse, Strasse", "Die Strasse darf nicht leer sein.");
    }
}
