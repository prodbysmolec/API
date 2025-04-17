using System;

namespace Artikelsystem.Shared;

public class ApiResponseDTO
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object? Data { get; set; }
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Optional constructor for successful responses
    public ApiResponseDTO(object? data = null, string message = "Operation successful")
    {
        Success = true;
        Message = message;
        Data = data;
        StatusCode = 200;
    }

    // Constructor for error responses
    public ApiResponseDTO(string errorMessage, int statusCode = 400)
    {
        Success = false;
        Message = errorMessage;
        StatusCode = statusCode;
    }

}
