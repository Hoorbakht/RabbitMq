using System.Text.Json.Serialization;

namespace Hoorbakht.RabbitMq.Models;

public record GarbageCollection(
    [property: JsonPropertyName("fullsweep_after")] int? FullsweepAfter,
    [property: JsonPropertyName("max_heap_size")] int? MaxHeapSize,
    [property: JsonPropertyName("min_bin_vheap_size")] int? MinBinVheapSize,
    [property: JsonPropertyName("min_heap_size")] int? MinHeapSize,
    [property: JsonPropertyName("minor_gcs")] int? MinorGcs
);