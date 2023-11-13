using Godot;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

[StructLayout(LayoutKind.Sequential)]
public struct SaveAccTxt
{
    private int full_0;
    public BOOL SaveFontSize;
    public BOOL SaveFontColor;
    public BOOL SaveOutlineSize;
    public BOOL SaveOutlineColor;

    private int full_1;
    public BOOL SaveFont;

    public BOOL SaveTts;
    public BOOL SaveTtsRate;
    public BOOL SaveTtsVoice;

    public SaveAccTxt(int fontSzState, int fontClrState, int outlineSzState, int outlineClrState, int fontState, int ttsState, int ttsRateState, int ttsVoiceState)
    {
        full_0 = 0;
        SaveFontSize = fontSzState != 0 ? BOOL.TRUE : BOOL.FALSE;
        SaveFontColor = fontClrState != 0 ? BOOL.TRUE : BOOL.FALSE;
        SaveOutlineSize = outlineSzState != 0 ? BOOL.TRUE : BOOL.FALSE;
        SaveOutlineColor = outlineClrState != 0 ? BOOL.TRUE : BOOL.FALSE;

        full_1 = 0;
        SaveFont = fontState != 0 ? BOOL.TRUE : BOOL.FALSE;

        SaveTts = ttsState != 0 ? BOOL.TRUE : BOOL.FALSE;
        SaveTtsRate = ttsRateState != 0 ? BOOL.TRUE : BOOL.FALSE;
        SaveTtsVoice = ttsVoiceState != 0 ? BOOL.TRUE : BOOL.FALSE;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool UpdateTextAccessibility()
    {
        return SaveFontSize == BOOL.TRUE ||
                SaveFontColor == BOOL.TRUE ||
                SaveOutlineSize == BOOL.TRUE ||
                SaveOutlineColor == BOOL.TRUE ||
                SaveFont == BOOL.TRUE;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool UpdateTts()
    {
        return  SaveTts == BOOL.TRUE || 
                SaveTtsRate == BOOL.TRUE || 
                SaveTtsVoice == BOOL.TRUE;
    }
}

public unsafe partial class accUi : Node
{
    #region Font/Text Vars

    public int FontSzState = 0;     public int FontClrState = 0;
    public int OutlineSzState = 0;  public int OutlineClrState = 0;
    public int DyslexicFontState = 0;

    public const int SZ_PCT_MIN = 0;    public const int SZ_PCT_MAX = 3;
    public const int _75_PCT = 0;       public const int _1OO_PCT_IDX = 1; public const int _15O_PCT = 2;

    public bool usingDyslexicFont;
    public int FontSz = _1OO_PCT_IDX;       public int FontClr = utl.WHITE;
    public int OutlineSz = _1OO_PCT_IDX;    public int OutlineClr = utl.BLACK;

    public Theme testTxtTheme;
    public Theme baseTxtTheme;
    [Export] public Font BaseFont;
    [Export] public Font DyslexicFont;

    [Export] public AspectRatioContainer AccOpts;
    [Export] public TextureButton AccOptsBtn;
    [Export] public TextureButton ResetAccBtn;
    [Export] public TextureButton Ret2OptsBtn;

    [Export] public Label TestTxt;
    [Export] public TextureButton FontSzDec;       [Export] public Label FontSzVal;            [Export] public TextureButton FontSzInc;
    [Export] public TextureButton FontClrDec;      [Export] public ColorRect FontClrRect;      [Export] public TextureButton FontClrInc;
    [Export] public TextureButton OutlineSzDec;    [Export] public Label OutlineSzVal;         [Export] public TextureButton OutlineSzInc;
    [Export] public TextureButton OutlineClrDec;   [Export] public ColorRect OutlineClrRect;   [Export] public TextureButton OutlineClrInc;
    [Export] public CheckButton DyslexicCheck;

    #endregion

    #region Text-to-Speech Vars

    public int TtsState = 0; public int TtsRateState = 0;
    public int TtsVoiceState = 0;

    public const int RATE_MIN = 0;
    public const int BASE_RATE = 10;
    public const int RATE_MAX = 100;
    public static readonly string TTS_TEST_STR = "The juice of lemons makes fine punch. The source of the huge river is the clear spring. It snowed, rained, and hailed the same morning.";

    public int TtsRate = BASE_RATE;
    public int TtsVoice = 0;
    [Export] public CheckButton TtsCheck;
    [Export] public TextureButton TtsRateDec; [Export] public Label TtsRateVal; [Export] public TextureButton TtsRateInc;
    [Export] public TextureButton TtsVoiceDec; [Export] public TextureButton TtsVoiceInc;
    [Export] public TextureButton TtsTest;
    [Export] public Label TtsVoiceLabel;

    #endregion

    #region Text-to-Speech

    public void TestTts() { mgt.acc.TtsSpeak(TTS_TEST_STR); }

    public void TogTts() 
    {
        bool ttsEnabled = TtsCheck.ButtonPressed;
        TtsState += ttsEnabled ? 1 : -1;
        mgt.acc.UpdateTts(ttsEnabled);
    }

    public void DecTtsRate()
    {
        if (TtsRate == RATE_MIN) { return; }

        --TtsRateState;
        TtsRate = mth.Plug(TtsRate - 2, RATE_MIN);
        
        TtsRateVal.Text = str.i2s(TtsRate);
        mgt.acc.ttsRate = TtsRate * 0.1f;
    }

    public void IncTtsRate()
    {
        if (TtsRate == RATE_MAX) { return; }

        ++TtsRateState;
        TtsRate = mth.Cap(TtsRate + 2, RATE_MAX);

        TtsRateVal.Text = str.i2s(TtsRate);
        mgt.acc.ttsRate = TtsRate * 0.1f;
    }

    public void IncVoice()
    {
        int voiceCt = mgt.acc.VOICE_ID_CT - 1;
        if (TtsVoice == voiceCt) { return; }

        TtsVoice = mth.Cap(TtsVoice + 1, voiceCt);
        ++TtsVoiceState;

        TtsVoiceLabel.Text = mgt.acc.TTS_VOICE_NAMES[TtsVoice];
        mgt.acc.UpdateCurrVoiceId(TtsVoice);
    }

    public void DecVoice()
    {
        if (TtsVoice == 0) { return; }

        TtsVoice = mth.Plug(TtsVoice - 1, 0);
        --TtsVoiceState;

        TtsVoiceLabel.Text = mgt.acc.TTS_VOICE_NAMES[TtsVoice];
        mgt.acc.UpdateCurrVoiceId(TtsVoice);
    }

    public void UpdateTts(in SaveAccTxt sat)
    {
        if (sat.SaveTts == BOOL.TRUE)
        {
        }

        if (sat.SaveTtsRate == BOOL.TRUE) 
        { 
        
        }

        if (sat.SaveTtsVoice == BOOL.TRUE)
        {
            mgt.acc.CurrTtsVoiceId = mgt.acc.TTS_VOICE_IDXS[TtsVoice];
        }
    }

    #endregion

    #region Font/Text

    public void TogDyslexicFont()
    {
        mgt.acc.useDyslexicFont = DyslexicCheck.ButtonPressed;
        if(mgt.acc.useDyslexicFont)
        {
            ++DyslexicFontState;
            testTxtTheme.Set(shdr.FONT, DyslexicFont);
        }
        else
        {
            --DyslexicFontState;
            testTxtTheme.Set(shdr.FONT, BaseFont);
        }
    }

    public void UpdateTestTxtClr()
    {
        testTxtTheme.Set(shdr.FONT_COLOR, utl.Clrs[FontClr]);
    }

    public void UpdateTestTxtSize()
    {
        int* FontSizes = stackalloc int[SZ_PCT_MAX] { 27, 36, 54};
        testTxtTheme.Set(shdr.FONT_SIZE, FontSizes[FontSz]);
    }

    public void UpdateTestOutlineSz()
    {
        int* OutlineSizes = stackalloc int[SZ_PCT_MAX] { 2, 7, 15 };
        testTxtTheme.Set(shdr.OUTLINE_SHADOW_SIZE, OutlineSizes[OutlineSz]);
    }

    public void UpdateTestOutlineClr()
    {
        testTxtTheme.Set(shdr.OUTLINE_COLOR, utl.Clrs[OutlineClr]);
        testTxtTheme.Set(shdr.OUTLINE_SHADOW_COLOR, utl.Clrs[OutlineClr]);
    }

    public void UpdateTestTxt()
    {
        testTxtTheme.Set(shdr.FONT_COLOR, utl.Clrs[FontClr]);

        int* FontSizes = stackalloc int[SZ_PCT_MAX] { 27, 36, 54 };
        testTxtTheme.Set(shdr.FONT_SIZE, FontSizes[FontSz]);

        int* OutlineSizes = stackalloc int[SZ_PCT_MAX] { 2, 5, 15 };
        testTxtTheme.Set(shdr.OUTLINE_SHADOW_SIZE, OutlineSizes[OutlineSz]);

        testTxtTheme.Set(shdr.OUTLINE_COLOR, utl.Clrs[OutlineClr]);
        testTxtTheme.Set(shdr.OUTLINE_SHADOW_COLOR, utl.Clrs[OutlineClr]);

        testTxtTheme.Set(shdr.FONT, mgt.acc.useDyslexicFont ? DyslexicFont : BaseFont);
    }

    public void IncOutlineClr()
    {
        if (++OutlineClr > utl.YELLOW) { OutlineClr -= utl.CLR_MAX; }

        OutlineClrRect.Color = utl.Clrs[OutlineClr];
        ++OutlineClrState;

        UpdateTestOutlineClr();
    }

    public void DecOutlineClr()
    {
        if (--OutlineClr < 0) { OutlineClr += utl.CLR_MAX; }

        OutlineClrRect.Color = utl.Clrs[OutlineClr];
        --OutlineClrState;

        UpdateTestOutlineClr();
    }

    public void IncOutlineSz()
    {
        if (OutlineSz == _15O_PCT) { return; }

        OutlineSz = mth.Cap(OutlineSz + 1, _15O_PCT);

        int* PERCENTAGES = stackalloc int[SZ_PCT_MAX] { 75, 100, 150 };
        OutlineSzVal.Text = utl.i2sPercent(PERCENTAGES[OutlineSz]);
        ++OutlineSzState;

        UpdateTestOutlineSz();
    }

    public void DecOutlineSz()
    {
        if (OutlineSz == SZ_PCT_MIN) { return; }

        OutlineSz = mth.Plug(OutlineSz - 1, SZ_PCT_MIN);

        int* PERCENTAGES = stackalloc int[SZ_PCT_MAX] { 75, 100, 150 };
        OutlineSzVal.Text = utl.i2sPercent(PERCENTAGES[OutlineSz]);
        --OutlineSzState;

        UpdateTestOutlineSz();
    }

    public void IncFontClr()
    {
        if (++FontClr > utl.YELLOW) { FontClr -= utl.CLR_MAX; }

        FontClrRect.Color = utl.Clrs[FontClr];
        ++FontClrState;

        UpdateTestTxtClr();
    }

    public void DecFontClr()
    {
        if (--FontClr < 0) { FontClr += utl.CLR_MAX; }

        FontClrRect.Color = utl.Clrs[FontClr];
        --FontClrState;

        UpdateTestTxtClr();
    }

    public void IncFontSz()
    {
        if (FontSz == _15O_PCT) { return; }

        FontSz = mth.Cap(FontSz + 1, _15O_PCT);

        int* PERCENTAGES = stackalloc int[SZ_PCT_MAX] { 75, 100, 150 };
        FontSzVal.Text = utl.i2sPercent(PERCENTAGES[FontSz]);
        ++FontSzState;

        UpdateTestTxtSize();
    }

    public void DecFontSz()
    {
        if (FontSz == SZ_PCT_MIN) { return; }

        FontSz = mth.Plug(FontSz - 1, SZ_PCT_MIN);

        int* PERCENTAGES = stackalloc int[SZ_PCT_MAX] { 75, 100, 150 };
        FontSzVal.Text = utl.i2sPercent(PERCENTAGES[FontSz]);
        --FontSzState;

        UpdateTestTxtSize();
    }

    public void UpdateGameTxt(in SaveAccTxt sat)
    { 
        if (sat.SaveFontSize == BOOL.TRUE)
        {
            int* FontSizes = stackalloc int[SZ_PCT_MAX] { 27, 36, 54 };
            baseTxtTheme.Set(shdr.FONT_SIZE, FontSizes[FontSz]);
        }

        if (sat.SaveFontColor == BOOL.TRUE)
        {
            baseTxtTheme.Set(shdr.FONT_COLOR, utl.Clrs[FontClr]);
        }

        if (sat.SaveOutlineColor == BOOL.TRUE)
        {
            baseTxtTheme.Set(shdr.OUTLINE_COLOR, utl.Clrs[OutlineClr]);
            baseTxtTheme.Set(shdr.OUTLINE_SHADOW_COLOR, utl.Clrs[OutlineClr]);
        }

        if (sat.SaveOutlineSize == BOOL.TRUE)
        {
            int* OutlineSizes = stackalloc int[SZ_PCT_MAX] { 2, 7, 15 };
            testTxtTheme.Set(shdr.OUTLINE_SHADOW_SIZE, OutlineSizes[OutlineSz]);
        }

        if (sat.SaveFont == BOOL.TRUE) 
        {
            if (mgt.acc.useDyslexicFont) 
            { 
                baseTxtTheme.Set(shdr.FONT, DyslexicFont); 
            }
            else { baseTxtTheme.Set(shdr.FONT, BaseFont); }
        }
    }

    #endregion

    public void ShowAcc()
    {
        FontSzState = FontClrState = OutlineSzState = OutlineClrState = DyslexicFontState = 0;
        mgt.opts.OptsMenu.Visible = false;
        AccOpts.Visible = true;
    }

    public void HideAcc()
    {
        SaveAccTxt s = new SaveAccTxt(  FontSzState, FontClrState, 
                                        OutlineSzState, OutlineClrState, 
                                        DyslexicFontState, TtsState, 
                                        TtsRateState, TtsVoiceState);

        if (s.UpdateTextAccessibility())    { UpdateGameTxt(in s); }
        if (s.UpdateTts())                  { UpdateTts(in s); }

        mgt.opts.OptsMenu.Visible = true;
        AccOpts.Visible = false;
    }

    public void Reset()
    {
        #region Font/Text

        testTxtTheme = TestTxt.Theme;
        baseTxtTheme = FontSzVal.Theme;

        FontSzState = FontSz - _1OO_PCT_IDX;
        FontClrState = FontClr - utl.WHITE;
        OutlineSzState = OutlineSz - _1OO_PCT_IDX;
        OutlineClrState = OutlineClr - utl.BLACK;

        FontSz = _1OO_PCT_IDX;
        FontSzVal.Text = utl._100_PCT;
        FontClr = utl.WHITE;
        FontClrRect.Color = utl.Clrs[FontClr];

        OutlineSz = _1OO_PCT_IDX;
        OutlineSzVal.Text = utl._100_PCT;
        OutlineClr = utl.BLACK;
        OutlineClrRect.Color = utl.Clrs[OutlineClr];
        
        if (DyslexicCheck.ButtonPressed)
        {
            DyslexicCheck.ButtonPressed = false;
            --DyslexicFontState;
        }
        mgt.acc.useDyslexicFont = false;

        UpdateTestTxt();

        #endregion

        #region Text-to-Speech

        if (TtsCheck.ButtonPressed) 
        {
            TtsCheck.ButtonPressed = false;
            --TtsState;
        }

        mgt.acc.DisableTts();

        TtsRateState = 0;
        TtsRate = BASE_RATE;
        TtsRateVal.Text = utl._10;
        mgt.acc.ttsRate = BASE_RATE * 0.1f;

        TtsVoice = 0;
        TtsVoiceState = 0;
        mgt.acc.CurrTtsVoiceId = mgt.acc.TTS_VOICE_IDXS[TtsVoice];
        TtsVoiceLabel.Text = mgt.acc.TTS_VOICE_NAMES[0];

        #endregion
    }

    public void Init()
    {
        bool SavedOptsFound = false;
        if (SavedOptsFound) { }
        else 
        {
            mgt.acc.Fetch_EN_Voices();
            Reset(); 
        }
    }

    public override void _Ready()
    {
        //GD.Print("sizeof TwnUtl._state: " + sizeof(TwnUtl.twn));

        AccOptsBtn.ButtonUp += ShowAcc;
        Ret2OptsBtn.ButtonUp += HideAcc;
        ResetAccBtn.ButtonUp += Reset;

        FontSzDec.ButtonUp += DecFontSz;    FontSzInc.ButtonUp += IncFontSz;
        FontClrDec.ButtonUp += DecFontClr;  FontClrInc.ButtonUp += IncFontClr;

        OutlineSzDec.ButtonUp += DecOutlineSz;     OutlineSzInc.ButtonUp += IncOutlineSz;
        OutlineClrDec.ButtonUp += DecOutlineClr;   OutlineClrInc.ButtonUp += IncOutlineClr;

        DyslexicCheck.ButtonUp += TogDyslexicFont;

        TtsCheck.ButtonUp += TogTts;
        TtsRateDec.ButtonUp += DecTtsRate; TtsRateInc.ButtonUp += IncTtsRate;

        TtsVoiceDec.ButtonUp += DecVoice; TtsVoiceInc.ButtonUp += IncVoice;
        TtsTest.ButtonUp += TestTts;

        AccOpts.Visible = false;

        Ftmr.Start(1, Init);
    }
}