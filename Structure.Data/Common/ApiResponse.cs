using System;
using System.Collections.Generic;
using System.Text;

namespace Structure.Data.Common;
public class ApiResponse<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();

    // Success Factories
    public static ApiResponse<T> Success(T data, string message = "Success")
        => new() { Data = data, IsSuccess = true, Message = message };

    // Failure Factories
    public static ApiResponse<T> Failure(string message, List<string>? errors = null)
        => new() { IsSuccess = false, Message = message, Errors = errors ?? new List<string>() };
}