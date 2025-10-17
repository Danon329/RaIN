using Godot;
using Game.Managers;

namespace Game.Items;

public partial class Lock : Sprite2D
{
    [Signal]
    public delegate void LockOpenedEventHandler();

    public enum LockTypes
    {
        Green,
        Blue,
        Yellow,
        Red
    }

    [Export]
    public LockTypes LockType { get; private set; }

    private Area2D area2D;

    private bool playerInArea = false;
    private bool lockOpened = false;

    public override void _Ready()
    {
        SetTexture();

        area2D = GetNode<Area2D>("Area2D");

        area2D.BodyEntered += OnPlayerEntered;
        area2D.BodyExited += OnPlayerExited;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("interact") && playerInArea && !lockOpened)
        {
            // Load Lock, check for Lock with same ID
            CheckForKey();
        }
    }

    private void SetTexture()
    {
        switch (LockType)
        {
            case LockTypes.Yellow:
                Frame = 65;
                break;

            case LockTypes.Red:
                Frame = 83;
                break;

            case LockTypes.Green:
                Frame = 101;
                break;

            case LockTypes.Blue:
                Frame = 119;
                break;

        }
    }

    private void CheckForKey()
    {
        SaveManagerComponent saveManager = new SaveManagerComponent();
        Godot.Collections.Dictionary<int, bool> keys = saveManager.LoadKeys();

        foreach (var (keyID, wasUsed) in keys)
        {
            if (keyID == (int)LockType)
            {
                // Save KeyID with new WasUsed Value
                saveManager.SaveKey(keyID, true);
                EmitSignal(SignalName.LockOpened);
                lockOpened = true;
            }
        }
    }

    private void OnPlayerEntered(Node2D body)
    {
        if (body == GetTree().GetFirstNodeInGroup("player"))
        {
            playerInArea = true;
        }
    }

    private void OnPlayerExited(Node2D body)
    {
        if (body == GetTree().GetFirstNodeInGroup("player"))
        {
            playerInArea = false;
        }
    }
}
