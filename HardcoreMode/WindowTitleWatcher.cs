using StardewModdingAPI.Events;

namespace HardcoreMode;

public class WindowTitleWatcher
{
    static int _counter;
    
    public static void Begin(object? sender, SaveLoadedEventArgs saveLoadedEventArgs)
    {
        ModEntry.Instance!.Monitor.Log("Starting window watcher");
        ModEntry.Instance!.Helper.Events.GameLoop.UpdateTicking += OnUpdateTick;
    }

    public static void End(object? sender, ReturnedToTitleEventArgs returnedToTitleEventArgs)
    {
        ModEntry.Instance!.Monitor.Log("Stopping window watcher");
        ModEntry.Instance!.Helper.Events.GameLoop.UpdateTicking -= OnUpdateTick;
    }

    private static void OnUpdateTick(object? sender, UpdateTickingEventArgs e)
    {
        _counter++;
        if (_counter >= 10)
        {
            _counter = 0;
            ScanWindows();
        }
    }

    private static void ScanWindows()
    {
        var wins = Native.FindFilteredWindows(hwnd =>
        {
            var text = Native.GetWindowTitle(hwnd);
            return text.Contains("Stardew Valley Wiki");
        });

        var anyPresent = wins.Count > 0;
        if (anyPresent)
        {
            // don't let the user see it, actually
            wins.ForEach(Native.CloseWindow);
            
            ModEntry.Instance!.OnPlayerIsCheating();
        }
    }
}