namespace System.Windows
{
    using System;
    using System.IO;
    using System.Windows.Resources;
    using System.Collections.Specialized;

    internal static class SourceGenerator
    {
        public static void GetSourceFile()
        {

            StringCollection files = new StringCollection();
        }

        public static void GetSourceFromResources(string className)
        {
            Uri uri;
            if (className.Equals("NeuUC", StringComparison.OrdinalIgnoreCase))
            {
                uri = new Uri("pack://application:,,,/Resources/Source/NeuUC.xaml.cs.source", UriKind.Absolute);

                StreamResourceInfo sri = Application.GetResourceStream(uri);
                using StreamReader reader = new StreamReader(sri.Stream);
                string code = reader.ReadToEnd();
            }
        }
    }
}
