using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommenResponse
{
    public class Result
    {
        private readonly List<Error> _errors = [];
        // isSuccess
        public bool IsSuccess => _errors.Count == 0;
        // isFail
        public bool IsFailure => !IsSuccess;
        // Errors
        public IReadOnlyList<Error> Errors => _errors;

        // IsSuccess
        protected Result() { }

        // Failure With one Only Failure
        protected Result(Error error)
        {
            _errors.Add(error);
        }

        // Failure with errors
        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }

        public static Result Ok() => new();
        public static Result Fail(Error error) => new(error);
        public static Result Fail(List<Error> errors) => new(errors); 
    }

    public class Result<T> : Result
    {
        public T? Data { get; }

        private Result(T data)
        {
            Data = data;
        }

        private Result(Error error) : base(error)
        {
        }

        private Result(List<Error> errors) : base(errors)
        {
        }

        // Success with data
        public static Result<T> Ok(T data) => new(data);

        // Failure with one error
        public new static Result<T> Fail(Error error) => new(error);

        // Failure with errors
        public new static Result<T> Fail(List<Error> errors) => new(errors);

        public static implicit operator Result<T>(T data) => Ok(data);
        public static implicit operator Result<T>(Error error) => Fail(error);
        public static implicit operator Result<T>(List<Error> errors) => Fail(errors);
    }

}
