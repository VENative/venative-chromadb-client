﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace VENative.ChromaDB.Client.Models;

internal class CollectionRequest
{
    [JsonProperty("ids")]
    public IEnumerable<string>? Ids { get; set; } = null;
    [JsonProperty("embeddings")]
    public IEnumerable<IEnumerable<float>>? Embeddings { get; set; } = null;
    [JsonProperty("metadatas")]
    public IEnumerable<IDictionary<string, object>>? Metadatas { get; set; } = null;
    [JsonProperty("documents")]
    public IEnumerable<string>? Documents { get; set; } = null;
    public CollectionRequest() { }
    public CollectionRequest(IEnumerable<string>? ids, IEnumerable<IEnumerable<float>>? embeddings, IEnumerable<IDictionary<string, object>>? metadatas, IEnumerable<string>? documents)
    {
        Ids = ids;
        Embeddings = embeddings;
        Metadatas = metadatas;
        Documents = documents;
    }
}
