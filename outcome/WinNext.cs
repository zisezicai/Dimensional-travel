using Godot;
using System;

public partial class WinNext : TextureButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("pressed",new Callable(this, nameof(On_Pressed)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void On_Pressed()
	{
		CallDeferred("ChangeScene");
	}
	private void ChangeScene()
	{
		GetTree().ChangeSceneToFile("res://SelectLevel/LevelSelect.tscn");
	}
}
