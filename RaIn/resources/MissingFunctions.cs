namespace Game;

public class MissFunc()
{
    // Dictionary Size
    public static int GetDictionarySize(Godot.Collections.Dictionary dict)
    {
        int size = 0;
        foreach (var (key, value) in dict)
        {
            size++;
        }

        return size;
    }

    // Dictionary Empty
    public static bool IsDictEmpty(Godot.Collections.Dictionary dict)
    {
        if (GetDictionarySize(dict) == 0) return true;
        else return false;
    }

    // Array Size
    public static int GetArraySize(Godot.Collections.Array array)
    {
        int size = 0;
        foreach (var value in array)
        {
            size++;
        }

        return size;
    }

    public static bool IsArrayEmpty(Godot.Collections.Array array)
    {
        if (GetArraySize(array) == 0) return true;
        else return false;
    }
}
