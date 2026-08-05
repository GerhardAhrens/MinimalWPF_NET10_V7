namespace System.Windows.Controls
{
    using System.Windows;
    using System.Windows.Input;

    public class AdvancedTabControl : TabControl
    {
        static AdvancedTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(AdvancedTabControl),
                new FrameworkPropertyMetadata(typeof(AdvancedTabControl)));
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
    }
}
