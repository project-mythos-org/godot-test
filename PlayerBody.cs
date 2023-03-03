using Godot;
using System;

public partial class PlayerBody : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public const float MouseSensitivity = 0.002f;

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y -= gravity * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
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

    public override void _Input(InputEvent @event)
    {
        if ((@event is InputEventMouseMotion mouseEvent) &&
		    (Input.MouseMode == Input.MouseModeEnum.Captured))
		{
			RotateY(-mouseEvent.Relative.X * MouseSensitivity);

			Camera3D cam = GetNode<Camera3D>("PlayerCamera");
			cam.RotateX(-mouseEvent.Relative.Y * MouseSensitivity);
			cam.RotationDegrees = new Vector3(
				Mathf.Clamp(cam.RotationDegrees.X, -70, 70),
				cam.RotationDegrees.Y,
				cam.RotationDegrees.Z
			);
		}
    }
}
