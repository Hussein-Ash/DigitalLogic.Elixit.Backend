namespace Elixir.Generic;

public class GenericResponse<T>
{
    public int Code { get; set; } 
    public string Message { get; set; } = string.Empty; 
    public T? Data { get; set; } 
    public PaginationMetadata? Pagination { get; set; }
    public List<string>? Errors { get; set; } 

    // Success response
    public static GenericResponse<T> Success(T data, string message = "Operation successful", int code = 200)
    {
        return new GenericResponse<T>
        {
            Code = code,
            Message = message,
            Data = data
        };
    }

    public static GenericResponse<T> Success(T data, PaginationMetadata pagination, string message = "Operation successful", int code = 200)
    {
        return new GenericResponse<T>
        {
            Code = code,
            Message = message,
            Data = data,
            Pagination = pagination
        };
    }

    // Failure response
    public static GenericResponse<T> Failure(string message, int code = 500, List<string>? errors = null)
    {
        return new GenericResponse<T>
        {
            Code = code,
            Message = message,
            Data = default,
            Errors = errors
        };
    }
}

public class PaginationMetadata
{
    public int TotalItems { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public PaginationMetadata(int totalItems, int pageSize, int currentPage)
    {
        TotalItems = totalItems;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
    }
}
