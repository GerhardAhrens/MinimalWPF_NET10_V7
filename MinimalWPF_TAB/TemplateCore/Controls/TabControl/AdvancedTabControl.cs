namespace System.Windows.Controls
{
    using System.Windows;
    using System.Windows.Input;

    public class AdvancedTabControl : TabControl
    {
        static AdvancedTabControl()
        {
            /*
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(AdvancedTabControl),
                new FrameworkPropertyMetadata(typeof(AdvancedTabControl)));
            */
        }

        public AdvancedTabControl()
        {
            WeakEventManager<AdvancedTabControl, KeyEventArgs>.AddHandler(this, "PreviewKeyDown", this.OnPreviewKeyDow);
        }


        #region SelectionChangedCommand

        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.Register(
                nameof(SelectionChangedCommand),
                typeof(ICommand),
                typeof(AdvancedTabControl),
                new PropertyMetadata(null));

        public ICommand SelectionChangedCommand
        {
            get => (ICommand)GetValue(SelectionChangedCommandProperty);
            set => SetValue(SelectionChangedCommandProperty, value);
        }

        #endregion

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (SelectionChangedCommand != null &&
                SelectionChangedCommand.CanExecute(e))
            {
                SelectionChangedCommand.Execute(e);
            }
        }

        private void OnPreviewKeyDow(object sender, KeyEventArgs e)
        {
            // Prüfen, ob die Strg-Taste (Ctrl) gedrückt gehalten wird
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                var tabControl = sender as TabControl;
                if (tabControl == null)
                {
                    return;
                }

                // Bestimmen der gedrückten Zahl (Zifferntasten oder Nummernblock)
                int targetIndex = -1;

                if (e.Key >= Key.D1 && e.Key <= Key.D9)
                {
                    targetIndex = e.Key - Key.D1; // Berechnet 0 für Key.D1, 1 für Key.D2 etc.
                }
                else if (e.Key >= Key.NumPad1 && e.Key <= Key.NumPad9)
                {
                    targetIndex = e.Key - Key.NumPad1; // Berechnet 0 für Key.NumPad1 etc.
                }

                // Wenn eine gültige Zahl gedrückt wurde und der Tab-Index existiert
                if (targetIndex >= 0 && targetIndex < tabControl.Items.Count)
                {
                    tabControl.SelectedIndex = targetIndex;
                    e.Handled = true; // Verhindert, dass das Event weitergereicht wird
                }

                if (e.Key == Key.T && tabControl.Items.Count > 1)
                {
                    Dictionary<int,string> openTabItems = new Dictionary<int,string>();
                    int tabIndex = -1;
                    foreach (TabItem item in tabControl.Items)
                    {
                        tabIndex++;
                        openTabItems.Add(tabIndex, item.Header.ToString());
                    }

                    if (openTabItems.Count > 0)
                    {
                        Window parentWindow = Window.GetWindow(this);
                        if (parentWindow != null)
                        {
                            SelectTabItem dialog = new SelectTabItem(openTabItems);
                            dialog.Left = parentWindow.ActualWidth - dialog.Width;
                            dialog.Top = dialog.Height;
                            dialog.Owner = parentWindow;
                            if (dialog.ShowDialog() == true)
                            {
                                if (dialog.Result.Accepted == true)
                                {
                                    tabControl.SelectedIndex = dialog.Result.ResultValue;
                                    e.Handled = true; // Verhindert, dass das Event weitergereicht wird
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
