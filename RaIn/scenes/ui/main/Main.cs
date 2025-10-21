using Godot;
using Game.Managers;

public partial class Main : Control
{
    private Button newWorldButton;
    private Button continueWorldButton;
    private Button settingsButton;

    private MarginContainer continueMC;

    private SaveManagerComponent saveManager = new SaveManagerComponent();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNodes();
        CreateContinueButton();

        ConnectToSignals();

    }

    private void GetNodes()
    {
        newWorldButton = GetNode<Button>("HBoxContainer/VBoxCenter/NewWorldMC/NewWorldButton");
        settingsButton = GetNode<Button>("HBoxContainer/VBoxCenter/SettingsMC/SettingsButton");

        continueMC = GetNode<MarginContainer>("HBoxContainer/VBoxCenter/ContinueMC");
    }

    private void ConnectToSignals()
    {
        if (newWorldButton != null && settingsButton != null)
        {
            newWorldButton.Pressed += OnNewWorldButtonPressed;
            settingsButton.Pressed += OnSettingsButtonPressed;
        }

        if (continueWorldButton != null)
        {
            continueWorldButton.Pressed += OnContinueButtonPressed;
        }
    }

    private bool CheckForExistingWorld()
    {
        if (saveManager.LoadLastWorld() == -1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void CreateContinueButton()
    {
        if (!CheckForExistingWorld()) return;

        // Creating Button
        continueWorldButton = new Button();
        continueMC.AddChild(continueWorldButton);

        // Button Setup
        continueWorldButton.Text = "Continue";
        continueWorldButton.AddThemeFontSizeOverride("font_size", 64);
        continueMC.AddThemeConstantOverride("margin_top", 16);
        continueMC.AddThemeConstantOverride("margin_bottom", 16);
    }

    private void OnNewWorldButtonPressed()
    {
        // TODO: Create new World (Overwrite old saves)
        saveManager.WipeKeySave();
        saveManager.WipeWorlds();

        PackedScene world = GD.Load<PackedScene>(Paths.GetWorldPath(0));
        GetTree().ChangeSceneToPacked(world);
    }

    private void OnSettingsButtonPressed()
    {
        // TODO: Create Settings Scene
    }

    private void OnContinueButtonPressed()
    {
        PackedScene world = GD.Load<PackedScene>(Paths.GetWorldPath(saveManager.LoadLastWorld()));
        GetTree().ChangeSceneToPacked(world);
    }
}
