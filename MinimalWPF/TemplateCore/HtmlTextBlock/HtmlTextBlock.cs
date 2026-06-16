namespace System.Windows
{
    using System.Windows.Data;
    using System.Windows.Threading;

    public class HtmlTextBlock : System.Windows.Controls.RichTextBox
    {
        #region Private Members

        private bool isInvokePending;

        #endregion Private Members

        #region Constructors

        public HtmlTextBlock()
        {
            this.Loaded += this.RichTextBox_Loaded;

            //Added
            this.IsReadOnly = true;
            this.Focusable = false;
        }

        public HtmlTextBlock(System.Windows.Documents.FlowDocument document) : base(document)
        {
        }

        #endregion Constructors

        #region Properties

        private Microsoft.Windows.Controls.ITextFormatter _textFormatter;
        /// <summary>
        /// The ITextFormatter the is used to format the text of the RichTextBox.
        /// Deafult formatter is the RtfFormatter
        /// </summary>
        public Microsoft.Windows.Controls.ITextFormatter TextFormatter
        {
            get
            {
                if (this._textFormatter == null)
                {
                    this._textFormatter = new Microsoft.Windows.Controls.HtmlFormatter(); //default is HTML
                }

                return this._textFormatter;
            }
            set
            {
                _textFormatter = value;
            }
        }

        #region Text

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(HtmlTextBlock),
            new FrameworkPropertyMetadata(
                String.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnTextPropertyChanged),
                new CoerceValueCallback(CoerceTextProperty),
                true,
                System.Windows.Data.UpdateSourceTrigger.LostFocus));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HtmlTextBlock rtb = (HtmlTextBlock)d;

            rtb.TextFormatter.SetText(rtb.Document, (string)e.NewValue);
        }

        private static object CoerceTextProperty(DependencyObject d, object value)
        {
            return value ?? "";
        }

        #endregion //Text

        #endregion //Properties

        #region Methods

        private void InvokeUpdateText()
        {
            if (!this.isInvokePending)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(this.UpdateText));
                this.isInvokePending = true;
            }
        }

        private void UpdateText()
        {
            this.Text = TextFormatter.GetText(Document);
            this.isInvokePending = false;
        }

        #endregion Methods

        #region Event Hanlders

        private void RichTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.Binding binding = BindingOperations.GetBinding(this, TextProperty);

            if (binding != null)
            {
                if (binding.UpdateSourceTrigger == UpdateSourceTrigger.Default || binding.UpdateSourceTrigger == UpdateSourceTrigger.LostFocus)
                {
                    this.LostFocus += (o, ea) => this.UpdateText(); //do this synchronously
                }
                else
                {
                    this.TextChanged += (o, ea) => this.InvokeUpdateText(); //do this async
                }
            }
        }

        #endregion Event Hanlders
    }
}
