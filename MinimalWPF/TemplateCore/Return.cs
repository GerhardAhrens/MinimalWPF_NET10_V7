namespace NotNullDemo
{
    public struct Return<TResult>
    {
        public bool IsSuccess { get; private set; }
        public bool IsFail { get; private set; }

        public TResult? Value { get; private set; }

        public string FailMessage { get; private set; } 

        public string SuccessMessage { get; private set; }

        public Exception? Exception { get; private set; } 

        public static Return<TResult> Success(TResult result, string successMessage)
        {
            return new Return<TResult>
            {
                IsSuccess = true, 
                IsFail = false,
                Value = result,
                SuccessMessage = successMessage
            };
        }

        public static Return<TResult> Success(TResult result)
        {
            return new Return<TResult>
            {
                IsSuccess = true,
                IsFail = false,
                Value = result,
                SuccessMessage = string.Empty
            };
        }


        public static Return<TResult> Fail()
        {
            return new Return<TResult>
            {
                IsSuccess = false,
                Value = default(TResult),
                FailMessage = string.Empty
            };
        }

        public static Return<TResult> Fail(string nonMessage)
        {
            return new Return<TResult>
            {
                IsSuccess = false,
                FailMessage = nonMessage,
                Exception = null
            };
        }

        public static Return<TResult> Fail(Exception ex)
        {
            return new Return<TResult>
            {
                IsSuccess = false,
                FailMessage = $"{ex.Message}{Environment.NewLine}{ex.StackTrace}",
                Exception = ex
            };
        }

        public static Return<TResult> Fail(Exception ex, string nonMessage)
        {
            return new Return<TResult>
            {
                IsSuccess = false,
                FailMessage = $"{ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}{nonMessage}",
                Exception = ex
            };
        }
    }
}