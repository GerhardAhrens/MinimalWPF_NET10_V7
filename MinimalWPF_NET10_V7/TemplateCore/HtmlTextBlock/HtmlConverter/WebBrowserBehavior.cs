namespace System.Windows
{
    using System.Windows;
    using System.Windows.Controls;

    public class WebBrowserBehavior
    {
        public static readonly DependencyProperty BodyProperty =
                DependencyProperty.RegisterAttached("Body", typeof(string), typeof(WebBrowserBehavior), new PropertyMetadata(OnChanged));

        public static string GetBody(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(BodyProperty);
        }

        public static void SetBody(DependencyObject dependencyObject, string body)
        {
            dependencyObject.SetValue(BodyProperty, body);
        }

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var webBrowser = (WebBrowser)d;
            webBrowser.NavigateToString((string)e.NewValue);
        }
    }
}
