using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LinkO.Shared.CommonResult
{
    public class Result
    {
        // Is Success
        // Is Failure
        // Errors [Code - Desc - Type] => new class

        protected readonly List<Error> _errors = [];

        public bool IsSuccess => _errors.Count == 0;

        public bool IsFailure => !IsSuccess;

        // property to get Errors

        public IReadOnlyList<Error> Errors => _errors;


        // No Errors =>
        protected Result()
        {
            
        }
        // One Error Occured =>
        protected Result(Error error)
        {
            _errors.Add(error);
        }
        // More Than one Erros Occured =>
        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }
        // Methods to set Values to CTORs
        public static Result Ok() => new Result();
        // If One Error Occured 
        public static Result Fail(Error error) => new Result(error);
        // If two or more Erros Occured
        public static Result Fail(List<Error> errors) => new Result(errors);

    }

    public class Result<TValue> : Result
    {
        private readonly TValue _value;
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("Cannot Access The Value Of a Failed Result");

        private Result(TValue value)
        {
            _value = value;
        }
        private Result(Error error) : base(error)
        {
            _value = default!;
        }
        private Result(List<Error> errors) : base(errors)
        {
            _value = default!;
        }

        // static Factory Method
        public static Result<TValue> Ok(TValue value) => new Result<TValue>(value);
        public static Result<TValue> Fail(Error error) => new Result<TValue>(error);
        public static Result<TValue> Fail(List<Error> errors) => new Result<TValue>(errors);

        // Implict Casting
        public static implicit operator Result<TValue>(TValue value) => Ok(value);
        public static implicit operator Result<TValue>(Error error) => Fail(error);
        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);

    }
}
