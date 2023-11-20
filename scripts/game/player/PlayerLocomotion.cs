using Godot;

public partial class PlayerLocomotion : Node
{
	[Export] Node2D PlayerNode;
	[Export] float speed;

	public Vector2 pos;
	public Vector2 vel;

	public override void _Ready()
	{
		pos = PlayerNode.Position;
		vel = new Vector2(0f, 0f);
	}

    public override void _PhysicsProcess(double delta)
    {
        bool noMovementInput = inp.MOVE_HELD == false;
        if (noMovementInput) { return; }

        float scaledSpd = speed * Utl.fdt;

        if (inp.FORWARD_HELD) { vel.Y = -scaledSpd; }
        else if (inp.BACKWARD_HELD) { vel.Y = scaledSpd; }

        if (inp.RIGHT_HELD) { vel.X = scaledSpd; }
        else if (inp.LEFT_HELD) { vel.X = -scaledSpd; }

        pos += vel.Normalized();
        PlayerNode.Position = pos;
        vel = new(0f, 0f);
    }
}
