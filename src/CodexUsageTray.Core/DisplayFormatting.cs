namespace CodexUsageTray.Core;

public static class DisplayFormatting
{
    public static string WindowName(TimeSpan duration) => duration.TotalHours switch
    {
        < 1 => $"{Math.Max(1, Math.Round(duration.TotalMinutes)):0} min",
        < 24 => $"{duration.TotalHours:0.#} h",
        _ => $"{duration.TotalDays:0.#} días"
    };

    public static string ResetText(DateTimeOffset reset, DateTimeOffset now)
    {
        if (reset == DateTimeOffset.MinValue) return "reinicio desconocido";
        var remaining = reset - now;
        if (remaining <= TimeSpan.Zero) return "reiniciando…";
        if (remaining.TotalDays >= 1) return $"reinicia en {Math.Floor(remaining.TotalDays)} d {remaining.Hours} h";
        if (remaining.TotalHours >= 1) return $"reinicia en {Math.Floor(remaining.TotalHours)} h {remaining.Minutes} min";
        return $"reinicia en {Math.Max(1, remaining.Minutes)} min";
    }
}
