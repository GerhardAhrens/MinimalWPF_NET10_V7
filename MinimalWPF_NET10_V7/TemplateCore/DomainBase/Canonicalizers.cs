namespace System.Windows
{
    public static class Canonicalizers
    {
        public static Func<T, T> AllOf<T>(params Func<T, T>[] canonicalizers) =>
            str => canonicalizers.Aggregate(str, (currentStr, canonicalize) => canonicalize(currentStr));

        public static Func<T, T> None<T>() => str => str;
    }
}
