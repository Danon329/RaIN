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

    public virtual Godot.Collections.Dictionary<int, Variant> Save()
    {
        Godot.Collections.Dictionary<int, Variant> saves =
            new Godot.Collections.Dictionary<int, Variant>();

        if (player != null && gameManager != null)
        {
            saves[0] = gameManager.IsKeyCollected();
            saves[1] = gameManager.IsLockOpened();
            saves[2] = player.GlobalPosition;
            GD.Print("Loaded Data into Save Dict: " + saves.ToString());
        }

        return saves;

    }

    public virtual void Load(Godot.Collections.Dictionary<int, Variant> dict)
    {
        GD.Print("In Load function");
        if (!MissFunc.IsDictEmpty((Godot.Collections.Dictionary)dict))
        {
            if (player != null && gameManager != null)
            {
                gameManager.SetKeyCollected((bool)dict[0]);
                gameManager.SetLockOpened((bool)dict[1]);
                player.GlobalPosition = (Vector2)dict[2];
                GD.Print("Applied Load");
            }
        }
    }
}
