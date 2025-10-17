using Godot;

namespace Game.Items;

[GlobalClass]
public partial class Key : Sprite2D
{
    [Signal]
    public delegate void KeyCollectedEventHandler(int keyTypeId);

    public enum KeyTypes
    {
        Green,
        Blue,
        Yellow,
        Red
    }

    [Export]
    public KeyTypes KeyType { get; private set; }

    private Area2D area2D;
    private bool areaEntered = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetupTexture();

        area2D = GetNode<Area2D>("Area2D");

        area2D.BodyEntered += OnPlayerEntered;
        area2D.BodyExited += OnPlayerExited;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("interact") && areaEntered)
        {
            // Send Signal (with int Keytype)
            EmitSignal(SignalName.KeyCollected, (int)KeyType);
            // Remove Object
            CallDeferred("queue_free");
        }
    }

    private void SetupTexture()
    {
        switch (KeyType)
        {
            case KeyTypes.Green:
                Frame = 12;
                break;

            case KeyTypes.Blue:
                Frame = 30;
                break;

            case KeyTypes.Yellow:
                Frame = 299;
                break;

            case KeyTypes.Red:
                Frame = 317;
                break;
        }
    }

    private void OnPlayerEntered(Node2D body)
    {
        if (body == GetTree().GetFirstNodeInGroup("player"))
        {
            areaEntered = true;
        }
    }

    private void OnPlayerExited(Node2D body)
    {
        if (body == GetTree().GetFirstNodeInGroup("player"))
        {
            areaEntered = false;
        }
    }
}
