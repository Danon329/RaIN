using Godot;
using Game.Worlds;

namespace Game.Managers;

[GlobalClass]
public partial class SaveManagerComponent : Node
{
    private string keySavePath = "user://KeyData.dat";

    private string[] worldPaths = {
        "user://GreenWorld.dat",
        "user://PurpleWorld.dat",
        "user://BlackWorld.dat",
        "user://WhiteWorld.dat"
    };

    public SaveManagerComponent()
    {
    }

    // Saving and Loading of Keys
    public void SaveKey(int newKey, bool newWasUsed)
    {
        Godot.Collections.Dictionary<int, bool> keyIds = LoadKeys();

        foreach (var (key, wasUsed) in keyIds)
        {
            if (key == newKey && wasUsed != newWasUsed)
            {
                keyIds[key] = newWasUsed;
            }
            else if (key == newKey && wasUsed == newWasUsed)
            {
                GD.PrintErr("This Key already exists, with the same usage");
                return;
            }
        }

        keyIds[newKey] = newWasUsed;

        using FileAccess saveFile = FileAccess.Open(keySavePath, FileAccess.ModeFlags.Write);
        saveFile.StoreVar(keyIds);
    }

    public Godot.Collections.Dictionary<int, bool> LoadKeys()
    {
        Godot.Collections.Dictionary<int, bool> keyIds = new Godot.Collections.Dictionary<int, bool>();

        if (FileAccess.FileExists(keySavePath))
        {
            using FileAccess loadFile = FileAccess.Open(keySavePath, FileAccess.ModeFlags.Read);
            keyIds = (Godot.Collections.Dictionary<int, bool>)loadFile.GetVar();
        }

        return keyIds;
    }

    public void WipeKeySave()
    {
        using FileAccess saveFile = FileAccess.Open(keySavePath, FileAccess.ModeFlags.Write);
        saveFile.StoreVar(new Godot.Collections.Dictionary());
    }
    // Saving and Loading of World data, depending on World

    public void SaveWorld(World world, int worldID)
    {
        Godot.Collections.Dictionary<int, Variant> saveVars = world.Save();
        GD.Print("Loaded Save Dictionary: " + saveVars.ToString());

        using FileAccess saveFile = FileAccess.Open(worldPaths[worldID], FileAccess.ModeFlags.Write);
        saveFile.StoreVar(saveVars);
        GD.Print("Saved File: Success");
    }

    public void LoadWorld(World world, int worldID)
    {
        Godot.Collections.Dictionary<int, Variant> saveVars =
            new Godot.Collections.Dictionary<int, Variant>();
        GD.Print("Loaded new Dictionary");

        if (FileAccess.FileExists(worldPaths[worldID]))
        {
            using FileAccess loadFile = FileAccess.Open(worldPaths[worldID], FileAccess.ModeFlags.Read);
            saveVars = (Godot.Collections.Dictionary<int, Variant>)loadFile.GetVar();
            GD.Print("Loaded Dict into Memory");
        }

        world.Load(saveVars);
        GD.Print("Called Load");
    }
    // Saving current World
}
