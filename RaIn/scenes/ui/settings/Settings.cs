using Godot;
using Godot.Collections;

namespace Game.UI;

public partial class Settings : Control
{
    private OptionButton windowResolution;
    private CheckButton fullscreenButton;
    private Button returnButton;
    private HSlider generalSound;

    private long currentResolutionIndex;
    private bool isFullscreen;
    private Dictionary<int, float> currentVolume;

    private const string CONFIG_FILE_PATH = "user://settings.cfg";

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
        windowResolution = GetNode<OptionButton>("MarginContainer/VBoxContainer/TabContainer/Video/VBoxContainer/HBoxContainer/ResolutionButton");
        fullscreenButton = GetNode<CheckButton>("MarginContainer/VBoxContainer/TabContainer/Video/VBoxContainer/FullscreenCheckButton");
        returnButton = GetNode<Button>("MarginContainer/VBoxContainer/ReturnButton");
        generalSound = GetNode<HSlider>("MarginContainer/VBoxContainer/TabContainer/Music/VBoxContainer/HBoxContainer/GeneralSoundSlider");
    }

    private void ConnectSignals()
    {
        windowResolution.ItemSelected += OnWindowSizeSelected;
        fullscreenButton.Toggled += OnFullscreenButtonToggled;
        returnButton.Pressed += OnReturnButtonPressed;
        generalSound.ValueChanged += OnGeneralSoundValueChanged;
    }

    public void CheckForLoad()
    {
        if (FileAccess.FileExists(CONFIG_FILE_PATH))
        {
            LoadSettings();
        }
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
                currentResolutionIndex = index;
                DisplayServer.WindowSetSize(new Vector2I(1280, 720));
                break;
            case 1:
                currentResolutionIndex = index;
                DisplayServer.WindowSetSize(new Vector2I(1920, 1080));
                break;
            case 2:
                currentResolutionIndex = index;
                DisplayServer.WindowSetSize(new Vector2I(2560, 1440));
                break;
            case 3:
                currentResolutionIndex = index;
                DisplayServer.WindowSetSize(new Vector2I(3840, 2160));
                break;
            default:
                GD.PrintErr("Chose inexistent option");
                break;
        }
        SaveSettings();
    }

    private void OnFullscreenButtonToggled(bool isToggled)
    {
        if ((bool)isToggled)
        {
            isFullscreen = true;
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
        else
        {
            isFullscreen = false;
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }

        SaveSettings();
    }

    private void OnReturnButtonPressed()
    {
        ChangeSceneToMain();
    }

    private void OnGeneralSoundValueChanged(double value)
    {
        // TODO: Test Audio
        AudioServer.SetBusVolumeLinear(0, (float)value);
        currentVolume[0] = (float)value;

        SaveSettings();
    }

    private void SaveSettings()
    {
        ConfigFile config = new ConfigFile();

        config.SetValue("video", "resolution", currentResolutionIndex);
        config.SetValue("video", "fullscreen", isFullscreen);
        config.SetValue("audio", "volume", currentVolume);

        config.Save(CONFIG_FILE_PATH);
    }

    private void LoadSettings()
    {
        ConfigFile config = new ConfigFile();

        Error err = config.Load(CONFIG_FILE_PATH);

        // TODO: Check why config file is loading into default
        if (err == Error.Ok)
        {
            currentResolutionIndex = (long)config.GetValue("video", "resolution", 1);
            isFullscreen = (bool)config.GetValue("video", "fullscreen", false);
            currentVolume = (Dictionary<int, float>)config.GetValue("audio", "volume", new Dictionary<int, float>());
        }

        LoadSettingsIntoVars();

    }

    private void LoadSettingsIntoVars()
    {
        OnWindowSizeSelected(currentResolutionIndex);
        OnFullscreenButtonToggled(isFullscreen);

        if (MissFunc.GetDictionarySize((Dictionary)currentVolume) != 0)
        {
            foreach (var (busID, value) in currentVolume)
            {
                AudioServer.SetBusVolumeLinear(busID, value);
            }
        }
    }
}


