using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VENative.ChromaDB.Client;

internal static class HttpClientJsonExtensions
{
    public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string url, T request, CancellationToken cancellationToken = default)
    {
        HttpContent requestContent = new StringContent(JsonConvert.SerializeObject(request), encoding: Encoding.UTF8, "application/json");
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = requestContent
        };
        return httpClient.SendAsync(requestMessage, cancellationToken);
    }

    public static Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient httpClient, string url, T request, CancellationToken cancellationToken = default)
    {
        HttpContent requestContent = new StringContent(JsonConvert.SerializeObject(request), encoding: Encoding.UTF8, "application/json");
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = requestContent
        };
        return httpClient.SendAsync(requestMessage, cancellationToken);
    }
}
