namespace System.Windows
{
    public static class DomainValidators
    {
        public static Func<T, DomainValidationResult<T>> AllOf<T>(params Func<T, DomainValidationResult<T>>[] validators) =>
            str => validators
                .Select(validate => validate(str))
                .FirstOrDefault(result => result is DomainValidationResult<T>.Failure) is DomainValidationResult<T>.Failure error
                    ? error : DomainValidationResult.Success<T>();

        public static Func<T, DomainValidationResult<T>> None<T>() => str => DomainValidationResult.Success<T>();
    }
}
