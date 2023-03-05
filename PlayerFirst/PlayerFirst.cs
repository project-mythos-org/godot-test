using Godot;
using System.Collections.Generic;

public partial class PlayerFirst : CharacterBody3D
{
    private Node3D Head;
    private Camera3D Camera;

    public const float Speed = 7.0f;
    public const float JumpVelocity = 5.0f;
    public const float CameraAcceleration = 40.0f;
    public const float MouseSensitivity = 0.1f;
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    public int Acceleration;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Head = GetNode<Node3D>("Head");
        Camera = Head.GetChild<Camera3D>(0);
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.MouseMode != Input.MouseModeEnum.Captured) return;

        if (@event is InputEventMouseMotion mouseMotion)
        {
            /* horizontal look */
            RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * MouseSensitivity));
            
            /* vertical look */
            Head.RotateX(Mathf.DegToRad(-mouseMotion.Relative.Y * MouseSensitivity));
            Vector3 clamped = Head.RotationDegrees;
            clamped.X = Mathf.Clamp(clamped.X, -70.0f, 70.0f);
            Head.RotationDegrees = clamped;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Camera.TopLevel = false;
        Camera.GlobalTransform = Head.GlobalTransform;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity.Y -= gravity * (float)delta;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
