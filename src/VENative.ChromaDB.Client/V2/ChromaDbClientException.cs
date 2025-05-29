using System;
using System.Net;

namespace VENative.ChromaDB.Client.V2;

public class ChromaDbClientException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string? ErrorType { get; }

    public ChromaDbClientException(string? message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string? errorType = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorType = errorType;
    }
}