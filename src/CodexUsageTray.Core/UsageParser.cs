using System.Text.Json;

namespace CodexUsageTray.Core;

public static class UsageParser
{
    public static UsageSnapshot Parse(JsonElement result, DateTimeOffset fetchedAt)
    {
        var buckets = new List<UsageBucket>();
        if (result.TryGetProperty("rateLimitsByLimitId", out var byId) && byId.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in byId.EnumerateObject())
                buckets.Add(ParseBucket(property.Value, property.Name));
        }
        else if (result.TryGetProperty("rateLimits", out var single) && single.ValueKind == JsonValueKind.Object)
        {
            buckets.Add(ParseBucket(single, "codex"));
        }

        return new UsageSnapshot(buckets, fetchedAt);
    }

    private static UsageBucket ParseBucket(JsonElement value, string fallbackId)
    {
        var id = ReadString(value, "limitId") ?? fallbackId;
        return new UsageBucket(id, ReadString(value, "limitName"), ReadWindow(value, "primary"), ReadWindow(value, "secondary"));
    }

    private static UsageWindow? ReadWindow(JsonElement parent, string property)
    {
        if (!parent.TryGetProperty(property, out var value) || value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
            return null;
        if (!value.TryGetProperty("usedPercent", out var used) || !used.TryGetDouble(out var usedPercent))
            return null;
        var minutes = value.TryGetProperty("windowDurationMins", out var duration) && duration.TryGetDouble(out var mins) ? mins : 0;
        var reset = value.TryGetProperty("resetsAt", out var resets) && resets.TryGetInt64(out var unix)
            ? DateTimeOffset.FromUnixTimeSeconds(unix)
            : DateTimeOffset.MinValue;
        return new UsageWindow(usedPercent, TimeSpan.FromMinutes(minutes), reset);
    }

    private static string? ReadString(JsonElement value, string name) =>
        value.TryGetProperty(name, out var property) && property.ValueKind == JsonValueKind.String ? property.GetString() : null;
}
