// 0 = bool Key Collected
// 1 = bool Lock Opened
// 2 = Vector2 Globalposition of Player

using Godot;
using Game.Entity;
using Game.Managers;

namespace Game.Worlds;

[GlobalClass]
public partial class World : Node
{
    [Export]
    public GameManagerComponent gameManager;
    [Export]
    public Player player;

    public virtual Godot.Collections.Dictionary<string, Variant> Save()
    {
        Godot.Collections.Dictionary<string, Variant> saves =
            new Godot.Collections.Dictionary<string, Variant>();

        if (player != null && gameManager != null)
        {
            saves["is_key_collectd"] = gameManager.IsKeyCollected();
            saves["is_lock_opened"] = gameManager.IsLockOpened();
            saves["global_position_player"] = player.GlobalPosition;
        }

        return saves;

    }

    public virtual void Load(Godot.Collections.Dictionary<string, Variant> dict)
    {
        if (!MissFunc.IsDictEmpty((Godot.Collections.Dictionary)dict))
        {
            gameManager.SetKeyCollected((bool)dict["is_key_collectd"]);
            gameManager.SetLockOpened((bool)dict["is_lock_opened"]);
            player.GlobalPosition = (Vector2)dict["global_position_player"];
        }
    }
}
