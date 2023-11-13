using Godot;

public partial class splash : Node
{
	[Export] ColorRect BlackBckgrnd;
	[Export] ColorRect WhiteForgrnd;
	[Export] TextureRect NoFunAllowedLogo;
	[Export] HBoxContainer SubLogos;

	Color transparency;
	public bool canQuit = true;

	public Twn.Tween splashTwn;

    public void DuringSplashHide(float alpha)
	{
        transparency.A = alpha;

        BlackBckgrnd.Modulate = transparency;
	}

    public void HideSplash()
    {
		canQuit = false;

		Twn.Stop(splashTwn.id, false);

        splashTwn.start = 1f;
        splashTwn.end = 0f;
        splashTwn.easeFunction = TwnFunc.CUBIC_EO;
        splashTwn.during = DuringSplashHide;
        splashTwn.onComplete = this.QueueFree;

        Twn.Start(ref splashTwn);
    }

    public void SubLogoShown() 
	{ 
		// NOTE(RYAN_2023-07-14):
		// since we're doing a timer one shot (aren't returned the timer that
		// is ticking and thus can't do gc), going to prevent quitting after
		// this point
		canQuit = false; 

		Tmr.Start(2f, HideSplash); 
	}

	public void DuringSubLogoReveal(float alpha)
	{
		transparency.A = alpha;

        SubLogos.Modulate = transparency;
	}

	public void NFALogoShown()
	{
        transparency.A = 0f;

        splashTwn.during = DuringSubLogoReveal;
        splashTwn.onComplete = SubLogoShown;

        Twn.Start(ref splashTwn);
    }

	public void DuringNFALogoReveal(float alpha)
	{
		transparency.A = alpha;

        NoFunAllowedLogo.SelfModulate = transparency;
	}

	public void WhiteForegroundShown()
	{
		transparency.A = 0f;
        
		splashTwn.during = DuringNFALogoReveal;
        splashTwn.onComplete = NFALogoShown;

        Twn.Start(ref splashTwn);
    }

	public void DuringWhiteForegroundReveal(float alpha)
	{
		transparency.A = alpha;
        WhiteForgrnd.Modulate = transparency;
    }

    public void ShowSplash()
	{
		splashTwn.start = 0f;
        splashTwn.end = 1f;
        splashTwn.duration = 2f;
        splashTwn.easeFunction = TwnFunc.CUBIC_EO;
		splashTwn.during = DuringWhiteForegroundReveal;
		splashTwn.onComplete = WhiteForegroundShown; 

		Twn.Start(ref splashTwn);
    }

	public override void _Ready()
	{
        transparency = shdr.Wht(0);

		WhiteForgrnd.Modulate = transparency;
		NoFunAllowedLogo.SelfModulate = transparency;
		SubLogos.Modulate = transparency;

        Ftmr.Start(2, ShowSplash);
    }

    public override void _Input(InputEvent @event)
    {
		if(canQuit == false) return;

		bool anyKeyPressed = @event.IsPressed();
		if(anyKeyPressed) { HideSplash(); }
    }
}
