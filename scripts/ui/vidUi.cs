using Godot;

public partial class vidUi : Node
{
    public int resState = 0;    public int fullScrnState = 0;
    public int vsyncState = 0;  public int scrnShakeState = 0;

    [Export] public AspectRatioContainer VidOpts;
    [Export] public TextureButton VidOptsBtn;
    [Export] public TextureButton ResetVidBtn;
    [Export] public TextureButton Ret2OptsBtn;

    [Export] public TextureButton ResDec; [Export] public Label ResTxt; [Export] public TextureButton ResInc;
    [Export] public CheckButton FullScrnCheck;
    [Export] public CheckButton VsyncCheck;
    [Export] public CheckButton ScrnShakeCheck;

    public static readonly Vector2I[] RES_V2I =
    {
        new Vector2I(1024, 768),
        new Vector2I(1280, 720),
        new Vector2I(1360, 768),
        new Vector2I(1366, 768),
        new Vector2I(1440, 900),
        new Vector2I(1600, 900),
        new Vector2I(1680, 1050),
        new Vector2I(1920, 1080),
        new Vector2I(2560, 1080),
        new Vector2I(2560, 1440),
        new Vector2I(2560, 1600),
        new Vector2I(3840, 2160),
    };
    public static readonly string[] RES_STRS =
    {
        "1024 x 768",   "1280 x 720",   "1360 x 760",
        "1366 x 768",   "1440 x 900",   "1600 x 900",
        "1680 x 1050",  "1920 x 1080",  "2560 x 1080",
        "2560 x 1440",  "2560 x 1600",  "3840 x 2160"
    };
    public const int RES_CT = 12;
    public const int HALF_RES_CT = RES_CT >> 1;
    public int currResIdx;

    public void TogFullScrn()
    {
        bool tog = FullScrnCheck.ButtonPressed;
        ResDec.Disabled = tog; ResInc.Disabled = tog;

        if (tog)
        {
            ++fullScrnState;
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
        else
        {
            --fullScrnState;
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }
    }

    public void TogVysnc()
    {
        bool tog = VsyncCheck.ButtonPressed;

        if (tog)
        {
            ++vsyncState;
            DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Enabled);
        }
        else
        {
            --vsyncState;
            DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);
        }
    }

    public void TogScrnShake()
    {
        mgt.opts.ShakeScreen = ScrnShakeCheck.ButtonPressed;

        if (mgt.opts.ShakeScreen) { ++scrnShakeState; }
        else { --scrnShakeState; }
    }

    public void IncRes()
    {
        if (currResIdx == RES_CT - 1) { return; }

        currResIdx = mth.Cap(currResIdx + 1, RES_CT - 1);
        DisplayServer.WindowSetSize(RES_V2I[currResIdx]);
        ResTxt.Text = RES_STRS[currResIdx];
    }

    public void DecRes()
    {
        if(currResIdx == 0) { return; }

        currResIdx = mth.Plug(currResIdx - 1, 0);
        DisplayServer.WindowSetSize(RES_V2I[currResIdx]);
        ResTxt.Text = RES_STRS[currResIdx];
    }

    public void ShowVid()
    {
        resState = fullScrnState = vsyncState = scrnShakeState = 0;

        mgt.opts.OptsMenu.Visible = false;
        VidOpts.Visible = true;
    }

    public void HideVid()
    {
        mgt.opts.OptsMenu.Visible = true;
        VidOpts.Visible = false;
    }

    public void Reset()
    {
        // this is really only here to make the debug directive slightly easier
        bool fullscrnEnabled = true;

#if DEBUG
        fullscrnEnabled = false;
        FullScrnCheck.ButtonPressed = fullscrnEnabled;
        ResDec.Disabled = ResInc.Disabled = fullscrnEnabled;
        DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
#else
        FullScrnCheck.ButtonPressed = fullscrnEnabled;
        ResDec.Disabled = ResInc.Disabled = fullscrnEnabled;
        DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
#endif

        VsyncCheck.ButtonPressed = false;
        DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);

        ScrnShakeCheck.ButtonPressed = true;
        mgt.opts.ShakeScreen = true;
    }

    public void Init()
    {
        bool SavedOptsFound = false;
        if (SavedOptsFound) { }
        else { Reset(); }
    }

    public override void _Ready()
	{
        VidOptsBtn.ButtonUp += ShowVid;
        Ret2OptsBtn.ButtonUp += HideVid;
        ResetVidBtn.ButtonUp += Reset;

        ResDec.ButtonUp += DecRes;
        ResInc.ButtonUp += IncRes;
        FullScrnCheck.ButtonUp += TogFullScrn;
        VsyncCheck.ButtonUp += TogVysnc;
        ScrnShakeCheck.ButtonUp += TogScrnShake;
        VidOpts.Visible = false;

        Ftmr.Start(1, Init);
    }
}
