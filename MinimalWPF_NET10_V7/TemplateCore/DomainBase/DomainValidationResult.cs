namespace System.Windows
{
    public class DomainValidationResult
    {
        public static DomainValidationResult<T> Success<T>() => new DomainValidationResult<T>.Success();

        public static DomainValidationResult<T> Failure<T>(string errorMessage, T invalidValue) =>
            new DomainValidationResult<T>.Failure(errorMessage, invalidValue);
    }

    public abstract class DomainValidationResult<T>
    {
        private DomainValidationResult()
        {
        }

        public sealed class Success: DomainValidationResult<T>
        {
            public Success()
            {
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1051:Sichtbare Instanzfelder nicht deklarieren", Justification = "<Ausstehend>")]
        public sealed class Failure : DomainValidationResult<T>
        {
            public readonly string ErrorMessage;
            public readonly T OriginalValue;

            public Failure(string errorMessage, T invalidValue)
            {
                ErrorMessage = errorMessage;
                OriginalValue = invalidValue;
            }
        }
    }
}
