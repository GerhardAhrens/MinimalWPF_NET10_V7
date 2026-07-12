namespace System.Windows
{
    public static class ValidationResultExtensions
    {
        public static T Match<TValue, T>(this DomainValidationResult<TValue> result, Func<T> onSuccess, Func<DomainValidationResult<TValue>.Failure, T> onError) =>
               result is DomainValidationResult<TValue>.Failure error ? onError(error) : onSuccess();
    }
}
