using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VENative.ChromaDB.Client.V2;

public class HeartbeatResponse
{
    [JsonPropertyName("nanosecond heartbeat")]
    public long NanosecondHeartbeat { get; set; }
}

public class ChecklistResponse
{
    [JsonPropertyName("max_batch_size")]
    public int MaxBatchSize { get; set; }
}

public class GetUserIdentityResponse
{
    [JsonPropertyName("user_id")]
    public string UserId { get; set; } = string.Empty;

    [JsonPropertyName("tenant")]
    public string Tenant { get; set; } = string.Empty;

    [JsonPropertyName("databases")]
    public List<string> Databases { get; set; } = new();
}

public class CreateTenantResponse { }
public class GetTenantResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class Database
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("tenant")]
    public string Tenant { get; set; } = string.Empty;
}

public class CreateDatabaseResponse { }
public class DeleteDatabaseResponse { }

public class Collection
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("configuration_json")]
    public CollectionConfiguration? Configuration { get; set; }

    [JsonPropertyName("tenant")]
    public string Tenant { get; set; } = string.Empty;

    [JsonPropertyName("database")]
    public string Database { get; set; } = string.Empty;

    [JsonPropertyName("log_position")]
    public long LogPosition { get; set; }

    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("dimension")]
    public int? Dimension { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

public class CollectionConfiguration
{
    [JsonPropertyName("embedding_function")]
    public EmbeddingFunctionConfiguration? EmbeddingFunction { get; set; }

    [JsonPropertyName("hnsw")]
    public HnswConfiguration? Hnsw { get; set; }

    [JsonPropertyName("spann")]
    public SpannConfiguration? Spann { get; set; }
}

public class EmbeddingFunctionConfiguration
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "known";

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("config")]
    public Dictionary<string, object>? Config { get; set; }
}

public class HnswConfiguration
{
    [JsonPropertyName("ef_construction")]
    public int? EfConstruction { get; set; }

    [JsonPropertyName("ef_search")]
    public int? EfSearch { get; set; }

    [JsonPropertyName("max_neighbors")]
    public int? MaxNeighbors { get; set; }

    [JsonPropertyName("resize_factor")]
    public double? ResizeFactor { get; set; }

    [JsonPropertyName("space")]
    public string? Space { get; set; }

    [JsonPropertyName("sync_threshold")]
    public int? SyncThreshold { get; set; }
}

public class SpannConfiguration
{
    [JsonPropertyName("ef_construction")]
    public int? EfConstruction { get; set; }

    [JsonPropertyName("ef_search")]
    public int? EfSearch { get; set; }

    [JsonPropertyName("max_neighbors")]
    public int? MaxNeighbors { get; set; }

    [JsonPropertyName("merge_threshold")]
    public int? MergeThreshold { get; set; }

    [JsonPropertyName("reassign_neighbor_count")]
    public int? ReassignNeighborCount { get; set; }

    [JsonPropertyName("search_nprobe")]
    public int? SearchNprobe { get; set; }

    [JsonPropertyName("space")]
    public string? Space { get; set; }

    [JsonPropertyName("split_threshold")]
    public int? SplitThreshold { get; set; }

    [JsonPropertyName("write_nprobe")]
    public int? WriteNprobe { get; set; }
}

public class UpdateCollectionResponse { }

public class AddCollectionRecordsResponse { }
public class DeleteCollectionRecordsResponse { }
public class UpdateCollectionRecordsResponse { }
public class UpsertCollectionRecordsResponse { }

public class GetResponse
{
    [JsonPropertyName("ids")]
    public List<string> Ids { get; set; } = [];

    [JsonPropertyName("include")]
    public List<string> Include { get; set; } = [];

    [JsonPropertyName("documents")]
    public List<string?> Documents { get; set; } = [];

    [JsonPropertyName("embeddings")]
    public List<List<float>> Embeddings { get; set; } = [];

    [JsonPropertyName("metadatas")]
    public List<Dictionary<string, object>> Metadatas { get; set; } = [];

    [JsonPropertyName("uris")]
    public List<string?> Uris { get; set; } = [];
}

public class QueryResponse
{
    [JsonPropertyName("ids")]
    public List<List<string>> Ids { get; set; } = [];

    [JsonPropertyName("include")]
    public List<string> Include { get; set; } = [];

    [JsonPropertyName("distances")]
    public List<List<float>> Distances { get; set; } = [];

    [JsonPropertyName("documents")]
    public List<List<string?>> Documents { get; set; } = [];

    [JsonPropertyName("embeddings")]
    public List<List<List<float>?>> Embeddings { get; set; } = [];

    [JsonPropertyName("metadatas")]
    public List<List<Dictionary<string, object>?>> Metadatas { get; set; } = [];

    [JsonPropertyName("uris")]
    public List<List<string?>> Uris { get; set; } = [];
}

public class ErrorResponse
{
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}