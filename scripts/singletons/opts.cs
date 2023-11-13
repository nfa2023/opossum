using Godot;
using System;

public partial class opts : Node
{
    [Export] public AspectRatioContainer OptsMenu;
    //[Export] public Button OptsBtn;
    [Export] public TextureButton OptsBtn;
	[Export] public TextureButton CloseOptsBtn;

    [Export] public TextureButton VideoBtn;
	[Export] public TextureButton CtrlBtn;

    public const int MAX_BRIGHTNESS = 100;
    public const int MIN_BRIGHTNESS = 0;
    public const int BASE_BRIGHTNESS = 50;

    [Export] public AspectRatioContainer BrightnessMenu;
    [Export] public TextureButton BrightBtn;
    [Export] public TextureButton BrightIncBtn;
    [Export] public TextureButton BrightDecBtn;
    [Export] public TextureButton RetFromBrightBtn;
    [Export] public TextureButton ResetBrightBtn;
    [Export] public Label BrightValTxt;
    public int BrightVal;
    public int BrightState = 0;

    public bool ShakeScreen = true;

    public void IncBright()
    {
        if(BrightVal == MAX_BRIGHTNESS) { return; }

        BrightVal = mth.Cap(BrightVal + 5, MAX_BRIGHTNESS);
        BrightValTxt.Text = Str.i2s(BrightVal);
        ++BrightState;
    }

    public void DecBright()
    {
        if(BrightVal == MIN_BRIGHTNESS) { return; }

        BrightVal = mth.Plug(BrightVal - 5, MIN_BRIGHTNESS);
        BrightValTxt.Text = Str.i2s(BrightVal);
        --BrightState;
    }

    public void ResetBrightness()
    {
        BrightState += BASE_BRIGHTNESS - BrightVal;
        BrightVal = BASE_BRIGHTNESS;
        BrightValTxt.Text = Utl.FIFTY;
    }

    public void ShowBrightness()
    {
        BrightState = 0;

        BrightnessMenu.Visible = true;
        OptsMenu.Visible = false;
    }

    public void HideBrightness()
    {
        if(BrightState != 0) { GD.Print("Save Brightness"); }

        BrightnessMenu.Visible = false;
        OptsMenu.Visible = true;
    }

    public void InitBright()
    {
        BrightVal = BASE_BRIGHTNESS;
        BrightValTxt.Text = Utl.FIFTY;
    }

	public void ShowOpts()
	{
		mgt.ui.mainMenuBtns.HideMenuBtns();

        OptsMenu.Visible = true;
    }

	public void HideOpts()
	{
        mgt.ui.mainMenuBtns.ShowMenuBtns();

        OptsMenu.Visible = false;
    }

    public override void _Ready()
	{
		OptsBtn.ButtonUp += ShowOpts;
		CloseOptsBtn.ButtonUp += HideOpts;
        OptsMenu.Visible = false;

        BrightBtn.ButtonUp += ShowBrightness;
        RetFromBrightBtn.ButtonUp += HideBrightness;
        ResetBrightBtn.ButtonUp += ResetBrightness;
        BrightIncBtn.ButtonUp += IncBright;
        BrightDecBtn.ButtonUp += DecBright;
        BrightnessMenu.Visible = false;
        InitBright();
    }
}
