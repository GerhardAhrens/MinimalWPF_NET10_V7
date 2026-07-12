namespace System.Windows
{
    using System.Collections.Immutable;
    using System.Reflection;

    public static class Create
    {
        public static DomainResult<TResult> From<TIn0, TIn1, TResult>(Func<TIn0, TIn1, DomainResult<TResult>> create,
            DomainResult<TIn0> arg0Result,
            DomainResult<TIn1> arg1Result)
        {
            if (arg0Result is DomainResult<TIn0>.Ok { Item: var arg0 } && arg1Result is DomainResult<TIn1>.Ok { Item: var arg1 })
            {
                return create(arg0, arg1);
            }

            var parameters = create.Method.GetParameters();
            List<Error> errors = new();

            AddErrors(arg0Result, parameters[0], errors);
            AddErrors(arg1Result, parameters[1], errors);

            return new DomainResult<TResult>.Failure(errors.ToImmutableArray());
        }

        public static DomainResult<(TIn0, TIn1)> From<TIn0, TIn1, TResult>(DomainResult<TIn0> arg0Result, DomainResult<TIn1> arg1Result)
        {
            if (arg0Result is DomainResult<TIn0>.Ok { Item: var arg0 } && arg1Result is DomainResult<TIn1>.Ok { Item: var arg1 })
            {
                return (arg0, arg1);
            }

            List<Error> errors = new();

            AddErrors(arg0Result, 0, errors);
            AddErrors(arg1Result, 1, errors);

            return new DomainResult<(TIn0, TIn1)>.Failure(errors.ToImmutableArray());
        }

        static void AddErrors<T>(DomainResult<T> result, ParameterInfo parameterInfo, List<Error> errors)
        {
            if (result is DomainResult<T>.Failure { Errors: var resultErrors })
            {
                errors.Add(new($"Parameter '{parameterInfo.Name}' für den Typ {parameterInfo.ParameterType.Name} wird nicht unterstützt", resultErrors));
            }
        }

        static void AddErrors<T>(DomainResult<T> result, int paramIndex, List<Error> errors)
        {
            if (result is DomainResult<T>.Failure { Errors: var resultErrors })
            {
                errors.Add(new($"Parameter '{paramIndex}' für den Typ {typeof(T).Name} wird nicht unterstützt", resultErrors));
            }
        }
    }
}
