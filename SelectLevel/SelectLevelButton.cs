using Godot;
using System;

public partial class SelectLevelButton : TextureButton
{
	[Export]
	public int levelIndex = 0;
	public override void _Ready()
	{
		this.Pressed += onClick;
		GetNode<Label>("LevelLabel").Text = $"{levelIndex}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void onClick()
	{
		// if(@event is InputEventMouseButton)
		// {
			GD.Print("detectClick");
			GetTree().ChangeSceneToFile($"res://Levels/Level{levelIndex}.tscn");
		// }
	}
}
