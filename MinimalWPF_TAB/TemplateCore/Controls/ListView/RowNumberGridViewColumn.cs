namespace MinimalWPF_TAB.TemplateCore.Controls.ListView
{
    using System.Windows.Controls;

    internal sealed class RowNumberGridViewColumn : AdvancedGridViewColumn
    {
        public RowNumberGridViewColumn()
        {
            Header = "#";
            Width = 45;
            AllowSorting = false;
            ShowFilter = false;
            IsRowNumberColumn = true;
        }
    }
}
