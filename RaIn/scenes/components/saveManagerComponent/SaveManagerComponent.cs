using Godot;
using Game.Worlds;

namespace Game.Managers;

[GlobalClass]
public partial class SaveManagerComponent : Node
{
    private string keySavePath = "user://KeyData.dat";
    private string lastWorldSavePath = "user://LastWorld.dat";

    private string[] worldSavePaths = {
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
        if (FileAccess.FileExists(keySavePath))
        {
            using FileAccess saveFile = FileAccess.Open(keySavePath, FileAccess.ModeFlags.Write);
            saveFile.StoreVar(new Godot.Collections.Dictionary());
        }
    }

    // Save World
    public void SaveWorld(World world, int worldID)
    {
        Godot.Collections.Dictionary<int, Variant> saveVars = world.Save();

        using FileAccess saveFile = FileAccess.Open(worldSavePaths[worldID], FileAccess.ModeFlags.Write);
        saveFile.StoreVar(saveVars);
    }

    public void LoadWorld(World world, int worldID)
    {
        Godot.Collections.Dictionary<int, Variant> saveVars =
            new Godot.Collections.Dictionary<int, Variant>();

        if (FileAccess.FileExists(worldSavePaths[worldID]))
        {
            using FileAccess loadFile = FileAccess.Open(worldSavePaths[worldID], FileAccess.ModeFlags.Read);
            saveVars = (Godot.Collections.Dictionary<int, Variant>)loadFile.GetVar();
        }

        world.Load(saveVars);
    }

    public void WipeWorlds()
    {
        Godot.Collections.Dictionary<int, Variant> emptyDict = new Godot.Collections.Dictionary<int, Variant>();

        foreach (string worldSavePath in worldSavePaths)
        {
            if (FileAccess.FileExists(worldSavePath))
            {
                using FileAccess saveFile = FileAccess.Open(worldSavePath, FileAccess.ModeFlags.Write);
                saveFile.StoreVar(emptyDict);
            }
        }
    }

    // Saving current World
    public void SaveLastWorld(int worldID)
    {
        using FileAccess saveFile = FileAccess.Open(lastWorldSavePath, FileAccess.ModeFlags.Write);
        saveFile.StoreVar(worldID);
    }

    public int LoadLastWorld()
    {
        int worldID = -1;

        if (FileAccess.FileExists(lastWorldSavePath))
        {
            using FileAccess loadFile = FileAccess.Open(lastWorldSavePath, FileAccess.ModeFlags.Read);
            worldID = (int)loadFile.GetVar();
        }

        return worldID;
    }
}
