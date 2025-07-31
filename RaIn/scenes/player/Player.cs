using Godot;

public partial class Player : CharacterBody2D
{
    [Export]
    private float speed = 200f;
    [Export]
    private float jump = -700f;
    [Export]
    private float gravity = 600f;

    public override void _PhysicsProcess(double delta)
    {
        if (!IsOnFloor()) {
            Velocity += new Vector2(0, gravity) * (float) delta;
        }

        MovementLogic(delta);
        GD.Print(Velocity);
        MoveAndSlide();
    }

    private void MovementLogic(double delta) {
        // Input left and right
        float dir = Input.GetAxis("left", "right");
        Velocity = new Vector2(dir * speed, Velocity.Y);

        // Jumping
        if (Input.IsActionPressed("jump") && IsOnFloor()) {
            Velocity = new Vector2(0, jump);
        }
    }
}
