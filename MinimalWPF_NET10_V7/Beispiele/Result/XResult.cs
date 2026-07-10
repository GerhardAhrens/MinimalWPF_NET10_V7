namespace MinimalWPF.Beispiele
{
    using System.Collections.Immutable;
    using System.Diagnostics;

    public abstract record XResult<T>
    {
        XResult() { }

        [DebuggerDisplay("Ok: {Value}")]
        public sealed record Ok(T Value) : XResult<T>;

        [DebuggerDisplay("Fehler: Erster Fehler: {Errors[0]}")]
        public sealed record Failure(ImmutableArray<Fail> Errors) : XResult<T>;

        public static implicit operator XResult<T>(T Value) => new Ok(Value);

        public static implicit operator XResult<T>(string errorMessage) => new Failure(ImmutableArray.Create<Fail>(errorMessage));
    }

    [DebuggerDisplay("{Message}, {ChildErrors.Length} Untergeordnete Fehler")]
    public record Fail(string Message, ImmutableArray<Fail> ChildErrors)
    {
        public static implicit operator Fail(string errorMessage) => new(errorMessage, ImmutableArray<Fail>.Empty);
    }
}
