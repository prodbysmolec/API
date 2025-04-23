using System;
using System.Net;

namespace Client.Core.Services.ApiClient;

public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public ApiException(string message, HttpStatusCode statusCode)
            : base(message)
    {
        StatusCode = statusCode;
    }

    public ApiException(string message, HttpStatusCode statusCode, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    public bool IsUnauthorized => StatusCode == HttpStatusCode.Unauthorized;

    public bool IsForbidden => StatusCode == HttpStatusCode.Forbidden;

    public bool IsNotFound => StatusCode == HttpStatusCode.NotFound;

    public bool IsServerError => (int)StatusCode >= 500;

}
