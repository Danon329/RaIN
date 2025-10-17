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
    [Export]
    private Items.Lock lockItem;

    public enum Worlds
    {
        Green,
        Purple,
        Black,
        White
    }

    private Timer timer;

    private bool keyCollected = false;
    private bool lockOpened = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
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
        lockItem.LockOpened += OnLockOpened;
    }

    private int GetRandomWorld()
    {
        int newWorld;

        do
        {
            newWorld = GD.RandRange(0, 3);
        } while ((int)World == newWorld);

        return newWorld;
    }

    private bool CheckForGameFinished()
    {
        SaveManagerComponent saveManager = new SaveManagerComponent();
        Godot.Collections.Dictionary<int, bool> keys = saveManager.LoadKeys();

        if (MissFunc.DictionarySize((Godot.Collections.Dictionary)keys) == 4)
        {
            foreach (var (key, wasUsed) in keys)
            {
                if (wasUsed == false)
                {
                    return false;
                }
            }

            return true;
        }

        return false;
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
        save.SaveKey(keyTypeId, false);
        // General Save
    }

    private void OnLockOpened()
    {
        lockOpened = true;
        GD.Print("Lock opened");

        // General Save
        // Check for all locks opened
        if (CheckForGameFinished())
        {
            GD.Print("Game Finished");
        }
    }
}
