using Godot;
using System.Collections.Generic;

public partial class PlayerThird : CharacterBody3D
{
    [Export]
    public float LerpSpeed = 3.0f;

    [Export]
    public Vector3 Offset = new Vector3(0, 7, 5);
    
    public Node3D Head;
    public Camera3D Camera;
    public const float Speed = 7.0f;
    public const float JumpVelocity = 5.0f;
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Head = GetNode<Node3D>("Head");
        Camera = GetNode<Camera3D>("../Camera3D");
    }

    public override void _Input(InputEvent @event)
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        /* Player movement */
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

        /* Camera Movement */
        Camera.GlobalTransform = Camera.GlobalTransform.InterpolateWith(
            GlobalTransform.TranslatedLocal(Offset),
            LerpSpeed * (float)delta
        );
        Camera.LookAt(GlobalTransform.Origin, Transform.Basis.Y);

        /* Character Rotation */
        velocity.Y = 0;
        Vector3 forward = -GlobalTransform.Basis.Z;
        forward.Y = 0;
        float angle = forward.SignedAngleTo(velocity, Vector3.Up);
        if (angle > (Mathf.Pi / 2)) angle = 0;

        Rotate(Vector3.Up, angle * LerpSpeed * (float)delta);
    }
}
