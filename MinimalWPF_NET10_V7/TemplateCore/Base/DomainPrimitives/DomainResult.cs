namespace System.Windows.Domain
{
    using System;
    using System.Collections.Generic;

    public sealed record ResultError(string Code, string Message, Exception Exception = null);

    public class DomainResult
    {
        private readonly List<ResultError> _errors = new();

        public bool Success => _errors.Count == 0;

        public IReadOnlyCollection<ResultError> Errors
            => _errors;

        protected DomainResult()
        {
        }

        protected DomainResult(ResultError error)
        {
            _errors.Add(error);
        }

        protected DomainResult(IEnumerable<ResultError> errors)
        {
            _errors.AddRange(errors);
        }

        public static DomainResult Ok()
            => new();

        public static DomainResult Fail(ResultError error) => new(error);

        public static DomainResult Fail(IEnumerable<ResultError> errors)  => new(errors);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Statische Member nicht in generischen Typen deklarieren", Justification = "<Ausstehend>")]
    public sealed class DomainResult<T> : DomainResult
    {
        public T Value { get; }

        private DomainResult(T value)
        {
            Value = value;
        }

        private DomainResult(ResultError error) : base(error)
        {
        }

        private DomainResult(IEnumerable<ResultError> errors) : base(errors)
        {
        }

        public static DomainResult<T> Ok(T value)  => new(value);

        public static new DomainResult<T> Fail(ResultError error) => new(error);

        public static new DomainResult<T> Fail(IEnumerable<ResultError> errors) => new(errors);
    }
}
