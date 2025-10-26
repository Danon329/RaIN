using Godot.Collections;

public class Paths()
{
    private static Array<string> worldPaths = [
        "res://scenes/worlds/greenWorld/GreenWorld.tscn",
        "res://scenes/worlds/purpleWorld/PurpleWorld.tscn",
        "res://scenes/worlds/blackWorld/BlackWorld.tscn",
        "res://scenes/worlds/whiteWorld/WhiteWorld.tscn"
    ];
    public static string GetWorldPath(int worldID) { return worldPaths[worldID]; }

    private static string settingsPath = "res://scenes/ui/settings/Settings.tscn";
    public static string GetSettingsPath() { return settingsPath; }

    private static string quitGamePath = "res://scenes/ui/quitGame/QuitGame.tscn";
    public static string GetQuitGamePath() { return quitGamePath; }

    private static string mainScreenPath = "res://scenes/ui/main/Main.tscn";
    public static string GetMainScreenPath() { return mainScreenPath; }
}
