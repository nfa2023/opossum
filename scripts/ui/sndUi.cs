using Godot;

public partial class sndUi : Node
{
	public const int SND_CT = 5;
    public const int VOL_MAX = 100;
	public const int VOL_MIN = 0;
    public const int BASE_VOL = 50;
    public const int MASTER = (int)SND.MASTER;
    public const int EFFECTS = (int)SND.EFFECTS;
    public const int AMBIENCE = (int)SND.AMBIENCE;
    public const int MUSIC = (int)SND.MUSIC;
    public const int TTS = (int)SND.TTS;
	public int[] LnrSndVols = new int[SND_CT];
	public int[] SndState = new int[SND_CT];

    [Export] public AspectRatioContainer SndOpts;
	[Export] public TextureButton SndOptsBtn;
    [Export] public TextureButton ResetSndBtn;
	[Export] public TextureButton Ret2OptsBtn;

    /*
	// There is a bug in Godot that prevents these arrays from being populated
	// in the editor. It is scheduled to be fixed but further down the line.
	// Kill me.

	// Here lies a better idea:
    public int[] SndState = new int[SND_CT];
	[Export] public Button[] SndIncs = new Button[SND_CT];
	[Export] public Button[] SndDecs = new Button[SND_CT];
    [Export] public Label[] SndVolTxts = new Label[SND_CT];
	*/

    [Export] public TextureButton MasterInc;	[Export] public TextureButton MasterDec;	
    [Export] public TextureButton EffectsInc;	[Export] public TextureButton EffectsDec;	
    [Export] public TextureButton AmbienceInc;  [Export] public TextureButton AmbienceDec; 
    [Export] public TextureButton MusicInc;	    [Export] public TextureButton MusicDec;
    [Export] public TextureButton TtsInc;	    [Export] public TextureButton TtsDec;

    [Export] public Label MasterLabel;		
    [Export] public Label EffectsLabel;		
    [Export] public Label AmbienceLabel;	
    [Export] public Label MusicLabel;		
    [Export] public Label TtsLabel;		

    public void ShowSnd()
	{
		// reset sound state
		for(int i = 0; i < SND_CT; ++i) { SndState[i] = 0; }

	    mgt.opts.OptsMenu.Visible = false;
        SndOpts.Visible = true;
	}

	public void HideSnd()
	{
		bool saveSndOpts =	SndState[MASTER]    != 0	||
                            SndState[EFFECTS]   != 0	||
                            SndState[AMBIENCE]  != 0	||
                            SndState[MUSIC]     != 0    ||
                            SndState[TTS]       != 0;

        if (saveSndOpts) { SetVols2Snd(); }

        mgt.opts.OptsMenu.Visible = true;
        SndOpts.Visible = false;
    }

    public void SetVols2Snd()
    {
        mgt.snd.MASTER = LnrSndVols[MASTER] * 0.01f;
        mgt.snd.EFFECTS = LnrSndVols[EFFECTS] * 0.01f;
        mgt.snd.AMBIENCE = LnrSndVols[AMBIENCE] * 0.01f;
        mgt.snd.MUSIC = LnrSndVols[MUSIC] * 0.01f;
        mgt.snd.TTS = LnrSndVols[TTS] * 0.01f;

        mgt.snd.SetVols2Bus();
    }

	public Label GetVolLabel(SND sndType)
	{
		switch (sndType)
		{
			case SND.MASTER: return MasterLabel;
			case SND.EFFECTS: return EffectsLabel;
			case SND.AMBIENCE: return AmbienceLabel;
			case SND.MUSIC: return MusicLabel;
			case SND.TTS: return TtsLabel;
			default: return null;
		}
	}

	public void IncSnd(int sndType)
	{
        if(LnrSndVols[sndType] == VOL_MAX) { return; }

        int newVol = mth.Cap(LnrSndVols[sndType] + 5, VOL_MAX);
        LnrSndVols[sndType] = newVol;
        GetVolLabel((SND)sndType).Text = str.i2s(LnrSndVols[sndType]);
        ++SndState[sndType];
    }

	public void DecSnd(int sndType)
	{
        if (LnrSndVols[sndType] == VOL_MIN) { return; }

        LnrSndVols[sndType] = mth.Plug(LnrSndVols[sndType] - 5, VOL_MIN);
        GetVolLabel((SND)sndType).Text = str.i2s(LnrSndVols[sndType]);
        --SndState[sndType];
    }

    public void Reset()
    {
        for(int i = 0; i < SND_CT; ++i)
        {
            SndState[i] += BASE_VOL - LnrSndVols[i];
            LnrSndVols[i] = BASE_VOL;
        }

        SetVols2Snd();

        MasterLabel.Text = utl._50;
        EffectsLabel.Text = utl._50;
        AmbienceLabel.Text = utl._50;
        MusicLabel.Text = utl._50;
        TtsLabel.Text = utl._50;
    }

    public void InitBaseVols()
    {
        LnrSndVols[MASTER] = BASE_VOL;
        LnrSndVols[EFFECTS] = BASE_VOL;
        LnrSndVols[AMBIENCE] = BASE_VOL;
        LnrSndVols[MUSIC] = BASE_VOL;
        LnrSndVols[TTS] = BASE_VOL;

        MasterLabel.Text = utl._50;
        EffectsLabel.Text = utl._50;
        AmbienceLabel.Text = utl._50;
        MusicLabel.Text = utl._50;
        TtsLabel.Text = utl._50;
    }

    public override void _Ready()
	{
        SndOptsBtn.ButtonUp += ShowSnd;
        Ret2OptsBtn.ButtonUp += HideSnd;
        ResetSndBtn.ButtonUp += Reset;

        MasterInc.ButtonUp += () => IncSnd(MASTER);
        EffectsInc.ButtonUp += () => IncSnd(EFFECTS);
        AmbienceInc.ButtonUp += () => IncSnd(AMBIENCE);
        MusicInc.ButtonUp += () => IncSnd(MUSIC);
        TtsInc.ButtonUp += () => IncSnd(TTS);

        MasterDec.ButtonUp += () => DecSnd(MASTER);
        EffectsDec.ButtonUp += () => DecSnd(EFFECTS);
        AmbienceDec.ButtonUp += () => DecSnd(AMBIENCE);
        MusicDec.ButtonUp += () => DecSnd(MUSIC);
        TtsDec.ButtonUp += () => DecSnd(TTS);

        InitBaseVols();

        SndOpts.Visible = false;
    }
}
