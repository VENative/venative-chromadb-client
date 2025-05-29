using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace VENative.ChromaDB.Client.V2;

public class ChromaDbClient : IChromaDbClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions _jsonOptions;

    const string default_tenant = "default_tenant";
    const string default_database = "default_database";

    public ChromaDbClient(HttpClient httpClient, string baseUrl, string? apiToken = null)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _baseUrl = baseUrl.TrimEnd('/');
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        if (!string.IsNullOrEmpty(apiToken))
        {
            _httpClient.DefaultRequestHeaders.Add("x-chroma-token", apiToken);
        }
    }

    private async Task<T> SendRequestAsync<T>(HttpMethod method, string endpoint, object? content = null, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(method, $"{_baseUrl}{endpoint}");

        if (content != null)
        {
            var json = JsonSerializer.Serialize(content, _jsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonSerializer.Deserialize<ErrorResponse>(responseContent, _jsonOptions)
                        ?? new ErrorResponse { Error = response.StatusCode.ToString(), Message = responseContent };
            throw new ChromaDbClientException(error.Message, response.StatusCode, error.Error);
        }

        return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions)
               ?? throw new ChromaDbClientException("Failed to deserialize response");
    }

    public Task<string> HealthCheckAsync(CancellationToken cancellationToken = default) =>
        SendRequestAsync<string>(HttpMethod.Get, "/api/v2/healthcheck", null, cancellationToken);

    public Task<HeartbeatResponse> HeartbeatAsync(CancellationToken cancellationToken = default) =>
        SendRequestAsync<HeartbeatResponse>(HttpMethod.Get, "/api/v2/heartbeat", null, cancellationToken);

    public Task<ChecklistResponse> PreFlightChecksAsync(CancellationToken cancellationToken = default) =>
        SendRequestAsync<ChecklistResponse>(HttpMethod.Get, "/api/v2/pre-flight-checks", null, cancellationToken);

    public Task<bool> ResetAsync(CancellationToken cancellationToken = default) =>
        SendRequestAsync<bool>(HttpMethod.Post, "/api/v2/reset", null, cancellationToken);

    public Task<string> VersionAsync(CancellationToken cancellationToken = default) =>
        SendRequestAsync<string>(HttpMethod.Get, "/api/v2/version", null, cancellationToken);

    public Task<GetUserIdentityResponse> GetUserIdentityAsync(CancellationToken cancellationToken = default) =>
        SendRequestAsync<GetUserIdentityResponse>(HttpMethod.Get, "/api/v2/auth/identity", null, cancellationToken);

    public Task<CreateTenantResponse> CreateTenantAsync(CreateTenantPayload payload, CancellationToken cancellationToken = default) =>
        SendRequestAsync<CreateTenantResponse>(HttpMethod.Post, "/api/v2/tenants", payload, cancellationToken);

    public Task<GetTenantResponse> GetTenantAsync(string tenantName, CancellationToken cancellationToken = default) =>
        SendRequestAsync<GetTenantResponse>(HttpMethod.Get, $"/api/v2/tenants/{Uri.EscapeDataString(tenantName)}", null, cancellationToken);

    public Task<IEnumerable<Database>> ListDatabasesAsync(string tenant, int? limit = null, int? offset = null, CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString(("limit", limit), ("offset", offset));
        return SendRequestAsync<IEnumerable<Database>>(
            HttpMethod.Get,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases{query}",
            null,
            cancellationToken);
    }

    public Task<CreateDatabaseResponse> CreateDatabaseAsync(string tenant, CreateDatabasePayload payload, CancellationToken cancellationToken = default) =>
        SendRequestAsync<CreateDatabaseResponse>(
            HttpMethod.Post,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases",
            payload,
            cancellationToken);

    public Task<Database> GetDatabaseAsync(string tenant, string database, CancellationToken cancellationToken = default) =>
        SendRequestAsync<Database>(
            HttpMethod.Get,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases/{Uri.EscapeDataString(database)}",
            null,
            cancellationToken);

    public Task<DeleteDatabaseResponse> DeleteDatabaseAsync(string tenant, string database, CancellationToken cancellationToken = default) =>
        SendRequestAsync<DeleteDatabaseResponse>(
            HttpMethod.Delete,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases/{Uri.EscapeDataString(database)}",
            null,
            cancellationToken);

    public Task<IEnumerable<Collection>> ListCollectionsAsync(CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString(("limit", null), ("offset", null));
        return SendRequestAsync<IEnumerable<Collection>>(
            HttpMethod.Get,
            $"/api/v2/tenants/{Uri.EscapeDataString(default_tenant)}/databases/{Uri.EscapeDataString(default_database)}/collections{query}",
            null,
            cancellationToken);
    }

    public Task<IEnumerable<Collection>> ListCollectionsAsync(string tenant, string database, int? limit = null, int? offset = null, CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString(("limit", limit), ("offset", offset));
        return SendRequestAsync<IEnumerable<Collection>>(
            HttpMethod.Get,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases/{Uri.EscapeDataString(database)}/collections{query}",
            null,
            cancellationToken);
    }

    public Task<int> CountCollectionsAsync(string tenant, string database, CancellationToken cancellationToken = default) =>
        SendRequestAsync<int>(
            HttpMethod.Get,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases/{Uri.EscapeDataString(database)}/collections_count",
            null,
            cancellationToken);

    public Task<ICollectionClient> CreateCollectionAsync(CreateCollectionPayload payload, CancellationToken cancellationToken = default)
    {
        return CreateCollectionAsync(default_tenant, default_database, payload, cancellationToken);
    }

    public async Task<ICollectionClient> CreateCollectionAsync(string tenant, string database, CreateCollectionPayload payload, CancellationToken cancellationToken = default)
    {
        var collection = await SendRequestAsync<Collection>(
            HttpMethod.Post,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases/{Uri.EscapeDataString(database)}/collections",
            payload,
            cancellationToken);

        return new CollectionClient(this, tenant, database, collection.Id);
    }

    public async Task<ICollectionClient> GetCollectionByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        IEnumerable<Collection> collections = await ListCollectionsAsync(cancellationToken);
        Collection? collection = collections.FirstOrDefault(x => x.Name == name);

        if (collection is null)
        {
            throw new ChromaDbClientException($"Collection with name '{name}' doesn't exists");
        }

        return new CollectionClient(this, default_tenant, default_database, collection.Id);
    }

    public Task<ICollectionClient> GetCollectionByIdAsync(string collectionId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<ICollectionClient>(new CollectionClient(this, default_tenant, default_database, collectionId));
    }

    public Task<ICollectionClient> GetCollectionAsync(string tenant, string database, string collectionId, CancellationToken cancellationToken = default)
    {
        // Validamos que la colección exista
        return Task.FromResult<ICollectionClient>(new CollectionClient(this, tenant, database, collectionId));
    }

    // Métodos internos para operaciones de colecciones (usados por CollectionClient)
    internal Task<Collection> GetCollectionInternalAsync(string tenant, string database, string collectionId, CancellationToken cancellationToken = default) =>
        SendRequestAsync<Collection>(
            HttpMethod.Get,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases/{Uri.EscapeDataString(database)}/collections/{Uri.EscapeDataString(collectionId)}",
            null,
            cancellationToken);

    internal Task<UpdateCollectionResponse> UpdateCollectionInternalAsync(string tenant, string database, string collectionId, UpdateCollectionPayload payload, CancellationToken cancellationToken = default) =>
        SendRequestAsync<UpdateCollectionResponse>(
            HttpMethod.Put,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases/{Uri.EscapeDataString(database)}/collections/{Uri.EscapeDataString(collectionId)}",
            payload,
            cancellationToken);

    public Task<UpdateCollectionResponse> DeleteCollectionAsync(string collectionId, CancellationToken cancellationToken = default)
    {
        return DeleteCollectionInternalAsync(default_tenant, default_database, collectionId, cancellationToken);
    }

    internal Task<UpdateCollectionResponse> DeleteCollectionInternalAsync(string tenant, string database, string collectionId, CancellationToken cancellationToken = default) =>
        SendRequestAsync<UpdateCollectionResponse>(
            HttpMethod.Delete,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases/{Uri.EscapeDataString(database)}/collections/{Uri.EscapeDataString(collectionId)}",
            null,
            cancellationToken);

    internal Task<Collection> ForkCollectionInternalAsync(string tenant, string database, string collectionId, ForkCollectionPayload payload, CancellationToken cancellationToken = default) =>
        SendRequestAsync<Collection>(
            HttpMethod.Post,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases/{Uri.EscapeDataString(database)}/collections/{Uri.EscapeDataString(collectionId)}/fork",
            payload,
            cancellationToken);

    internal Task<AddCollectionRecordsResponse> AddRecordsInternalAsync(string tenant, string database, string collectionId, AddCollectionRecordsPayload payload, CancellationToken cancellationToken = default) =>
        SendRequestAsync<AddCollectionRecordsResponse>(
            HttpMethod.Post,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases/{Uri.EscapeDataString(database)}/collections/{Uri.EscapeDataString(collectionId)}/add",
            payload,
            cancellationToken);

    internal Task<int> CountRecordsInternalAsync(string tenant, string database, string collectionId, CancellationToken cancellationToken = default) =>
        SendRequestAsync<int>(
            HttpMethod.Get,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases/{Uri.EscapeDataString(database)}/collections/{Uri.EscapeDataString(collectionId)}/count",
            null,
            cancellationToken);

    internal Task<DeleteCollectionRecordsResponse> DeleteRecordsInternalAsync(string tenant, string database, string collectionId, DeleteCollectionRecordsPayload payload, CancellationToken cancellationToken = default) =>
        SendRequestAsync<DeleteCollectionRecordsResponse>(
            HttpMethod.Post,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases/{Uri.EscapeDataString(database)}/collections/{Uri.EscapeDataString(collectionId)}/delete",
            payload,
            cancellationToken);

    internal Task<GetResponse> GetRecordsInternalAsync(string tenant, string database, string collectionId, GetRequestPayload payload, CancellationToken cancellationToken = default) =>
        SendRequestAsync<GetResponse>(
            HttpMethod.Post,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases/{Uri.EscapeDataString(database)}/collections/{Uri.EscapeDataString(collectionId)}/get",
            payload,
            cancellationToken);

    internal Task<QueryResponse> QueryRecordsInternalAsync(string tenant, string database, string collectionId, QueryRequestPayload payload, int? limit = null, int? offset = null, CancellationToken cancellationToken = default)
    {
        var query = BuildQueryString(("limit", limit), ("offset", offset));
        return SendRequestAsync<QueryResponse>(
            HttpMethod.Post,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases/{Uri.EscapeDataString(database)}/collections/{Uri.EscapeDataString(collectionId)}/query{query}",
            payload,
            cancellationToken);
    }

    internal Task<UpdateCollectionRecordsResponse> UpdateRecordsInternalAsync(string tenant, string database, string collectionId, UpdateCollectionRecordsPayload payload, CancellationToken cancellationToken = default) =>
        SendRequestAsync<UpdateCollectionRecordsResponse>(
            HttpMethod.Post,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases/{Uri.EscapeDataString(database)}/collections/{Uri.EscapeDataString(collectionId)}/update",
            payload,
            cancellationToken);

    internal Task<UpsertCollectionRecordsResponse> UpsertRecordsInternalAsync(string tenant, string database, string collectionId, UpsertCollectionRecordsPayload payload, CancellationToken cancellationToken = default) =>
        SendRequestAsync<UpsertCollectionRecordsResponse>(
            HttpMethod.Post,
            $"/api/v2/tenants/{Uri.EscapeDataString(tenant)}/databases/{Uri.EscapeDataString(database)}/collections/{Uri.EscapeDataString(collectionId)}/upsert",
            payload,
            cancellationToken);

    private string BuildQueryString(params (string Key, object? Value)[] parameters)
    {
        var query = new StringBuilder();
        bool first = true;

        foreach (var (key, value) in parameters)
        {
            if (value == null) continue;

            if (first)
            {
                query.Append('?');
                first = false;
            }
            else
            {
                query.Append('&');
            }

            query.Append(Uri.EscapeDataString(key));
            query.Append('=');
            query.Append(Uri.EscapeDataString(value.ToString()!));
        }

        return query.ToString();
    }
}