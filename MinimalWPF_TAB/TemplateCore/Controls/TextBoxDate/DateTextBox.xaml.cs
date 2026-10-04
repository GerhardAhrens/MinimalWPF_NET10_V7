namespace System.Windows.Controls
{
    using System.Globalization;
    using System.Windows.Input;
    using System.Windows.Media;

    public partial class DateTextBox : UserControl
    {
        private static readonly CultureInfo GermanCulture = CultureInfo.GetCultureInfo("de-DE");
        private DateTime _displayMonth = DateTime.Today;
        private bool _updating;

        public DateTextBox()
        {
            InitializeComponent();

            this.PART_MonthCombo.ItemsSource = GermanCulture.DateTimeFormat.MonthNames.Take(12).ToArray();

            int currentYear = DateTime.Today.Year;
            this.PART_YearCombo.ItemsSource = Enumerable.Range(currentYear - 100, 201).ToArray();

            this.Loaded += (_, _) =>
            {
                this.UpdateHeader();
                this.RenderCalendar();
                this.UpdateTextFromDate();
            };
        }

        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register(
                nameof(SelectedDate),
                typeof(DateTime?),
                typeof(DateTextBox),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedDateChanged));

        public DateTime? SelectedDate
        {
            get => (DateTime?)GetValue(SelectedDateProperty);
            set => SetValue(SelectedDateProperty, value);
        }

        private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (DateTextBox)d;

            if (e.NewValue is DateTime date)
            {
                control._displayMonth = new DateTime(date.Year, date.Month, 1);
            }

            control.UpdateTextFromDate();
            control.UpdateHeader();
            control.RenderCalendar();
        }

        private void UpdateTextFromDate()
        {
            if (this.PART_TextBox == null)
            {
                return;
            }

            this._updating = true;

            this.PART_TextBox.Text = this.SelectedDate.HasValue ? this.SelectedDate.Value.ToString("dd.MM.yyyy", GermanCulture) : string.Empty;

            this._updating = false;
        }

        private void UpdateHeader()
        {
            if (this.PART_MonthCombo == null || this.PART_YearCombo == null)
                return;

            this._updating = true;
            this.PART_MonthCombo.SelectedIndex = this._displayMonth.Month - 1;
            this.PART_YearCombo.SelectedItem = this._displayMonth.Year;
            this._updating = false;
        }

        private void RenderCalendar()
        {
            if (this.PART_DaysGrid == null)
                return;

            this.PART_DaysGrid.Children.Clear();

            DateTime firstDay = new DateTime(this._displayMonth.Year, this._displayMonth.Month, 1);

            // Montag = 0, Dienstag = 1, ..., Sonntag = 6
            int leadingDays = ((int)firstDay.DayOfWeek + 6) % 7;
            int daysInMonth = DateTime.DaysInMonth(firstDay.Year, firstDay.Month);

            for (int i = 0; i < 42; i++)
            {
                int day = i - leadingDays + 1;

                if (day < 1 || day > daysInMonth)
                {
                    // Leere Felder statt Tage des Nachbarmonats
                    this.PART_DaysGrid.Children.Add(new Border
                    {
                        Height = 34,
                        Margin = new Thickness(1)
                    });
                    continue;
                }

                DateTime date = new DateTime(firstDay.Year, firstDay.Month, day);
                bool isSelected = this.SelectedDate.HasValue && this.SelectedDate.Value.Date == date.Date;
                bool isToday = date.Date == DateTime.Today;
                var button = new Button
                {
                    Content = day.ToString(GermanCulture),
                    Height = 34,
                    Margin = new Thickness(1),
                    Padding = new Thickness(0),
                    FontSize = 13,
                    Cursor = Cursors.Hand,
                    BorderThickness = isToday && !isSelected
                        ? new Thickness(1)
                        : new Thickness(0),
                    BorderBrush = new SolidColorBrush(
                        Color.FromRgb(37, 99, 235)),
                    Background = isSelected
                        ? new SolidColorBrush(
                            Color.FromRgb(37, 99, 235))
                        : Brushes.Transparent,
                    Foreground = isSelected
                        ? Brushes.White
                        : new SolidColorBrush(
                            Color.FromRgb(31, 41, 55)),
                    Tag = date
                };

                button.Template = CreateDayButtonTemplate();

                button.Click += (_, _) =>
                {
                    this.SelectedDate = (DateTime)button.Tag;
                    this.PART_Popup.IsOpen = false;
                };

                this.PART_DaysGrid.Children.Add(button);
            }
        }

        private ControlTemplate CreateDayButtonTemplate()
        {
            var template = new ControlTemplate(typeof(Button));

            var border = new FrameworkElementFactory(typeof(Border));
            border.Name = "DayBorder";
            border.SetValue(Border.CornerRadiusProperty, new CornerRadius(6));
            border.SetValue(Border.BackgroundProperty, new TemplateBindingExtension( Control.BackgroundProperty));
            border.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension( Control.BorderBrushProperty));
            border.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension( Control.BorderThicknessProperty));

            var presenter = new FrameworkElementFactory(typeof(ContentPresenter));

            presenter.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            presenter.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);

            border.AppendChild(presenter);
            template.VisualTree = border;

            var hover = new Trigger
            {
                Property = UIElement.IsMouseOverProperty,
                Value = true
            };
            hover.Setters.Add(new Setter(Border.BackgroundProperty, new SolidColorBrush(Color.FromRgb(229, 231, 235)), "DayBorder"));
            template.Triggers.Add(hover);

            return template;
        }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedDate.HasValue)
            {
                this._displayMonth = new DateTime(this.SelectedDate.Value.Year, this.SelectedDate.Value.Month, 1);
            }
            else
            {
                this._displayMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            }

            this.UpdateHeader();
            this.RenderCalendar();
            this.PART_Popup.IsOpen = !this.PART_Popup.IsOpen;
        }

        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            this._displayMonth = this._displayMonth.AddMonths(-1);
            this.UpdateHeader();
            this.RenderCalendar();
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            this._displayMonth = this._displayMonth.AddMonths(1);
            this.UpdateHeader();
            this.RenderCalendar();
        }

        private void MonthCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this._updating || this.PART_MonthCombo.SelectedIndex < 0)
                return;

            this._displayMonth = new DateTime(this._displayMonth.Year, this.PART_MonthCombo.SelectedIndex + 1, 1);

            this.RenderCalendar();
        }

        private void YearCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this._updating || this.PART_YearCombo.SelectedItem == null)
                return;

            int year = (int)this.PART_YearCombo.SelectedItem;

            this._displayMonth = new DateTime(year, this._displayMonth.Month, 1);

            this.RenderCalendar();
        }

        private void Today_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedDate = DateTime.Today;
            this.PART_Popup.IsOpen = false;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedDate = null;
            this.PART_Popup.IsOpen = false;
        }

        private void PART_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this._updating)
                return;
        }

        private void PART_TextBox_LostFocus(
            object sender, RoutedEventArgs e)
        {
            if (TryParseInput(this.PART_TextBox.Text, out DateTime date))
            {
                this.SelectedDate = date;
            }
            else
            {
                // Ungültige oder leere Eingaben zurücksetzen.
                this.UpdateTextFromDate();
            }
        }

        private void PART_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (TryParseInput(this.PART_TextBox.Text, out DateTime date))
                {
                    this.SelectedDate = date;
                    this.PART_Popup.IsOpen = false;
                }
                else
                {
                    this.UpdateTextFromDate();
                }

                e.Handled = true;
            }

            if (e.Key == Key.Escape)
            {
                this.UpdateTextFromDate();
                e.Handled = true;
            }

            if (e.Key == Key.F4 || (Keyboard.Modifiers == ModifierKeys.Alt && e.Key == Key.Down))
            {
                CalendarButton_Click(this, new RoutedEventArgs());
                e.Handled = true;
            }
        }

        private static bool TryParseInput(
            string text, out DateTime date)
        {
            string[] formats =
            {
                "dd.MM.yyyy",
                "d.M.yyyy",
                "dd.MM.yy",
                "d.M.yy",
                "yyyy-MM-dd"
            };

            return DateTime.TryParseExact(text?.Trim(),formats, GermanCulture, DateTimeStyles.None, out date);
        }
    }
}
