using Game.Entity;
using Game.Managers;
using Godot;

namespace Game.Components;

public partial class WorldTransitionComponent : Node
{
    [Export]
    private GameManagerComponent gameManagerComponent;
    [Export]
    private Player player;

    // Create Array of String with Paths to possible Worlds

    public override void _Ready()
    {
        gameManagerComponent.StartWorldChanging += OnWorldChangingSignal;
    }

    public override void _Process(double delta)
    {
    }

    private void OnWorldChangingSignal(int worldID)
    {
        // Save current world before loading next scene
        PackedScene newWorldScene = GD.Load<PackedScene>(Paths.GetWorldPath(worldID));
        GetTree().ChangeSceneToPacked(newWorldScene);
    }
}
