namespace MinimalWPF_TAB.DemoData
{
    using System.Data;

    internal class DemoData
    {
        public static DataTable LadeArtikel()
        {
            DataTable table = new("Artikel");

            DataColumn artikelNummer = new();
            artikelNummer.ColumnName = "A";
            artikelNummer.DataType = typeof(int);
            artikelNummer.AllowDBNull = true;
            artikelNummer.ExtendedProperties["ArtikelNummer"] = "Artikelnummer";
            table.Columns.Add(artikelNummer);         // Key

            DataColumn artikelName = new();
            artikelName.ColumnName = "B";
            artikelName.DataType = typeof(string);
            artikelName.MaxLength = 50;
            artikelName.ExtendedProperties["Artikelname"] = "Artikelname";
            table.Columns.Add(artikelName);      // Artikelname

            table.Columns.Add("C", typeof(decimal));     // Preis
            table.Columns.Add("Warengruppe", typeof(string));      // Warengruppe
            table.Columns.Add("Anzahl", typeof(int));      // Stückzahl pro Packung

            //table.PrimaryKey = new[] { table.Columns["A"] };

            table.Rows.Add(2001, "Kugelschreiber", 1.99m, "Schreibwaren",2);
            table.Rows.Add(2002, "Bleistift", 0.79m, "Schreibwaren",5);
            table.Rows.Add(2003, "Radiergummi", 1.29m, "Schreibwaren",2);
            table.Rows.Add(2004, "Notizblock", 3.49m, "Schreibwaren",1);
            table.Rows.Add(2005, "Ordner", 4.99m, "Schreibwaren",1);
            table.Rows.Add(2006, "Locher", 8.95m, "Schreibwaren",1);
            table.Rows.Add(2007, "Tacker", 12.50m, "Schreibwaren",1);
            table.Rows.Add(2008, "Lineal bis 200mm", 2.19m, "Schreibwaren",1);
            table.Rows.Add(2009, "Schere", 6.75m, "Schreibwaren",1);
            table.Rows.Add(2010, "Marker", 2.99m, "Schreibwaren",3);
            table.Rows.Add(2011, "Lineal bis 300mm", 1.49m, "Schreibwaren",1);
            table.Rows.Add(1010, "Akku Bohrer", 39.49m, "Baumarkt",1);
            table.Rows.Add(1011, "Akku, Ersatz", 19.89m, "Baumarkt",1);
            table.Rows.Add(1012, "Bohrkopf, Ersatz", 13.89m, "Baumarkt",1);
            table.Rows.Add(1013, "Bohrkopf, Bit", 13.89m, "Baumarkt",1);
            table.Rows.Add(1020, "Bit Set, kurz", 9.39m, "Baumarkt",1);
            table.Rows.Add(1021, "Bit Set, lang", 11.69m, "Baumarkt",1);
            table.Rows.Add(1022, "Bohrer Bit-Aufnahme", 6.80m, "Baumarkt",1);
            table.Rows.Add(1023, "Bohrer Metall, Standard", 7.90m, "Baumarkt",1);
            table.Rows.Add(1024, "Bohrer Stein, Standard", 7.90m, "Baumarkt",1);
            table.Rows.Add(1025, "Bohrer Holz, Standard", 7.90m, "Baumarkt",1);
            table.Rows.Add(3010, "Sechs-Kant M5x30", 5.50m, "Eisenwaren",100);
            table.Rows.Add(3011, "Sechs-Kant M5x60", 5.75m, "Eisenwaren",100);
            table.Rows.Add(3012, "Sechs-Kant M5x100", 6.00m, "Eisenwaren", 100);
            table.Rows.Add(3030, "Sechs-Kant Mutter M3", 4.50m, "Eisenwaren", 100);
            table.Rows.Add(3031, "Sechs-Kant Mutter M4", 4.90m, "Eisenwaren", 100);
            table.Rows.Add(3032, "Sechs-Kant Mutter M5", 6.00m, "Eisenwaren", 100);
            table.Rows.Add(3033, "Sechs-Kant Mutter M6", 6.50m, "Eisenwaren", 100);
            table.Rows.Add(3034, "Sechs-Kant Mutter M8", 7.10m, "Eisenwaren", 100);
            table.Rows.Add(3035, "Sechs-Kant Mutter M10", 7.50m, "Eisenwaren", 100);

            return table;
        }
    }
}
