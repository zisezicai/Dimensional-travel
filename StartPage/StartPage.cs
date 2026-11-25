using Godot;
using System;

public partial class StartPage : TextureButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("pressed", new Callable(this, "_on_StartButton_pressed"));
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
	public void _on_StartButton_pressed()
	{
		GetTree().ChangeSceneToFile("res://SelectLevel/LevelSelect.tscn");
	}
}
