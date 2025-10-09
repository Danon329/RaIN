using Godot;

namespace Game.Entity;

public partial class Player : CharacterBody2D
{
    [Export]
    private float speed = 800f;
    [Export]
    private float jump = -800f;
    [Export]
    private float gravity = 1200f;

    public override void _PhysicsProcess(double delta)
    {
        if (!IsOnFloor())
        {
            if (Velocity.Y < 0 && !Input.IsActionPressed("jump"))
            {
                Velocity = Vector2.Zero;
            }
            Velocity += new Vector2(0, gravity) * (float)delta;
        }

        MovementLogic(delta);
        MoveAndSlide();
    }

    private void MovementLogic(double delta)
    {
        // Input left and right
        float dir = Input.GetAxis("left", "right");
        Velocity = new Vector2(dir * speed, Velocity.Y);

        // TODO: coyote time
        // Jumping
        if (Input.IsActionPressed("jump") && IsOnFloor())
        {
            Velocity = new Vector2(0, jump);
        }

        // Think about Wall jump
    }
}
