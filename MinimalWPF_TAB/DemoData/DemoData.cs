namespace MinimalWPF_TAB.DemoData
{
    using System.Data;

    internal class DemoData
    {
        public static DataTable LadeArtikel()
        {
            DataTable table = new("Artikel");

            table.Columns.Add("A", typeof(int));         // Key
            table.Columns.Add("B", typeof(string));      // Artikelname
            table.Columns.Add("C", typeof(decimal));     // Preis
            table.Columns.Add("Warengruppe", typeof(string));      // Warengruppe

            table.PrimaryKey = new[] { table.Columns["A"] };

            table.Rows.Add(2001, "Kugelschreiber", 1.99m, "Schreibwaren");
            table.Rows.Add(2002, "Bleistift", 0.79m, "Schreibwaren");
            table.Rows.Add(2003, "Radiergummi", 1.29m, "Schreibwaren");
            table.Rows.Add(2004, "Notizblock", 3.49m, "Schreibwaren");
            table.Rows.Add(2005, "Ordner", 4.99m, "Schreibwaren");
            table.Rows.Add(2006, "Locher", 8.95m, "Schreibwaren");
            table.Rows.Add(2007, "Tacker", 12.50m, "Schreibwaren");
            table.Rows.Add(2008, "Lineal", 2.19m, "Schreibwaren");
            table.Rows.Add(2009, "Schere", 6.75m, "Schreibwaren");
            table.Rows.Add(2010, "Marker", 2.99m, "Schreibwaren");
            table.Rows.Add(2011, "Lineal", 1.49m, "Schreibwaren");
            table.Rows.Add(1010, "Akku Bohrer", 39.49m, "Baumarkt");
            table.Rows.Add(1011, "Akku, Ersatz", 19.89m, "Baumarkt");
            table.Rows.Add(1012, "Bohrkopf, Ersatz", 13.89m, "Baumarkt");
            table.Rows.Add(1013, "Bohrkopf, Bit", 13.89m, "Baumarkt");
            table.Rows.Add(1020, "Bit Set, kurz", 9.39m, "Baumarkt");
            table.Rows.Add(1021, "Bit Set, lang", 11.69m, "Baumarkt");
            table.Rows.Add(1022, "Bohrer Bit-Aufnahme", 6.80m, "Baumarkt");
            table.Rows.Add(1023, "Bohrer Metall, Standard", 7.90m, "Baumarkt");
            table.Rows.Add(1024, "Bohrer Stein, Standard", 7.90m, "Baumarkt");
            table.Rows.Add(1025, "Bohrer Holz, Standard", 7.90m, "Baumarkt");

            return table;
        }
    }
}
