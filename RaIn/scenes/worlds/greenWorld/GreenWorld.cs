using Godot;

namespace Game.Worlds;

[GlobalClass]
public partial class GreenWorld : World
{
	public override Godot.Collections.Dictionary<int, Variant> Save()
	{
		GD.Print("Save on Green World Level");
		return base.Save();
	}

	public override void Load(Godot.Collections.Dictionary<int, Variant> dict)
	{
		GD.Print("Load on Green World Level");
		base.Load(dict);
	}
}
