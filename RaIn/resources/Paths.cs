using Godot.Collections;

public class Paths()
{
    private static Array<string> worldPaths = [
        "res://scenes/worlds/greenWorld/GreenWorld.tscn",
        "res://scenes/worlds/purpleWorld/PurpleWorld.tscn",
        "res://scenes/worlds/blackWorld/BlackWorld.tscn",
        "res://scenes/worlds/whiteWorld/WhiteWorld.tscn"
    ];

    public static string GetWorldPath(int worldID)
    {
        return worldPaths[worldID];
    }
}
