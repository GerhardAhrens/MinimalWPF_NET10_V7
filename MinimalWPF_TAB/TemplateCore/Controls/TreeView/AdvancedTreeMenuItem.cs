namespace System.Windows.Controls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    public class AdvancedTreeMenuItem : INotifyPropertyChanged
    {
        private string _header = string.Empty;
        private object _icon;
        private ICommand _command;
        private object _commandParameter;
        private bool _isEnabled = true;

        public AdvancedTreeMenuItem()
        {
            Items = new ObservableCollection<AdvancedTreeMenuItem>();
        }

        public AdvancedTreeMenuItem(string header, ICommand command = null) : this()
        {
            this.Header = header;
            if (command != null)
            {
                this.Command = command;
            }
        }

        /// <summary>
        /// Text des Menüeintrags.
        /// </summary>
        public string Header
        {
            get => _header;
            set
            {
                if (_header == value)
                    return;

                _header = value;
                this.OnPropertyChanged();
            }
        }


        /// <summary>
        /// Optionales Icon.
        /// Kann beispielsweise ein DrawingImage sein.
        /// </summary>
        public object Icon
        {
            get => _icon;
            set
            {
                if (ReferenceEquals(_icon, value))
                    return;

                _icon = value;
                this.OnPropertyChanged();
            }
        }


        /// <summary>
        /// Command des Menüeintrags.
        /// </summary>
        public ICommand Command
        {
            get => _command;
            set
            {
                if (ReferenceEquals(_command, value))
                    return;

                _command = value;
                this.OnPropertyChanged();
            }
        }


        /// <summary>
        /// Optionaler eigener CommandParameter.
        /// </summary>
        public object CommandParameter
        {
            get => _commandParameter;
            set
            {
                if (ReferenceEquals(
                        _commandParameter,
                        value))
                    return;

                _commandParameter = value;
                this.OnPropertyChanged();
            }
        }


        /// <summary>
        /// Gibt an, ob der Menüeintrag aktiviert ist.
        /// </summary>
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled == value)
                    return;

                _isEnabled = value;
                this.OnPropertyChanged();
            }
        }


        /// <summary>
        /// Untermenüs.
        /// </summary>
        public ObservableCollection<AdvancedTreeMenuItem> Items
        {
            get;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
