using Godot;

namespace Game.Worlds;

public partial class WhiteWorld : World
{
    public override Godot.Collections.Dictionary<int, Variant> Save()
    {
        return base.Save();
    }

    public override void Load(Godot.Collections.Dictionary<int, Variant> dict)
    {
        base.Load(dict);
    }
}
