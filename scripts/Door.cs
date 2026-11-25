using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public partial class Door : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public double takebackdistance;
	private bool playerInFirstDoor;
	private bool playerInSecondDoor;
	public Area2D firstDoor;
	public Area2D secondDoor;
	private int doorenergy;
	// public <Node> door;
	public void setenergy(int energy)
	{
		doorenergy = energy;
	}
	public override void _Ready()
	{
		firstDoor = GetNode<Area2D>("firstdoor");
		secondDoor = GetNode<Area2D>("seconddoor");
		firstDoor.BodyEntered+=onFirstDoorEntered;
		firstDoor.BodyExited+=onFirstDoorExited;
		secondDoor.BodyEntered+=onSecondDoorEntered;
		secondDoor.BodyExited+=onSecondDoorExited;
		playerInFirstDoor = true;
		playerInSecondDoor = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private double min(double x,double y)
	{
		return x<y?x:y;
	}
	private double Getdistance(Vector2 x,Vector2 y)
	{
		return Math.Sqrt((x.X-y.X)*(x.X-y.X)+(x.Y-y.Y)*(x.Y-y.Y));
	}
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("usedoor"))
		{
			if(playerInFirstDoor)
			{
				GetNode<Player>("../player").GlobalPosition = secondDoor.GlobalPosition;
				playerInFirstDoor = false;
				playerInSecondDoor = true;
			}
			else if(playerInSecondDoor)
			{
				GetNode<Player>("../player").GlobalPosition = firstDoor.GlobalPosition;
				playerInFirstDoor = true;
				playerInSecondDoor = false;
			}
		}
		var playerposition=GetNode<Player>("../player").GlobalPosition;
		if(min(Getdistance(firstDoor.GlobalPosition,playerposition)
		, Getdistance(secondDoor.GlobalPosition, playerposition)) > takebackdistance)
		{
			GD.Print(doorenergy);
			GetNode<Player>("../player").reBackEnergy(doorenergy);
			QueueFree();
		}
	}
	private void onFirstDoorEntered(Node body)
	{
		playerInFirstDoor = true;
		GD.Print("firstdoorEnterd");
	}
	private void onFirstDoorExited(Node body)
	{
		playerInFirstDoor = false;
		GD.Print("firstdoorExited");
	}
	private void onSecondDoorEntered(Node body)
	{
		playerInSecondDoor=true;
		GD.Print("seconddoorEntered");
	}
	private void onSecondDoorExited(Node body)
	{
		playerInSecondDoor = false;
		GD.Print("seconddoorExited");
	}

}
