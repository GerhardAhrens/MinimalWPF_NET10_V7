namespace System.Windows
{
    using System;
    using System.IO;
    using System.Windows.Resources;
    using System.Collections.Specialized;

    internal static class SourceGenerator
    {
        static SourceGenerator()
        {
            if (Directory.Exists(TemplatePath) == false)
            {
                Directory.CreateDirectory(TemplatePath);
            }
        }

        public static string TemplatePath { get; private set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template");

        public static void CreateSourceFile(string className, string newClassName)
        {
            StringCollection files = new StringCollection();

            (string,string) sources = GetSourceFromResources(className);
            if (string.IsNullOrEmpty(sources.Item1) == false && string.IsNullOrEmpty(sources.Item2) == false)
            {
                string rootNamespace = AppDomain.CurrentDomain.FriendlyName;
                string csFilePath = Path.Combine(TemplatePath, $"{newClassName}.xaml.cs");
                if (string.IsNullOrEmpty(sources.Item1) == false)
                {
                    string csContent = sources.Item1.Replace("[[ClassName]]", newClassName).Replace("[[RootNamespace]]", $"{rootNamespace}.Beispiel");
                    File.WriteAllText(csFilePath, csContent);
                    files.Add(csFilePath);
                }

                string xamlFilePath = Path.Combine(TemplatePath, $"{newClassName}.xaml");
                if (string.IsNullOrEmpty(sources.Item2) == false)
                {
                    string xamlContent = sources.Item2.Replace("[[ClassName]]", newClassName).Replace("[[RootNamespace]]", $"{rootNamespace}.Beispiel");
                    File.WriteAllText(xamlFilePath, xamlContent);
                    files.Add(xamlFilePath);
                }
            }
            else
            {
                string csFilePath = Path.Combine(TemplatePath, $"{className}.cs");
                File.WriteAllText(csFilePath, sources.Item1);
                files.Add(csFilePath);
            }

            if (files.Count > 0)
            {
                ClipboardHelper.CutFilesToClipboard(files);
            }
        }

        public static (string,string) GetSourceFromResources(string className)
        {
            Uri uriCS;
            Uri uriXAML;
            string outCodeCS = string.Empty;
            string outCodeXAML = string.Empty;

            uriCS = new Uri($"pack://application:,,,/Resources/Source/{className}.xaml.cs.source", UriKind.Absolute);
            if (DoesResourceExist(uriCS) == true)
            {
                StreamResourceInfo sri = Application.GetResourceStream(uriCS);
                using StreamReader reader = new StreamReader(sri.Stream);
                outCodeCS = reader.ReadToEnd();
            }

            uriXAML = new Uri($"pack://application:,,,/Resources/Source/{className}.xaml.source", UriKind.Absolute);
            if (DoesResourceExist(uriXAML) == true)
            {
                StreamResourceInfo sri = Application.GetResourceStream(uriXAML);
                using StreamReader reader = new StreamReader(sri.Stream);
                outCodeXAML = reader.ReadToEnd();
            }


            return (outCodeCS, outCodeXAML);
        }

        public static bool DoesResourceExist(Uri resourceUri)
        {
            try
            {
                // Versucht, den Stream der Ressource abzurufen
                var resourceStream = Application.GetResourceStream(resourceUri);

                // Wenn kein Fehler auftritt und der Stream existiert
                return resourceStream != null;
            }
            catch (IOException)
            {
                // FileNotFoundException (bzw. IOException in WPF) wird geworfen, wenn die Ressource fehlt
                return false;
            }
            catch (ArgumentException)
            {
                // Tritt auf, wenn die Uri fehlerhaft oder ungültig ist
                return false;
            }
        }
    }
}
