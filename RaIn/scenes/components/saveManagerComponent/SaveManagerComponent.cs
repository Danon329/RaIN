using Godot;

namespace Game.Managers;

[GlobalClass]
public partial class SaveManagerComponent : Node
{
	private string keySavePath = "user://KeyData.dat";

	public SaveManagerComponent()
	{

	}

	// Saving and Loading of Keys
	public void SaveKey(int newKey)
	{
		Godot.Collections.Array<int> keyIds = LoadKeys();
		GD.Print("Loaded Existing Keys: " + keyIds.ToString());

		foreach (int key in keyIds)
		{
			if (key == newKey)
			{
				GD.PrintErr("Key already exists");
				return;
			}
		}

		keyIds.Add(newKey);
		GD.Print("Added new Key: " + keyIds.ToString());

		using FileAccess saveFile = FileAccess.Open(keySavePath, FileAccess.ModeFlags.Write);
		saveFile.StoreVar(keyIds);
	}

	public Godot.Collections.Array<int> LoadKeys()
	{
		Godot.Collections.Array<int> keyIds = new Godot.Collections.Array<int>();

		if (FileAccess.FileExists(keySavePath))
		{
			using FileAccess loadFile = FileAccess.Open(keySavePath, FileAccess.ModeFlags.Read);
			keyIds = (Godot.Collections.Array<int>)loadFile.GetVar();
		}

		return keyIds;
	}

	public void WipeKeySave()
	{
		using FileAccess saveFile = FileAccess.Open(keySavePath, FileAccess.ModeFlags.Write);
		saveFile.StoreVar(new Godot.Collections.Array());
	}
	// Saving and Loading of World data, depending on World
	// Saving current World
}
