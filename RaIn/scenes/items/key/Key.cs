using Godot;
using Game.Managers;
using Game.Entity;

public partial class Key : Sprite2D
{
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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetupTexture();

        area2D = GetNode<Area2D>("Area2D");

        area2D.AreaEntered += OnPlayerEntered;
        area2D.AreaExited += OnPlayerExited;
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

    private void OnPlayerEntered(Area2D player)
    {
        // Listen for Input
    }

    private void OnPlayerExited(Area2D player)
    {
        // Tween stuff maybe??
    }
}
