using Godot;

namespace Game.Managers;

[GlobalClass]
public partial class GameManagerComponent : Node
{
	public enum Worlds
	{
		Green,
		Purple,
		Black,
		White
	}


	[Signal]
	public delegate void StartWorldChangingEventHandler(int worldNr);

	[Export]
	private SaveManagerComponent saveManagerComponent;
	[Export]
	public Worlds World { get; private set; }

	private Timer timer;

	private int currentWorld = -1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentWorld = (int)World;
		GetNodes();
		// Load SaveData for this World, if existing

		timer.Start(60);

		timer.Timeout += OnTimerTimeout;
	}

	private void GetNodes()
	{
		timer = GetNode<Timer>("TransitionTimer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private int GetRandomWorld()
	{
		int newWorld;

		do
		{
			newWorld = GD.RandRange(0, 3);
		} while (currentWorld == newWorld);

		currentWorld = newWorld;

		return newWorld;
	}

	private void OnTimerTimeout()
	{
		// Signal to World to start World changing sequence
		int newWorldNr = GetRandomWorld();
		EmitSignal(SignalName.StartWorldChanging, newWorldNr);
		GD.Print("Changing World");
	}
}
