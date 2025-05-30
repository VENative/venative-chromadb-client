﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace VENative.ChromaDB.Client.Models;

public class UpdateCollectionRequest
{
    [JsonProperty("new_name")]
    public string? NewName { get; set; } = null;
    [JsonProperty("new_metadata")]
    public IDictionary<string, object>? NewMetadata = null;
}
