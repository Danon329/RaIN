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
            saves["KeyCollected"] = gameManager.IsKeyCollected();
            saves["LockOpened"] = gameManager.IsLockOpened();
            saves["playerPos"] = player.GlobalPosition;
        }

        return saves;

    }

    public virtual void Load(Godot.Collections.Dictionary<string, Variant> dict)
    {
        if (!MissFunc.IsDictEmpty((Godot.Collections.Dictionary)dict))
        {
            if (player != null && gameManager != null)
            {
                gameManager.SetKeyCollected((bool)dict["KeyCollected"]);
                gameManager.SetLockOpened((bool)dict["LockOpened"]);
                player.GlobalPosition = (Vector2)dict["playerPos"];
            }
        }
    }
}
