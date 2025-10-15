using Godot;

namespace Game.Managers;

[GlobalClass]
public partial class GameManagerComponent : Node
{
	[Signal]
	public delegate void StartWorldChangingEventHandler(int worldNr);

	
	[ExportCategory("Setup")]
	[Export]
	public Worlds World { get; private set; }
	[Export]
	private Items.Key key;


	public enum Worlds
	{
		Green,
		Purple,
		Black,
		White
	}

	private Timer timer;

	private int currentWorld = -1;
	private bool keyCollected = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentWorld = (int)World;
		GetNodes();
		// Load SaveData for this World, if existing

		ConnectSignals();

		timer.Start(60);

	}

	private void GetNodes()
	{
		timer = GetNode<Timer>("TransitionTimer");
	}

	private void ConnectSignals()
	{
		timer.Timeout += OnTimerTimeout;
		key.KeyCollected += OnKeyCollected;
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

	private void OnKeyCollected(int keyTypeId)
	{
		keyCollected = true;
		GD.Print("Key Collected");

		SaveManagerComponent save = new SaveManagerComponent();
		save.SaveKey(keyTypeId);
	}
}
