using Godot;
using Game.Entity;
using Game.Worlds;

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
    private Player player;
    private World currentWorld;

    private bool keyCollected = false;
    private bool lockOpened = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNodes();

        loadGameFile();
        saveCurrentWorld();

        CheckItems();

        ConnectSignals();

        timer.Start(60);

    }

    // _Ready Methods

    private void GetNodes()
    {
        timer = GetNode<Timer>("TransitionTimer");
        currentWorld = (World)GetParent();
    }

    private void ConnectSignals()
    {
        timer.Timeout += OnTimerTimeout;
        key.KeyCollected += OnKeyCollected;
        lockItem.LockOpened += OnLockOpened;
    }

    private void loadGameFile()
    {
        SaveManagerComponent load = new SaveManagerComponent();
        load.LoadWorld(currentWorld, (int)World);
    }

    private void saveCurrentWorld()
    {
        SaveManagerComponent save = new SaveManagerComponent();
        save.SaveLastWorld((int)World);
    }

    private void CheckItems()
    {
        if (keyCollected)
        {
            key.CallDeferred("queue_free");
        }

        if (lockOpened)
        {
            lockItem.CallDeferred("queue_free");
        }
    }
    // Getters and Setters

    public bool IsKeyCollected()
    {
        return keyCollected;
    }

    public void SetKeyCollected(bool value)
    {
        keyCollected = value;
    }

    public bool IsLockOpened()
    {
        return lockOpened;
    }

    public void SetLockOpened(bool value)
    {
        lockOpened = value;
    }

    // General Methods

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

        if (MissFunc.GetDictionarySize((Godot.Collections.Dictionary)keys) == 4)
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

        SaveManagerComponent save = new SaveManagerComponent();
        save.SaveKey(keyTypeId, false);
        save.SaveWorld(currentWorld, (int)World);
    }

    private void OnLockOpened()
    {
        lockOpened = true;

        // General Save
        SaveManagerComponent save = new SaveManagerComponent();
        save.SaveWorld(currentWorld, (int)World);

        // Check for all locks opened
        if (CheckForGameFinished())
        {
            GD.Print("Game Finished");
        }
    }

}
