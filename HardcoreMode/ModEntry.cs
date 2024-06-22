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

    public void OnPlayerIsCheating()
    {
        Monitor.Log("PLAYER IS A DIRTY LITTLE CHEATER - save deletion initiated, quitting to title...");
        var name = Constants.SaveFolderName!;
        Game1.ExitToTitle(() =>
        {
            Monitor.Log("Erasing save file...");
            SaveManager.EraseFile(name);
        });
    }
}