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


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameManagerComponent.StartWorldChanging += OnWorldChangingSignal;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnWorldChangingSignal(int worldNr)
	{
		GD.Print("Signal Received");
	}
}
