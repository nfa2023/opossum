using Godot;

public partial class testTwn : Node
{
	[Export] public Sprite2D currSprite;

	public const int TEST_TWN_MAX = 31;

	public void FinishTwn()
	{
        Twn.Tween t = new Twn.Tween();
        t.start = GD.Randf() * 5f;
        t.end = t.start + (GD.Randf() * 5f);
        t.easeFunction = EaseFunction.SmoothStep;
        t.duration = GD.Randf() * 5f;
        t.during = TwnSpritePos;
        t.onComplete = FinishTwn;

        Twn.Start(ref t);
    }

	public void TwnSpritePos(float t)
	{
		Vector2 newPos = new Vector2(t, currSprite.Position.Y);
		currSprite.Position = newPos;
	}

	public void TestTwns()
	{
		GD.Print("Testing Tweens...");

        Twn.Tween t;

		for(int i = 0; i < TEST_TWN_MAX; ++i)
		{
			t.start = GD.Randf() * TEST_TWN_MAX;
			t.end = t.start + (GD.Randf() * TEST_TWN_MAX);
			t.easeFunction = EaseFunction.SmoothStep;
			t.duration = GD.Randf() * TEST_TWN_MAX;
			t.during = TwnSpritePos;
			t.onComplete = null;
			t.id = 0;

			Twn.Start(ref t);
		}
	}

    public override void _Ready()
    {
        if (currSprite == null) { GD.Print("CURRSPRITE IS NULL!"); return; }

        GD.Randomize();
    }

    public override void _Input(InputEvent e)
    {
		if(e.IsActionPressed("ui_select")) { TestTwns(); }
    }

}
