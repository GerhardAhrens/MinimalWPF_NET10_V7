namespace MinimalWPF.Beispiele.NotifyProperty
{
    using System.Windows;

    public class NotifyPropertyClass : NotifyPropertyBase
    {
        public string DemoText
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

    }
}
