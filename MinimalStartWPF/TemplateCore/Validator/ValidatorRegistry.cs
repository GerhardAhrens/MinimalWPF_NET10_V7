namespace System.Windows
{
    using System.Collections.Concurrent;

    public sealed class ValidatorRegistry
    {
        private readonly ConcurrentDictionary<Type, object> _validators = new();

        public int Count { get { return this._validators.Count; } }

        public string[] Names { get { return this._validators.Keys.Select(k => k.Name).ToArray(); } }
        public void Register<TValidator>(TValidator validator)
            where TValidator : class
        {
            this._validators[typeof(TValidator)] = validator;
        }

        public bool TryGet<TValidator>(out TValidator validator) where TValidator : class
        {
            if (this._validators.TryGetValue(typeof(TValidator), out var value))
            {
                validator = (TValidator)value;
                return true;
            }

            validator = null;
            return false;
        }
    }
}
