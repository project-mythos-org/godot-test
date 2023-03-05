using Godot;
using System;

public partial class Main : Node3D
{
    [Export(PropertyHint.File)]
    public string PlayerScenePath;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (PlayerScenePath is null) PlayerScenePath = "res://PlayerFirst/player_first.tscn";
        AddChild(GD.Load<PackedScene>(PlayerScenePath).Instantiate());
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }

        if (@event.IsActionPressed("click") &&
            Input.MouseMode == Input.MouseModeEnum.Visible)
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
            GetViewport().SetInputAsHandled();
        }
    }
}
