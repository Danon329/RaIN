using Godot;

namespace Game.UI;

public partial class Settings : Control
{
    private OptionButton windowSize;
    private Button returnButton;

    public override void _Ready()
    {
        GetNodes();
        ConnectSignals();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("esc"))
        {
            ChangeSceneToMain();
        }
    }

    private void GetNodes()
    {
        windowSize = GetNode<OptionButton>("MarginContainer/VBoxContainer/TabContainer/Video/HBoxContainer/WindowSizeButton");
        returnButton = GetNode<Button>("MarginContainer/VBoxContainer/ReturnButton");
    }

    private void ConnectSignals()
    {
        windowSize.ItemSelected += OnWindowSizeSelected;
        returnButton.Pressed += OnReturnButtonPressed;
    }

    private void ChangeSceneToMain()
    {
        PackedScene mainScene = GD.Load<PackedScene>("res://scenes/ui/main/Main.tscn");
        GetTree().ChangeSceneToPacked(mainScene);
    }

    private void OnWindowSizeSelected(long index) // FUCK ALL OF THESE TYPES. EVERYWHERE IT SAYS INT BUT ACTUALLY ITS LONG: BECAUSE GDSCRIPT INT == 64bit and C# INT == 32bit; long == 64 bit
    {
        switch (index)
        {
            case 0:
                GD.Print("Should be windowed");
                GetWindow().Mode = Window.ModeEnum.Windowed;
                break;
            case 1:
                GD.Print("Should be fullscreen");
                GetWindow().Mode = Window.ModeEnum.Fullscreen;
                break;
            default:
                GD.PrintErr("Chose inexistent option");
                break;
        }
    }

    private void OnReturnButtonPressed()
    {
        ChangeSceneToMain();
    }
}


