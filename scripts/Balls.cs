using Godot;
using System;

public partial class Balls : RigidBody2D,ITransimitable
{
	[Export]
	public double maxDroptime = 5.0;
	private double _timeSinceSpawn = 0.0;
	private int energyLeft;
	private int maxEnergy;
	public override void _Ready()
	{
		// 设置物理属性
		Mass = 0.1f;
		LinearDamp = 0.1f;
		AngularDamp = 0.1f;
		PhysicsMaterialOverride = new PhysicsMaterial
		{
			Friction = 0.5f,
			Bounce = 0.5f
		};
		// 碰撞层和掩码
		CollisionLayer = 2;
		CollisionMask = 2;
		GetNode<Area2D>("detectionArea").Connect("body_entered", new Callable(this, nameof(On_Drop)));
	}
	public void setEnergy(int energy)
	{
		energyLeft=energy;
		maxEnergy=energy;
	}
	public override void _Process(double delta)
	{
		_timeSinceSpawn += delta;
		if (_timeSinceSpawn >= maxDroptime)
		{
			var plarey=GetNodeOrNull<Player>("../player");
			if (plarey != null)
			{
				plarey.reBackEnergy(maxEnergy);
			}
			QueueFree();
		}
	}
	
	public void ShootBall(Vector2 direction)
	{
		direction = direction.Normalized();
		LinearVelocity = direction * 800f;
		AngularVelocity = 0;
	}
	public void On_Drop(Node body)
	{
		if(body is GroundForAll){
			energyLeft--;
			if (energyLeft <= 0)
			{
				CallDeferred(nameof(createdoor), GlobalPosition);
				QueueFree(); // 能量耗尽，销毁球
			}
		}
	}
	public void createdoor(Vector2 dropPosition)
	{
		Door newdoor=GD.Load<PackedScene>("res://things/door.tscn").Instantiate<Door>();
		newdoor.setenergy(maxEnergy);
		this.GetParent().AddChild(newdoor);
		newdoor.firstDoor.GlobalPosition = GetNode<Player>("../player").GlobalPosition;
		newdoor.secondDoor.GlobalPosition = dropPosition;
	}
	public void enteredDoor(Door door)
	{
		
	}
}