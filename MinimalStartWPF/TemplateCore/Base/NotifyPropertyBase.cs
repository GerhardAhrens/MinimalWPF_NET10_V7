namespace System.Windows
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using System.Windows.Controls;

    [DebuggerStepThrough]
    [Serializable]
    [SupportedOSPlatform("windows")]
    public class NotifyPropertyBase : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ConcurrentDictionary<string, object> values = new();
        private readonly string className;
        private bool _IsPropertyChanged;

        public NotifyPropertyBase() : base()
        {
            this.className = this.GetType().Name;
        }

        public Type BaseType { get; set; }

        public bool IsPropertyChanged
        {
            get { return this._IsPropertyChanged; }
            set
            {
                this._IsPropertyChanged = value;
                this.SetProperty(ref _IsPropertyChanged, value);
            }
        }

        #region Get/Set Implementierung
        private T GetPropertyValueInternal<T>(string propertyName)
        {
            if (values.ContainsKey(propertyName) == false)
            {
                values[propertyName] = default;
            }

            var value = values[propertyName];
            return value == null ? default : (T)value;
        }

        protected T GetValue<T>([CallerMemberName] string propertyName = "")
        {
            var rightsKey = $"{this.className}.{propertyName}";

            return this.GetPropertyValueInternal<T>(propertyName);
        }

        protected void SetValueUnchecked<T>(T value, [CallerMemberName] string propertyName = "")
        {
            if (this.values.ContainsKey(propertyName) == true)
            {
                this.values[propertyName] = value;
            }
            else
            {
                this.values.TryAdd(propertyName, value);
            }
        }

        protected void SetValue<T>(T value, Func<T, string, bool> preAction, Action<T, string> postAction, [CallerMemberName] string propertyName = "")
        {
            if (preAction?.Invoke(value, propertyName) == true)
            {
                this.SetValue(value, propertyName);
            }

            if (postAction != null)
            {
                postAction?.Invoke(value, propertyName);
            }
        }

        protected void SetValue<T>(T value, Action<T, string> postAction, [CallerMemberName] string propertyName = "")
        {
            this.SetValue(value, propertyName);
            if (postAction != null)
            {
                postAction?.Invoke(value, propertyName);
            }
        }

        protected void SetValue<T>(T value, [CallerMemberName] string propertyName = "")
        {
            bool changed = !object.Equals(value, this.GetPropertyValueInternal<T>(propertyName));
            if (changed == true)
            {
                this.IsPropertyChanged = true;
                var rightsKey = $"{this.className}.{propertyName}";
                this.values[propertyName] = value;
                this.OnPropertyChanged(propertyName);
            }
        }
        #endregion Get/Set Implementierung

        #region INotifyPropertyChanged Implementierung
        protected void SetProperty<T>(ref T oldValue, T newValue, [CallerMemberName] string property = "")
        {
            if (object.Equals(oldValue, newValue))
            {
                return;
            }

            oldValue = newValue;
            this.OnPropertyChanged(property);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged Implementierung

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod(int level = 1)
        {
            var st = new StackTrace();
            var sf = st.GetFrame(level);

            return sf.GetMethod().Name;
        }

        internal void SetValue(string value, Action<int, string> refreshData)
        {
            throw new NotImplementedException();
        }
    }
}
