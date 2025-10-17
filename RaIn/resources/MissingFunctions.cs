namespace Game;

public class MissFunc()
{
    // Dictionary Size
    public static int DictionarySize(Godot.Collections.Dictionary dict)
    {
        int size = 0;
        foreach (var (key, value) in dict)
        {
            size++;
        }

        return size;
    }
}
