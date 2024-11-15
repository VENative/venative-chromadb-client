﻿using VENative.ChromaDB.Client.Embeddings;
using VENative.ChromaDB.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace VENative.ChromaDB.Client;

public class ChromaDBClient : IChromaDBClient
{
    private const string DEFAULT_TENANT = "default_tenant";
    private const string DEFAULT_DATABASE = "default_database";
    private readonly HttpClient _httpClient;
    private readonly string _tenant = DEFAULT_TENANT;
    private readonly string _database = DEFAULT_DATABASE;
    //public ChromaDBClient(HttpClient httpClient)
    //{
    //    _httpClient = httpClient;
    //}

    public ChromaDBClient(HttpClient httpClient, string tenant = DEFAULT_TENANT, string database = DEFAULT_DATABASE)
    {
        _httpClient = httpClient;
        _tenant = tenant;
        _database = database;
    }

    public ICollectionClient CreateCollection(string name, IDictionary<string, object>? metadata = null, IEmbeddable? embeddingFunction = null, bool createOrGet = false)
    {
        Task<ICollectionClient> collectionTask = Task.Run(() => CreateCollectionAsync(name, metadata, embeddingFunction, createOrGet));
        return collectionTask.Result;
    }

    public async Task<ICollectionClient> CreateCollectionAsync(string name, IDictionary<string, object>? metadata = null, IEmbeddable? embeddingFunction = null, bool getOrCreate = false, CancellationToken cancellationToken = default)
    {
        CreateCollectionRequest request = new CreateCollectionRequest
        {
            Name = name,
            Metadata = metadata,
            GetOrCreate = getOrCreate
        };
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/v1/collections", request, cancellationToken).ConfigureAwait(false);
        string content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error creating collection: {content}");
        }

        Collection collection = JsonConvert.DeserializeObject<Collection>(content) 
            ?? throw new Exception($"Create Collection returned invalid response {content}");
        return new CollectionClient(_httpClient, collection, embeddingFunction);
    }

    public void DeleteCollection(string name)
    {
        Task deleteTask = Task.Run( () => DeleteCollectionAsync(name));
        deleteTask.Wait(); 
    }

    public async Task DeleteCollectionAsync(string name, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"api/v1/collections/{name}?tenant={_tenant}&database={_database}", cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new Exception($"Error deleting collection {name}: {content}");
        }
    }

    public ICollectionClient GetCollection(string name, IEmbeddable? embeddingFunction = null)
    {
        Task<ICollectionClient> getTask = Task.Run(() => GetCollectionAsync(name, embeddingFunction));
        return getTask.Result;
    }

    public async Task<ICollectionClient> GetCollectionAsync(string name, IEmbeddable? embeddingFunction = null, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/v1/collections/{name}?tenant={_tenant}&database={_database}", cancellationToken).ConfigureAwait(false);
        string content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error getting collection {name}: {content}");
        }

        Collection collection = JsonConvert.DeserializeObject<Collection>(content)
            ?? throw new Exception($"Invalid collection response: {content}");
        return new CollectionClient(_httpClient, collection, embeddingFunction);
    }

    public long Heartbeat()
    {
        Task<long> heartbeatTask = Task.Run(() => HeartbeatAsync());
        return heartbeatTask.Result;
    }

    public async Task<long> HeartbeatAsync(CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/api/v1/heartbeat", cancellationToken).ConfigureAwait(false);
        string content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error getting ChromaDB heartbeat: {content}");
        }

        HeartbeatResponse? heartbeatResponse = JsonConvert.DeserializeObject<HeartbeatResponse>(content)
            ?? throw new Exception($"Invalid heartbeat response from ChromaDB {content}");
        return heartbeatResponse.Heartbeat;
    }

    public IEnumerable<Collection> ListCollections()
    {
        Task<IEnumerable<Collection>> collectionTask = Task.Run(() => ListCollectionsAsync());
        return collectionTask.Result;
    }

    public async Task<IEnumerable<Collection>> ListCollectionsAsync(CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/v1/collections?tenant={_tenant}&database={_database}", cancellationToken).ConfigureAwait(false);
        string content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if(!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error getting Collections from ChromaDB: {content}");
        }

        IEnumerable<Collection> collections = JsonConvert.DeserializeObject<IEnumerable<Collection>>(content)
             ?? throw new Exception($"Invalid response from ListCollections: {content}");
        return collections;
    }

    public bool Reset()
    {
        Task<bool> resetTask = Task.Run(() => ResetAsync());
        return resetTask.Result;
    }

    public async Task<bool> ResetAsync(CancellationToken cancellationToken = default)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/v1/reset");
        HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        string content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error resetting ChromaDB: {content}");
        }
        bool responseValue = JsonConvert.DeserializeObject<bool>(content);
        return responseValue;
    }

    public void UpdateCollection(string collectionId, string? name = null, IDictionary<string, object>? metadata = null)
    {
        Task updateTask = Task.Run(() => UpdateCollectionAsync(collectionId, name, metadata));
        updateTask.Wait();
    }

    public async Task UpdateCollectionAsync(string collectionId, string? name = null, IDictionary<string, object>? metadata = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name) && metadata == null) throw new ArgumentException("Name or Metadata must include data.");
        UpdateCollectionRequest request = new UpdateCollectionRequest
        {
            NewName = name,
            NewMetadata = metadata
        };
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/v1/collections/{collectionId}", request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new Exception($"Failed to update collection {collectionId}: {content}");
        }
    }

    public string Version()
    {
        Task<string> versionTask = Task.Run(() => VersionAsync());
        return versionTask.Result;
    }

    public async Task<string> VersionAsync(CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/api/v1/version", cancellationToken).ConfigureAwait(false);
        string content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error getting ChromaDB version. {content}");
        }

        return content;
    }
}
