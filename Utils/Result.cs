using AllRiskSolutions_Desafio.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AllRiskSolutions_Desafio.Utils;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFail => !IsSuccess;
    public readonly ResultError Error;

    protected Result(bool isSuccess, ResultError error)
    {
        if (isSuccess && !error.IsEmpty())
            throw new ArgumentException("Success result can't have error");
        if (!isSuccess && error.IsEmpty())
            throw new ArgumentException("Error result must have error");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, ResultError.Empty());

    public static Result<T> Success<T>(T value)
        => new Result<T>(value, true, ResultError.Empty());

    public static Result Fail(ResultError error) => new(false, error);

    public static Result<T> Fail<T>(ResultError error)
        => new Result<T>(default!, false, error);

    public static Result Fail(string message, string code) =>
        new(false, new ResultError(message, code));

    public static Result<T> Fail<T>(string message, string code)
        => new Result<T>(default!, false, new ResultError(message, code));

    public static implicit operator ActionResult(Result result)
    {
        return result.IsSuccess
            ? new OkObjectResult(null)
            : new ObjectResult(result.Error.Message) { StatusCode = int.Parse(result.Error.Code) };
    }
}

public class Result<T> : Result
{
    private readonly T _value;

    protected internal Result(T value, bool isSuccess, ResultError error) : base(isSuccess, error)
    {
        _value = value;
    }

    public T Value()
    {
        return IsSuccess
            ? _value
            : throw new InvalidOperationException("Can't access value on error result");
    }


    public static implicit operator Result<T>(T value) => Success(value);

    public static implicit operator ActionResult<T>(Result<T> result)
    {
        return result.IsSuccess
            ? new OkObjectResult(result.Value())
            : new ObjectResult(result.Error.Message) { StatusCode = int.Parse(result.Error.Code) };
    }
}

public record ResultError(string Message, string Code)
{
    public static ResultError Empty() => new ResultError(string.Empty, string.Empty);

    public bool IsEmpty()
    {
        return this == ResultError.Empty();
    }
}