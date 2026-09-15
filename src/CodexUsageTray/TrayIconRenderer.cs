using System.Drawing.Drawing2D;

namespace CodexUsageTray;

internal static class TrayIconRenderer
{
    public static Icon Render(double remainingPercent, bool error = false)
    {
        using var bitmap = new Bitmap(32, 32);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.Clear(Color.Transparent);
        var color = error ? Color.Gray : remainingPercent switch
        {
            > 50 => Color.FromArgb(42, 181, 120),
            > 20 => Color.FromArgb(245, 166, 35),
            _ => Color.FromArgb(235, 76, 89)
        };
        using var track = new Pen(Color.FromArgb(65, 120, 125, 135), 4);
        using var progress = new Pen(color, 4) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        graphics.DrawEllipse(track, 3, 3, 26, 26);
        graphics.DrawArc(progress, 3, 3, 26, 26, -90, (float)(360 * Math.Clamp(remainingPercent, 0, 100) / 100));
        using var font = new Font("Segoe UI", 9, FontStyle.Bold, GraphicsUnit.Pixel);
        var text = error ? "!" : Math.Round(remainingPercent).ToString("0");
        var size = graphics.MeasureString(text, font);
        using var brush = new SolidBrush(Color.White);
        graphics.DrawString(text, font, brush, (32 - size.Width) / 2, (32 - size.Height) / 2);
        return Icon.FromHandle(bitmap.GetHicon());
    }
}
