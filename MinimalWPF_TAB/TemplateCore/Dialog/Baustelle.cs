namespace System.Windows.Controls
{
    public static class Baustelle
    {
        public static DialogValueResult Show()
        {
            Window owner = Application.Current.MainWindow;
            return Show(owner,string.Empty, string.Empty);
        }

        public static DialogValueResult Show(Window owner)
        {
            return Show(owner,string.Empty, string.Empty);
        }

        public static DialogValueResult Show(Window owner, string title, string message)
        {
            title = string.IsNullOrEmpty(title) == true ? "Information" : title;
            message = string.IsNullOrEmpty(message) == true ? "Die gewünschte Funktion steht aktuellen nicht zur Verfügung!" : message;

            BaustelleDlg dialog = new BaustelleDlg(title, message);
            dialog.ShowInTaskbar = false;
            dialog.Owner = owner;
            if (dialog.ShowDialog() == true)
            {
                return new DialogValueResult
                {
                    Accepted = true,
                };
            }

            return new DialogValueResult
            {
                Accepted = false
            };
        }
    }
}
