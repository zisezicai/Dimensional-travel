using Godot;
using System;

public partial class GroundForAll : StaticBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CollisionLayer=1|2;
		CollisionMask=1|2;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
