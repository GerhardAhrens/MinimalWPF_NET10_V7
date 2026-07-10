namespace System.Windows
{
    public interface IValidator<in T>
    {
        ValidationResult Validate(T value);
    }
}
