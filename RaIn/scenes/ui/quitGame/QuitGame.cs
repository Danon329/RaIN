using Godot;

public partial class QuitGame : Control
{
    private Button yesButton;
    private Button noButton;

    public override void _Ready()
    {
        GetNodes();
        ConnectSignals();
    }

    private void GetNodes()
    {
        yesButton = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/YesMC/YesButton");
        noButton = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/NoMC/NoButton");
    }

    private void ConnectSignals()
    {
        if (yesButton != null) yesButton.Pressed += OnYesButtonPressed;
        if (noButton != null) noButton.Pressed += OnNoButtonPressed;
    }

    private void OnYesButtonPressed()
    {
        GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
        GetTree().Quit(0);
    }

    private void OnNoButtonPressed()
    {
        PackedScene mainScreenScene = GD.Load<PackedScene>(Paths.GetMainScreenPath());
        GetTree().ChangeSceneToPacked(mainScreenScene);
    }
}
