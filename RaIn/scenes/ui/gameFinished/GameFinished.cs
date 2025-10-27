using Godot;

public partial class GameFinished : Control
{
    private Timer timer;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");

        timer.Timeout += OnTimerTimeout;
    }

    private void OnTimerTimeout()
    {
        // TODO: Create credits
        GD.Print("Tween maybe and Change to Credits");
    }
}
