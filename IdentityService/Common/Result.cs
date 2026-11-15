namespace IdentityService.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public int Status { get; }
        public string? Error { get; }

        protected Result(bool isSuccess, int status, string? error)
        {
            IsSuccess = isSuccess;
            Status = status;
            Error = error;
        }

        public static Result Success() => new(true, 200, null);
        public static Result Failure(int status, string error) => new(false, status, error);
    }
    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(T? value, bool isSuccess, int status, string? error)
            : base(isSuccess, status, error) => Value = value;

        public static Result<T> Success(T value) => new(value, true, 200, null);
        public new static Result<T> Failure(int status, string error) => new(default, false, status, error);
    }
}
