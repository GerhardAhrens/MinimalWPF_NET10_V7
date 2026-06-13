namespace System.Windows
{
    public sealed class Messenger
    {
        private readonly Dictionary<Type, List<Delegate>> _handlers = new();

        public void Register<TMessage, TResult>(Func<TMessage, TResult> handler)
        {
            if (!_handlers.TryGetValue(typeof(TMessage), out var list))
            {
                list = [];
                _handlers.Add(typeof(TMessage), list);
            }

            list.Add(handler);
        }

        public TResult SendRequest<TMessage, TResult>(TMessage message)
        {
            if (_handlers.TryGetValue(typeof(TMessage), out var list))
            {
                var handler = list.Cast<Func<TMessage, TResult>>().FirstOrDefault();

                if (handler != null)
                {
                    return handler(message);
                }
            }

            return default;
        }
    }
}
