namespace CodexUsageTray;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        using var mutex = new Mutex(true, "CodexUsageTray.SingleInstance", out var isFirst);
        if (!isFirst) return;
        Application.Run(new TrayApplicationContext());
    }
}
