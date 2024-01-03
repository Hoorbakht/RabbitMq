using System.Text.Json.Serialization;

namespace Hoorbakht.RabbitMq.Models;

public record BackingQueueStatus(
    [property: JsonPropertyName("avg_ack_egress_rate")] double? AvgAckEgressRate,
    [property: JsonPropertyName("avg_ack_ingress_rate")] double? AvgAckIngressRate,
    [property: JsonPropertyName("avg_egress_rate")] double? AvgEgressRate,
    [property: JsonPropertyName("avg_ingress_rate")] double? AvgIngressRate,
    [property: JsonPropertyName("delta")] IReadOnlyList<object> Delta,
    [property: JsonPropertyName("len")] int? Len,
    [property: JsonPropertyName("mode")] string Mode,
    [property: JsonPropertyName("next_deliver_seq_id")] int? NextDeliverSeqId,
    [property: JsonPropertyName("next_seq_id")] int? NextSeqId,
    [property: JsonPropertyName("num_pending_acks")] int? NumPendingAcks,
    [property: JsonPropertyName("num_unconfirmed")] int? NumUnconfirmed,
    [property: JsonPropertyName("q1")] int? Q1,
    [property: JsonPropertyName("q2")] int? Q2,
    [property: JsonPropertyName("q3")] int? Q3,
    [property: JsonPropertyName("q4")] int? Q4,
    [property: JsonPropertyName("target_ram_count")] string TargetRamCount,
    [property: JsonPropertyName("version")] int? Version
);