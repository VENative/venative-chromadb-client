using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VENative.ChromaDB.Client.V2;

public class CreateTenantPayload
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class CreateDatabasePayload
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class CreateCollectionPayload
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("configuration")]
    public CollectionConfiguration? Configuration { get; set; }

    [JsonPropertyName("get_or_create")]
    public bool GetOrCreate { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

public class UpdateCollectionPayload
{
    [JsonPropertyName("new_configuration")]
    public CollectionConfiguration? NewConfiguration { get; set; }

    [JsonPropertyName("new_metadata")]
    public Dictionary<string, object>? NewMetadata { get; set; }

    [JsonPropertyName("new_name")]
    public string? NewName { get; set; }
}

public class ForkCollectionPayload
{
    [JsonPropertyName("new_name")]
    public string NewName { get; set; } = string.Empty;
}

public class AddCollectionRecordsPayload
{
    [JsonPropertyName("ids")]
    public IEnumerable<string> Ids { get; set; } = [];

    [JsonPropertyName("documents")]
    public IEnumerable<string?>? Documents { get; set; }

    [JsonPropertyName("embeddings")]
    public IEnumerable<IEnumerable<float>>? Embeddings { get; set; }

    [JsonPropertyName("metadatas")]
    public IEnumerable<IDictionary<string, object>?>? Metadatas { get; set; }

    [JsonPropertyName("uris")]
    public IEnumerable<string?>? Uris { get; set; }
}

public class DeleteCollectionRecordsPayload : RawWhereFields
{
    [JsonPropertyName("ids")]
    public IEnumerable<string>? Ids { get; set; }
}

public class GetRequestPayload : RawWhereFields
{
    [JsonPropertyName("ids")]
    public IEnumerable<string>? Ids { get; set; }

    [JsonPropertyName("include")]
    public IEnumerable<string>? Include { get; set; }

    [JsonPropertyName("limit")]
    public int? Limit { get; set; }

    [JsonPropertyName("offset")]
    public int? Offset { get; set; }
}

public class QueryRequestPayload : RawWhereFields
{
    [JsonPropertyName("ids")]
    public IEnumerable<string>? Ids { get; set; }

    [JsonPropertyName("include")]
    public IEnumerable<string>? Include { get; set; }

    [JsonPropertyName("n_results")]
    public int? NResults { get; set; }

    [JsonPropertyName("query_embeddings")]
    public IEnumerable<IEnumerable<float>> QueryEmbeddings { get; set; } = [];
}

public class UpdateCollectionRecordsPayload
{
    [JsonPropertyName("ids")]
    public IEnumerable<string> Ids { get; set; } = [];

    [JsonPropertyName("documents")]
    public IEnumerable<string?>? Documents { get; set; }

    [JsonPropertyName("embeddings")]
    public IEnumerable<IEnumerable<float>?>? Embeddings { get; set; }

    [JsonPropertyName("metadatas")]
    public IEnumerable<IDictionary<string, object>?>? Metadatas { get; set; }

    [JsonPropertyName("uris")]
    public IEnumerable<string?>? Uris { get; set; }
}

public class UpsertCollectionRecordsPayload
{
    [JsonPropertyName("ids")]
    public List<string> Ids { get; set; } = [];

    [JsonPropertyName("documents")]
    public List<string?>? Documents { get; set; }

    [JsonPropertyName("embeddings")]
    public List<List<float>>? Embeddings { get; set; }

    [JsonPropertyName("metadatas")]
    public List<Dictionary<string, object>?>? Metadatas { get; set; }

    [JsonPropertyName("uris")]
    public List<string?>? Uris { get; set; }
}

public class RawWhereFields
{
    [JsonPropertyName("where")]
    public IDictionary<string, object>? Where { get; set; }

    [JsonPropertyName("where_document")]
    public IDictionary<string, object>? WhereDocument { get; set; }
}
