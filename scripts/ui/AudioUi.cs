using Godot;

public partial class AudioUi : Node
{
    #region Const/Readonly Vars

    public const int SND_CT = 5;
    public const int VOL_MAX = 100;
	public const int VOL_MIN = 0;
    public const int BASE_VOL = 50;
    public const int MASTER = (int)AudioType.MASTER;
    public const int EFFECTS = (int)AudioType.EFFECTS;
    public const int AMBIENCE = (int)AudioType.AMBIENCE;
    public const int MUSIC = (int)AudioType.MUSIC;
    public const int TTS = (int)AudioType.TTS;
	public int[] LnrSndVols = new int[SND_CT];
	public int[] SndState = new int[SND_CT];

    #endregion

    #region Exposed Objects

    [Export] public AspectRatioContainer SndOpts;
	[Export] public TextureButton SndOptsBtn;
    [Export] public TextureButton ResetSndBtn;
	[Export] public TextureButton Ret2OptsBtn;

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

    #endregion

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
        NFA.Accessibility.TTS.UpdateVol();
    }

	public Label GetVolLabel(AudioType sndType)
	{
		switch (sndType)
		{
			case AudioType.MASTER: return MasterLabel;
			case AudioType.EFFECTS: return EffectsLabel;
			case AudioType.AMBIENCE: return AmbienceLabel;
			case AudioType.MUSIC: return MusicLabel;
			case AudioType.TTS: return TtsLabel;
			default: return null;
		}
	}

	public void IncSnd(int sndType)
	{
        if(LnrSndVols[sndType] == VOL_MAX) { return; }

        int newVol = mth.Cap(LnrSndVols[sndType] + 5, VOL_MAX);
        LnrSndVols[sndType] = newVol;
        GetVolLabel((AudioType)sndType).Text = Str.i2s(LnrSndVols[sndType]);
        ++SndState[sndType];
    }

	public void DecSnd(int sndType)
	{
        if (LnrSndVols[sndType] == VOL_MIN) { return; }

        LnrSndVols[sndType] = mth.Plug(LnrSndVols[sndType] - 5, VOL_MIN);
        GetVolLabel((AudioType)sndType).Text = Str.i2s(LnrSndVols[sndType]);
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

        MasterLabel.Text = Utl.FIFTY;
        EffectsLabel.Text = Utl.FIFTY;
        AmbienceLabel.Text = Utl.FIFTY;
        MusicLabel.Text = Utl.FIFTY;
        TtsLabel.Text = Utl.FIFTY;
    }

    public void InitBaseVols()
    {
        LnrSndVols[MASTER] = BASE_VOL;
        LnrSndVols[EFFECTS] = BASE_VOL;
        LnrSndVols[AMBIENCE] = BASE_VOL;
        LnrSndVols[MUSIC] = BASE_VOL;
        LnrSndVols[TTS] = BASE_VOL;

        MasterLabel.Text = Utl.FIFTY;
        EffectsLabel.Text = Utl.FIFTY;
        AmbienceLabel.Text = Utl.FIFTY;
        MusicLabel.Text = Utl.FIFTY;
        TtsLabel.Text = Utl.FIFTY;
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
