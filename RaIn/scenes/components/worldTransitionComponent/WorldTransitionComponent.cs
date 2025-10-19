using Game.Entity;
using Game.Managers;
using Godot;
using Godot.Collections;

namespace Game.Components;

public partial class WorldTransitionComponent : Node
{
    [Export]
    private GameManagerComponent gameManagerComponent;
    [Export]
    private Player player;

    // Create Array of String with Paths to possible Worlds
    private Array<string> worldPaths = [
        "res://scenes/worlds/greenWorld/GreenWorld.tscn",
        "res://scenes/worlds/purpleWorld/PurpleWorld.tscn",
        "res://scenes/worlds/blackWorld/BlackWorld.tscn",
        "res://scenes/worlds/whiteWorld/WhiteWorld.tscn"
    ];

    public override void _Ready()
    {
        gameManagerComponent.StartWorldChanging += OnWorldChangingSignal;
    }

    public override void _Process(double delta)
    {
    }

    private void OnWorldChangingSignal(int worldNr)
    {
        // Save current world before loading next scene
        PackedScene newWorldScene = GD.Load<PackedScene>(worldPaths[worldNr]);
        GetTree().ChangeSceneToPacked(newWorldScene);
    }
}
