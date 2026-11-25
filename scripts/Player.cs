using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public Vector2 startPosition = new Vector2(285, 45);
	public delegate void PlayerMoveToRightHandler();
	public delegate void PlayerMoveToLeftHandler();
	public delegate void PlayerJumpHandler();
	public delegate void PlayerIdleHandler();
	public event PlayerMoveToRightHandler OnPlayerMoveToRight = new PlayerMoveToRightHandler(() => { });
	public event PlayerMoveToLeftHandler OnPlayerMoveToLeft = new PlayerMoveToLeftHandler(() => { });
	public event PlayerIdleHandler OnPlayerIdle=new PlayerIdleHandler(() => { });
	public event PlayerJumpHandler OnPlayerJump = new PlayerJumpHandler(() => { });
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	private int energy;
	private int willShootEnergy=1;
	public void reBackEnergy(int energy)
	{
		this.energy+=energy;
	}
	public override void _Ready()
	{
		energy = GetNode<Level>("..").Energy;
		base._Ready();
		OnPlayerMoveToRight += () => GetNode<AnimatedSprite2D>("appearance").Play("runToRight");
		OnPlayerMoveToLeft += () => GetNode<AnimatedSprite2D>("appearance").Play("runToLeft");
		OnPlayerJump += () => GetNode<AnimatedSprite2D>("appearance").Play("jump");
		OnPlayerIdle += () => GetNode<AnimatedSprite2D>("appearance").Play("idle");
		CollisionLayer = 1;
		CollisionMask=1;
		ZIndex = 1;
		GlobalPosition = startPosition;
	}
	
	public override void _Process(double delta)
	{
		Label energyLabel = GetNode<Label>("energyLabel");
		energyLabel.Text = $"Energy: {energy}\nWill Shoot Energy: {willShootEnergy}";
		Vector2 velocity = Velocity;
		
		// 添加重力
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// 处理跳跃
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// 处理移动
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
		
		// 动画处理
		if (velocity.Length() <= 0.1f && IsOnFloor())
		{
			OnPlayerIdle.Invoke();
		}
		
		// 触发移动事件
		if (Input.IsActionJustPressed("ui_right"))
		{
			OnPlayerMoveToRight.Invoke();
		}
		if (Input.IsActionJustPressed("ui_left"))
		{
			OnPlayerMoveToLeft.Invoke();
		}
		if (Input.IsActionJustPressed("ui_accept"))
		{
			OnPlayerJump.Invoke();
		}
		
		// 鼠标左键发射球
		if (Input.IsActionJustPressed("shoot"))
		{
			if(energy>=willShootEnergy && willShootEnergy>0)
			{
				ShootBall();
			}
			if (willShootEnergy > energy)
			{
				willShootEnergy = energy;
			}
		}
		if(Input.IsActionJustPressed("energyUp"))
		{
			if(energy>willShootEnergy)
			{
				willShootEnergy++;
			}
		}
		if(Input.IsActionJustPressed("energyDown"))
		{
			if(willShootEnergy>1)
			{
				willShootEnergy--;
			}
		}
		if (GlobalPosition.Y > 2000)
		{
			GlobalPosition = startPosition;
		}
	}
	
	private void ShootBall()
	{
		energy -= willShootEnergy;
		Vector2 mousePosition = GetGlobalMousePosition();
		
		// 创建球
		Balls shootball = GD.Load<PackedScene>("res://things/ball.tscn").Instantiate<RigidBody2D>() as Balls;
		if(shootball == null)
		{
			GD.PrintErr("Failed to instantiate ball scene.");
			return;
		}
		shootball.setEnergy(willShootEnergy);
		
		// 把球加入到与玩家同一父节点
		GetParent().AddChild(shootball);
		
		// // 设置球的位置为玩家位置（稍微偏移以避免立即碰撞）
		shootball.GlobalPosition = GlobalPosition + new Vector2(0, -20); // 稍微向上偏移一点
		
		// // 计算从玩家到鼠标的方向
		Vector2 directionToMouse = (mousePosition - GlobalPosition).Normalized();
		
		// // 发射球
		shootball.ShootBall(directionToMouse);
		
		// 添加后坐力效果
		// Velocity -= directionToMouse * 200f;
	}
}