﻿namespace VENative.ChromaDB.Client.Tests;

internal class TestHttpMessageHandler : HttpMessageHandler
{
    IDictionary<string, Func<HttpRequestMessage, HttpResponseMessage>> _calls;
    public TestHttpMessageHandler(IDictionary<string, Func<HttpRequestMessage, HttpResponseMessage>> mockCalls) 
    {
        _calls = mockCalls;
    }
  
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_calls.ContainsKey(request.RequestUri.AbsolutePath))
        {
            return Task.Run(() => _calls[request.RequestUri.AbsolutePath](request));
        }
        throw new Exception("Endpoint not mocked.");
    }
}
