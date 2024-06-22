using StardewValley;

namespace HardcoreMode;

public class SaveManager
{
    // See StardewValley.Menus.LoadGameMenu#deleteFile(int)
    public static void EraseFile(string saveName)
    {
        ModEntry.Instance!.Monitor.Log($"Erasing save file '{saveName}'!");
        
        var path = Path.Combine(Program.GetSavesFolder(), saveName);
        if (Directory.Exists(path))
            Directory.Delete(path, true);
        
        // hey concernedape what the fuck
        var c = 50;
        while (c > 0 && Directory.Exists(path))
        {
            c--;
            Thread.Sleep(100);
        }
    }
}