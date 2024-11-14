using Newtonsoft.Json;

namespace VENative.ChromaDB.Client.Models;

internal class HeartbeatResponse
{
    [JsonProperty("nanosecond heartbeat")]
    public long Heartbeat { get; set; }
}
