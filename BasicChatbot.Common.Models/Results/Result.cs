using BasicChatbot.Common.Models.Enums;

namespace BasicChatbot.Common.Models.Results;

public class Result
{
	public ResultStatus Status { get; }

	public bool IsSuccess => Status == ResultStatus.Success;

	public string? ErrorMessage { get; }

	protected Result(ResultStatus status, string? errorMessage = null)
	{
		Status = status;
		ErrorMessage = errorMessage;
	}

	public static Result Success()
		=> new(ResultStatus.Success);

	public static Result Failure(FailureModel failure)
		=> new(failure.Status, failure.ErrorMessage);
}

public class Result<T> : Result
{
	public T? Data { get; }

	private Result(
		ResultStatus status,
		T? data = default,
		string? errorMessage = null)
		: base(status, errorMessage)
	{
		Data = data;
	}

	public static Result<T> Success(T data)
		=> new(ResultStatus.Success, data);

	public new static Result<T> Failure(FailureModel failure) 
		=> new(failure.Status, default, failure.ErrorMessage);
}

public class SuccessModel
{
	public string? ApiMessage { get; set; }

	public ResultStatus Status { get; init; }
		= ResultStatus.Success;
}

public class FailureModel
{
	public string? ErrorMessage { get; set; }

	public ResultStatus Status { get; init; }
		= ResultStatus.Failure;
}