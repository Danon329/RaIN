using Godot;

public partial class Settings : Control
{
    private OptionButton windowSize;

    public override void _Ready()
    {
        GetNodes();
        ConnectSignals();
    }

    private void GetNodes()
    {
        windowSize = GetNode<OptionButton>("MarginContainer/VBoxContainer/TabContainer/Video/HBoxContainer/WindowSizeButton");
    }

    private void ConnectSignals()
    {
        windowSize.ItemSelected += OnWindowSizeSelected;
    }

    private void OnWindowSizeSelected(long index)
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
}


