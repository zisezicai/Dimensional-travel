using Godot;
using System;

public partial class End : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("body_entered",new Callable(this, nameof(On_BodyEntered)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void On_BodyEntered(Node body)
	{
		if (body is Player)
		{
			CallDeferred("ChangeScene","res://outcome/win.tscn");
		}
	}
	private void ChangeScene(String scene)
	{
		GetTree().ChangeSceneToFile(scene);
	}
}
