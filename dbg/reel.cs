using Godot;

public partial class reel : Node2D
{
	[Export] Sprite2D PrevFrame;
	[Export] Sprite2D NextFrame;

	public Texture2D src;
	public Texture2D PrevBuffer;
	public Texture2D NextBuffer;
	public Texture2D[] ReelBuffer;

	public void DisplayReel()
	{
        PrevFrame.Texture = PrevBuffer;
        NextFrame.Texture = NextBuffer;
	}

	public void PushScreen2Buffer()
	{
        // set previous buffer from src before assigning
        src = ImageTexture.CreateFromImage(Utl.VP.GetTexture().GetImage());
		// set next buffer afterward.

		// probably don't even need the buffer tex2ds, tbh


        DisplayReel();
    }

	public override void _Ready()
	{
		ReelBuffer = new Texture2D[2] { PrevBuffer, NextBuffer };
        RenderingServer.FramePostDraw += PushScreen2Buffer;
    }
}
