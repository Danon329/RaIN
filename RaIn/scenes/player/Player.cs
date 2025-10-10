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

    private Timer coyoteTimer;

    private bool appliedCoyoteTime = true;
    private bool jumpWasPressed = false;

    public override void _Ready()
    {
        coyoteTimer = GetNode<Timer>("CoyoteTimer");
        coyoteTimer.Timeout += OnCoyoteTimerTimeout;
    }

    public override void _PhysicsProcess(double delta)
    {
        ApplyGravity(delta);
        ResetCoyoteTime();

        MovementLogic();
        MoveAndSlide();
    }

    private void ApplyGravity(double delta)
    {
        if (!IsOnFloor())
        {
            if (Velocity.Y < 0 && !Input.IsActionPressed("jump"))
            {
                Velocity = Vector2.Zero;
            }
            Velocity += new Vector2(0, gravity) * (float)delta;
        }

        CheckForCoyoteTime();
    }

    private void ResetCoyoteTime()
    {
        if (IsOnFloor())
        {
            appliedCoyoteTime = false;
            jumpWasPressed = false;
        }
    }

    private void CheckForCoyoteTime()
    {
        if (!IsOnFloor() && !appliedCoyoteTime && !jumpWasPressed && coyoteTimer.TimeLeft == 0)
        {
            coyoteTimer.Start(0.2);
        }
    }

    private void MovementLogic()
    {
        // Input left and right
        float dir = Input.GetAxis("left", "right");
        Velocity = new Vector2(dir * speed, Velocity.Y);

        // Jumping
        if (Input.IsActionPressed("jump") && IsOnFloor() || Input.IsActionPressed("jump") && !appliedCoyoteTime && !jumpWasPressed)
        {
            Velocity = new Vector2(0, jump);
            jumpWasPressed = true;
        }

        // Think about Wall jump
    }

    private void OnCoyoteTimerTimeout()
    {
        appliedCoyoteTime = true;
        jumpWasPressed = true;
    }
}
