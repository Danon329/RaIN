using Godot;
using Godot.Collections;

namespace Game.UI;

[GlobalClass]
public partial class Settings : Control
{
    internal static Settings s_instance;
    public static Settings Instance => s_instance;

    private OptionButton windowResolution;
    private CheckButton fullscreenButton;
    private Button returnButton;
    private HSlider generalSound;

    private long currentResolutionIndex = 1;
    private bool isFullscreen = (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen) ? true : false;
    private Dictionary<int, float> currentVolume = new Dictionary<int, float>
    {
        {0, AudioServer.GetBusVolumeLinear(0)}
    };

    private const string CONFIG_FILE_PATH = "user://settings.cfg";

    public Settings()
    {
        if (s_instance != null)
        {
            return;
        }
        else
        {
            s_instance = this;
        }
    }

    public override void _Ready()
    {
        GetNodes();
        ConnectSignals();

        CheckForLoad();
        ApplyVisuals();
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

    private void ApplyVisuals()
    {
        fullscreenButton.ButtonPressed = isFullscreen;

        if (!fullscreenButton.ButtonPressed)
            windowResolution.Selected = (int)currentResolutionIndex;

        generalSound.Value = currentVolume[0];
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

    private void SetSelectedRes(long id)
    {
        currentResolutionIndex = id;
        if (windowResolution != null) windowResolution.Selected = (int)id;
    }

    private void SetWindowSize(long index)
    {
        switch (index)
        {
            case 0:
                SetSelectedRes(index);
                DisplayServer.WindowSetSize(new Vector2I(1280, 720));
                break;
            case 1:
                SetSelectedRes(index);
                DisplayServer.WindowSetSize(new Vector2I(1920, 1080));
                break;
            case 2:
                SetSelectedRes(index);
                DisplayServer.WindowSetSize(new Vector2I(2560, 1440));
                break;
            case 3:
                SetSelectedRes(index);
                DisplayServer.WindowSetSize(new Vector2I(3840, 2160));
                break;
            default:
                GD.PrintErr("Chose inexistent option");
                break;
        }
        SaveSettings();
    }

    private void SetIsFullscreen(bool value)
    {
        isFullscreen = value;
        if (fullscreenButton != null) fullscreenButton.ButtonPressed = value;
    }

    private void SetFullscreen(bool value)
    {
        if (value)
        {
            SetIsFullscreen(value);
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
        else
        {
            SetIsFullscreen(value);
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }

        SaveSettings();
    }

    private void SetVolume(int busIdx, float value)
    {
        // TODO: Test Audio
        AudioServer.SetBusVolumeLinear(busIdx, value);
        currentVolume[busIdx] = value;

        SaveSettings();
    }

    private void OnWindowSizeSelected(long index) // FUCK ALL OF THESE TYPES. EVERYWHERE IT SAYS INT BUT ACTUALLY ITS LONG: BECAUSE GDSCRIPT INT == 64bit and C# INT == 32bit; long == 64 bit
    {
        SetWindowSize(index);
        ApplyVisuals();
    }

    private void OnFullscreenButtonToggled(bool isToggled)
    {
        SetFullscreen(isToggled);
        ApplyVisuals();
    }

    private void OnReturnButtonPressed()
    {
        ChangeSceneToMain();
        ApplyVisuals();
    }

    private void OnGeneralSoundValueChanged(double value)
    {
        SetVolume(0, (float)value);
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

        if (err == Error.Ok)
        {
            currentResolutionIndex = (long)config.GetValue("video", "resolution");
            isFullscreen = (bool)config.GetValue("video", "fullscreen");
            currentVolume = (Dictionary<int, float>)config.GetValue("audio", "volume");
        }

        LoadSettingsIntoVars();

    }

    private void LoadSettingsIntoVars()
    {
        if (!isFullscreen) SetWindowSize(currentResolutionIndex);
        SetFullscreen(isFullscreen);

        foreach (var (busIdx, value) in currentVolume)
        {
            SetVolume(busIdx, value);
        }
    }
}


