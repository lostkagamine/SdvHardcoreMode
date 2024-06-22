using StardewModdingAPI;
using StardewValley;

namespace HardcoreMode;

public class ModEntry : Mod
{
    public static ModEntry? Instance;
    
    public override void Entry(IModHelper helper)
    {
        Instance = this;

        helper.Events.GameLoop.SaveLoaded += WindowTitleWatcher.Begin;
        helper.Events.GameLoop.ReturnedToTitle += WindowTitleWatcher.End;
        
        helper.Events.Input.ButtonPressed += (sender, args) =>
        {
            if (args.Button == SButton.F12)
            {
                if (!Context.IsWorldReady) return;
                
                // debug behaviour
                OnPlayerIsCheating();
            }
        };
    }
    
    private void EraseFile(string saveName)
    {
        Monitor.Log($"Erasing save file '{saveName}'!");
        
        var path = Path.Combine(Program.GetSavesFolder(), saveName);
        if (Directory.Exists(path))
            Directory.Delete(path, true);
        
        // hey concernedape what the fuck
        // (do note, the below code is in the game)
        var c = 50;
        while (c > 0 && Directory.Exists(path))
        {
            c--;
            Thread.Sleep(100);
        }
    }

    public void OnPlayerIsCheating()
    {
        Monitor.Log("PLAYER IS A DIRTY LITTLE CHEATER - save deletion initiated, quitting to title...");
        var name = Constants.SaveFolderName!;
        Game1.ExitToTitle(() =>
        {
            EraseFile(name);
        });
    }
}