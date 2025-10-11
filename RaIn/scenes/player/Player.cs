using System;
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
    private Sprite2D sprite;

    private float direction = 0;
    private int breakFactor = 10;

    private bool appliedCoyoteTime = true;
    private bool jumpWasPressed = false;

    public override void _Ready()
    {
        coyoteTimer = GetNode<Timer>("CoyoteTimer");
        sprite = GetNode<Sprite2D>("Sprite2D");

        coyoteTimer.Timeout += OnCoyoteTimerTimeout;
    }

    public override void _PhysicsProcess(double delta)
    {
        GetDirection();

        ApplyGravity(delta);
        ApplyBreak(delta);
        ResetCoyoteTime();

        MovementLogic();
        MoveAndSlide();
        GD.Print(IsOnWall());
        FlipSprite();
    }

    private void ApplyGravity(double delta)
    {
        if (!IsOnFloor() && !IsOnWall())
        {
            if (Velocity.Y < 0 && !Input.IsActionPressed("jump"))
            {
                Velocity = Vector2.Zero;
            }
            Velocity += new Vector2(0, gravity) * (float)delta;
        }
        else if (IsOnWall() && !IsOnFloor())
        {
            Velocity += new Vector2(0, gravity / 4) * (float)delta;
            Mathf.Clamp(Velocity.Y, float.MinValue, gravity / 5);
        }

        CheckForCoyoteTime();
    }

    private void ApplyBreak(double delta)
    {
        if (Math.Round(Velocity.X) != 0)
        {
            Velocity += new Vector2(gravity * breakFactor * -direction, 0) * (float)delta;
        }
        else
        {
            Velocity = new Vector2(0, Velocity.Y);
            breakFactor = 10;
        }
    }

    private void GetDirection()
    {
        if (Velocity.X > 0)
        {
            direction = 1;
        }
        else if (Velocity.X < 0)
        {
            direction = -1;
        }
        else
        {
            direction = 0;
        }
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
        Walk();
        Jump();
        WallJump();
        // Dash
    }

    private void Walk()
    {
        if (Input.IsActionPressed("left") || Input.IsActionPressed("right"))
        {
            direction = Input.GetAxis("left", "right");
            Velocity = new Vector2(direction * speed, Velocity.Y);
            FlipSprite();
        }
    }

    private void Jump()
    {
        if (Input.IsActionJustPressed("jump") && IsOnFloor() || Input.IsActionJustPressed("jump") && !appliedCoyoteTime && !jumpWasPressed)
        {
            Velocity = new Vector2(0, jump);
            jumpWasPressed = true;
        }
    }

    private void WallJump()
    {
        if (IsOnWall() && !IsOnFloor())
        {
            if (IsOnWall() && GetLastMotion().X != 0)
            {
                Velocity = Vector2.Zero;
                jumpWasPressed = false;
            }

            if (Input.IsActionJustPressed("jump") && !jumpWasPressed)
            {
                Vector2 wallJumpVelocity = new Vector2(800f, -800f);
                wallJumpVelocity = new Vector2(wallJumpVelocity.X * GetWallNormal().X, wallJumpVelocity.Y);
                Velocity = wallJumpVelocity;

                jumpWasPressed = true;
                breakFactor = 1;
            }
            FlipSprite();
        }
    }

    private void FlipSprite()
    {
        if (Math.Round(Velocity.X) < 0)
        {
            sprite.FlipH = true;
        }
        else if (Math.Round(Velocity.X) > 0)
        {
            sprite.FlipH = false;
        }
        else
        {
            if (IsOnWall())
            {
                if (GetWallNormal().X < 0)
                {
                    sprite.FlipH = true;
                }
                else if (GetWallNormal().X > 0)
                {
                    sprite.FlipH = false;
                }
            }
        }
    }

    private void OnCoyoteTimerTimeout()
    {
        appliedCoyoteTime = true;
    }
}
