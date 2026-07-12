namespace System.Windows
{
    using System.Collections.Immutable;
    using System.Diagnostics;

    public abstract record DomainResult<T>
    {
        DomainResult() { }

        [DebuggerDisplay("Ok: {Item}")]
        public sealed record Ok(T Item) : DomainResult<T>;

        [DebuggerDisplay("Fehler: Erster Fehler: {Errors[0]}")]
        public sealed record Failure(ImmutableArray<Error> Errors) : DomainResult<T>;

        public static implicit operator DomainResult<T>(T Item) => new Ok(Item);
        public static implicit operator DomainResult<T>(string errorMessage) => new Failure(ImmutableArray.Create<Error>(errorMessage));

        public ImmutableArray<Error> GetErrors() => this is Failure f ? f.Errors : ImmutableArray<Error>.Empty;
    }

    [DebuggerDisplay("{Message}, {ChildErrors.Length} Untergeordnete Fehler")]
    public record Error(string Message, ImmutableArray<Error> ChildErrors)
    {
        public static implicit operator Error(string errorMessage) => new(errorMessage, ImmutableArray<Error>.Empty);
    }
}
