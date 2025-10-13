using Godot;

namespace Game.Managers;

[GlobalClass]
public partial class SaveManagerComponent : Node
{
	[Export]
	private GameManagerComponent gameManagerComponent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Saving and Loading of Keys
	// Saving and Loading of World data, depending on World
	// Saving current World
}
