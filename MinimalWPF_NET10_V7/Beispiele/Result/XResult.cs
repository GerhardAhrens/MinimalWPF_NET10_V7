namespace MinimalWPF.Beispiele
{
    using System.Collections.Immutable;
    using System.Diagnostics;

    public abstract record XResult<T>
    {
        XResult() { }

        [DebuggerDisplay("Ok: {Item}")]
        public sealed record Ok(T Item) : XResult<T>;

        [DebuggerDisplay("Fehler: Erster Fehler: {Errors[0]}")]
        public sealed record Failure(ImmutableArray<Fail> Fail) : XResult<T>;

        public static implicit operator XResult<T>(T Item) => new Ok(Item);
        public static implicit operator XResult<T>(string errorMessage) => new Failure(ImmutableArray.Create<Fail>(errorMessage));
    }

    [DebuggerDisplay("{Meldung}, {ChildErrors.Length} Untergeordnete Fehler")]
    public record Fail(string Message, ImmutableArray<Fail> ChildErrors)
    {
        public static implicit operator Fail(string errorMessage) => new(errorMessage, ImmutableArray<Fail>.Empty);
    }
}
