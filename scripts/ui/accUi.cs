using Godot;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using NFA.Accessibility;
using static TwnUtl;

[StructLayout(LayoutKind.Sequential)]
public struct SaveAccTxt
{
    public bool SaveFontSize;
    public bool SaveFontColor;
    public bool SaveOutlineSize;
    public bool SaveOutlineColor;

    public bool SaveFont;

    public bool SaveTts;
    public bool SaveTtsRate;
    public bool SaveTtsVoice;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool TextStateChanged()
    {
        return  SaveFont || SaveFontSize || SaveOutlineSize ||
                SaveFontColor || SaveOutlineColor;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool TtsStateChanged()
    {
        return  SaveTts || SaveTtsRate || SaveTtsVoice;
    }
}

public unsafe partial class accUi : Node
{
    #region Font/Text Vars

    public enum Percent { _75 = 0, _100, _150 }

    public int FontSzState = 0;
    public int FontClrState = 0;
    public int OutlineSzState = 0;
    public int OutlineClrState = 0;
    public int DyslexicFontState = 0;

    public const int SZ_PCT_MIN = 0;
    public const int SZ_PCT_MAX = 3;

    public bool usingDyslexicFont;
    public Percent FontSz = Percent._100;
    public Colors FontClr = Colors.White;

    public Percent OutlineSz = Percent._100;
    public Colors OutlineClr = Colors.White;

    public Theme testTxtTheme;
    public Theme baseTxtTheme;
    [Export] public Font BaseFont;
    [Export] public Font DyslexicFont;

    [Export] public AspectRatioContainer AccOpts;
    [Export] public TextureButton AccOptsBtn;
    [Export] public TextureButton ResetAccBtn;
    [Export] public TextureButton Ret2OptsBtn;

    [Export] public Label TestTxt;
    [Export] public TextureButton FontSzDec;
    [Export] public Label FontSzVal;
    [Export] public TextureButton FontSzInc;

    [Export] public TextureButton FontClrDec;
    [Export] public ColorRect FontClrRect;
    [Export] public TextureButton FontClrInc;

    [Export] public TextureButton OutlineSzDec;
    [Export] public Label OutlineSzVal;
    [Export] public TextureButton OutlineSzInc;

    [Export] public TextureButton OutlineClrDec;
    [Export] public ColorRect OutlineClrRect;
    [Export] public TextureButton OutlineClrInc;

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

    public void TogTts()
    {
        if (TTS.usable == false) { return; }

        bool ttsEnabled = TtsCheck.ButtonPressed;
        TtsState += ttsEnabled ? 1 : -1;

        if(ttsEnabled) { TTS.Enable(); }
        else { TTS.Disable(); }
    }

    public void DecTtsRate()
    {
        if (TtsRate == RATE_MIN) { return; }

        --TtsRateState;
        TtsRate = mth.Plug(TtsRate - 2, RATE_MIN);

        TtsRateVal.Text = Str.i2s(TtsRate);
        TTS.rate = TtsRate * 0.1f;
    }

    public void IncTtsRate()
    {
        if (TtsRate == RATE_MAX) { return; }

        ++TtsRateState;
        TtsRate = mth.Cap(TtsRate + 2, RATE_MAX);

        TtsRateVal.Text = Str.i2s(TtsRate);
        TTS.rate = TtsRate * 0.1f;
    }

    public void IncVoice()
    {
        int voiceCt = TTS.voiceCt - 1;
        if (TtsVoice == voiceCt) { return; }

        TtsVoice = mth.Cap(TtsVoice + 1, voiceCt);
        ++TtsVoiceState;

        TtsVoiceLabel.Text = TTS.VOICE_NAMES[TtsVoice];
        TTS.SetVoice(TtsVoice);
    }

    public void DecVoice()
    {
        if (TtsVoice == 0) { return; }

        TtsVoice = mth.Plug(TtsVoice - 1, 0);
        --TtsVoiceState;

        TtsVoiceLabel.Text = TTS.VOICE_NAMES[TtsVoice];
        TTS.SetVoice(TtsVoice);
    }

    public void UpdateTts(in SaveAccTxt sat)
    {
        if (sat.SaveTts)
        {
        }

        if (sat.SaveTtsRate)
        {

        }

        if (sat.SaveTtsVoice)
        {
            TTS.currVoice = TTS.VOICE_IDS[TtsVoice];
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
            testTxtTheme.Set(Shdr.FONT, DyslexicFont);
        }
        else
        {
            --DyslexicFontState;
            testTxtTheme.Set(Shdr.FONT, BaseFont);
        }
    }

    public void UpdateTestTxtClr()
    {
        testTxtTheme.Set(Shdr.FONT_COLOR, Shdr.Clrs[(int)FontClr]);
    }

    public void UpdateTestTxtSize()
    {
        int* FontSizes = stackalloc int[SZ_PCT_MAX] { 27, 36, 54};
        testTxtTheme.Set(Shdr.FONT_SIZE, FontSizes[(int)FontSz]);
    }

    public void UpdateTestOutlineSz()
    {
        int* OutlineSizes = stackalloc int[SZ_PCT_MAX] { 2, 7, 15 };
        testTxtTheme.Set(Shdr.OUTLINE_SHADOW_SIZE, OutlineSizes[(int)OutlineSz]);
    }

    public void UpdateTestOutlineClr()
    {
        testTxtTheme.Set(Shdr.OUTLINE_COLOR, Shdr.Clrs[(int)OutlineClr]);
        testTxtTheme.Set(Shdr.OUTLINE_SHADOW_COLOR, Shdr.Clrs[(int)OutlineClr]);
    }

    public void UpdateTestTxt()
    {
        testTxtTheme.Set(Shdr.FONT_COLOR, Shdr.Clrs[(int)FontClr]);

        int* FontSizes = stackalloc int[SZ_PCT_MAX] { 27, 36, 54 };
        testTxtTheme.Set(Shdr.FONT_SIZE, FontSizes[(int)FontSz]);

        int* OutlineSizes = stackalloc int[SZ_PCT_MAX] { 2, 5, 15 };
        testTxtTheme.Set(Shdr.OUTLINE_SHADOW_SIZE, OutlineSizes[(int)OutlineSz]);

        testTxtTheme.Set(Shdr.OUTLINE_COLOR, Shdr.Clrs[(int)OutlineClr]);
        testTxtTheme.Set(Shdr.OUTLINE_SHADOW_COLOR, Shdr.Clrs[(int)OutlineClr]);

        testTxtTheme.Set(Shdr.FONT, mgt.acc.useDyslexicFont ? DyslexicFont : BaseFont);
    }

    public void IncOutlineClr()
    {
        if (++OutlineClr > Colors.Yellow) { OutlineClr -= Shdr.CLR_MAX; }

        OutlineClrRect.Color = Shdr.Clrs[(int)OutlineClr];
        ++OutlineClrState;

        UpdateTestOutlineClr();
    }

    public void DecOutlineClr()
    {
        if (--OutlineClr < 0) { OutlineClr += Shdr.CLR_MAX; }

        OutlineClrRect.Color = Shdr.Clrs[(int)OutlineClr];
        --OutlineClrState;

        UpdateTestOutlineClr();
    }

    public void IncOutlineSz()
    {
        if (OutlineSz >= Percent._150)
        {
            if(OutlineSz > Percent._150) { OutlineSz = Percent._150; }
            return;
        }

        ++OutlineSzState;
        ++OutlineSz;

        int* PERCENTAGES = stackalloc int[SZ_PCT_MAX] { 75, 100, 150 };
        OutlineSzVal.Text = Utl.i2sPercent(PERCENTAGES[(int)OutlineSz]);
        UpdateTestOutlineSz();
    }

    public void DecOutlineSz()
    {
        if (OutlineSz <= Percent._75)
        {
            if (OutlineSz < Percent._75) { OutlineSz = Percent._75; }
            return;
        }

        --OutlineSzState;
        --OutlineSz;

        int* PERCENTAGES = stackalloc int[SZ_PCT_MAX] { 75, 100, 150 };
        OutlineSzVal.Text = Utl.i2sPercent(PERCENTAGES[(int)OutlineSz]);
        UpdateTestOutlineSz();
    }

    public void IncFontClr()
    {
        if (++FontClr > Colors.Yellow) { FontClr -= Shdr.CLR_MAX; }

        FontClrRect.Color = Shdr.Clrs[(int)FontClr];
        ++FontClrState;

        UpdateTestTxtClr();
    }

    public void DecFontClr()
    {
        if (--FontClr < 0) { FontClr += Shdr.CLR_MAX; }

        FontClrRect.Color = Shdr.Clrs[(int)FontClr];
        --FontClrState;

        UpdateTestTxtClr();
    }

    public void IncFontSz()
    {
        if (FontSz >= Percent._150)
        {
            if(FontSz > Percent._150) { FontSz = Percent._150; }
            return;
        }

        ++FontSzState;
        ++FontSz;

        int* PERCENTAGES = stackalloc int[SZ_PCT_MAX] { 75, 100, 150 };
        FontSzVal.Text = Utl.i2sPercent(PERCENTAGES[(int)FontSz]);
        UpdateTestTxtSize();
    }

    public void DecFontSz()
    {
        if (FontSz <= Percent._75)
        {
            if (FontSz < Percent._75) { FontSz = Percent._75; }
            return;
        }

        --FontSzState;
        --FontSz;

        int* PERCENTAGES = stackalloc int[SZ_PCT_MAX] { 75, 100, 150 };
        FontSzVal.Text = Utl.i2sPercent(PERCENTAGES[(int)FontSz]);

        UpdateTestTxtSize();
    }

    public void UpdateGameTxt(in SaveAccTxt sat)
    {
        if (sat.SaveFontSize)
        {
            int* FontSizes = stackalloc int[SZ_PCT_MAX] { 27, 36, 54 };
            baseTxtTheme.Set(Shdr.FONT_SIZE, FontSizes[(int)FontSz]);
        }

        if (sat.SaveFontColor)
        {
            baseTxtTheme.Set(Shdr.FONT_COLOR, Shdr.Clrs[(int)FontClr]);
        }

        if (sat.SaveOutlineColor)
        {
            baseTxtTheme.Set(Shdr.OUTLINE_COLOR, Shdr.Clrs[(int)OutlineClr]);
            baseTxtTheme.Set(Shdr.OUTLINE_SHADOW_COLOR, Shdr.Clrs[(int)OutlineClr]);
        }

        if (sat.SaveOutlineSize)
        {
            int* OutlineSizes = stackalloc int[SZ_PCT_MAX] { 2, 7, 15 };
            testTxtTheme.Set(Shdr.OUTLINE_SHADOW_SIZE, OutlineSizes[(int)OutlineSz]);
        }

        if (sat.SaveFont)
        {
            if (mgt.acc.useDyslexicFont)
            {
                baseTxtTheme.Set(Shdr.FONT, DyslexicFont);
            }
            else { baseTxtTheme.Set(Shdr.FONT, BaseFont); }
        }
    }

    #endregion

    public void ShowAcc()
    {
        FontSzState = 0;
        FontClrState = 0;
        OutlineSzState = 0;
        OutlineClrState = 0;
        DyslexicFontState = 0;

        mgt.opts.OptsMenu.Visible = false;
        AccOpts.Visible = true;
    }

    public void HideAcc()
    {
        SaveAccTxt s = new()
        {
            SaveFontSize = FontSzState != 0,
            SaveFontColor = FontClrState != 0,
            SaveOutlineSize = OutlineSzState != 0,
            SaveOutlineColor = OutlineClrState != 0,

            SaveFont = DyslexicFontState != 0,
            SaveTts = TtsState != 0,
            SaveTtsRate = TtsRateState != 0,
            SaveTtsVoice = TtsVoiceState != 0
        };

        if (s.TextStateChanged()) { UpdateGameTxt(in s); }
        if (s.TtsStateChanged()) { UpdateTts(in s); }

        mgt.opts.OptsMenu.Visible = true;
        AccOpts.Visible = false;
    }

    public void Reset()
    {
        #region Font/Text

        testTxtTheme = TestTxt.Theme;
        baseTxtTheme = FontSzVal.Theme;

        FontSzState = FontSz - Percent._100;
        FontClrState = FontClr - Colors.White;
        OutlineSzState = OutlineSz - Percent._100;
        OutlineClrState = OutlineClr - Colors.Black;

        FontSz = Percent._100;
        FontSzVal.Text = Utl._100_PERCENT;
        FontClr = Colors.White;
        FontClrRect.Color = Shdr.Clrs[(int)FontClr];

        OutlineSz = Percent._100;
        OutlineSzVal.Text = Utl._100_PERCENT;
        OutlineClr = Colors.Black;
        OutlineClrRect.Color = Shdr.Clrs[(int)OutlineClr];

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

        TTS.Disable();

        TtsRateState = 0;
        TtsRate = BASE_RATE;
        TtsRateVal.Text = Utl.TEN;
        TTS.rate = BASE_RATE * 0.1f;

        TtsVoice = 0;
        TtsVoiceState = 0;

        if (TTS.usable)
        {
            TTS.currVoice = TTS.VOICE_IDS[TtsVoice];
            TtsVoiceLabel.Text = TTS.VOICE_NAMES[0];
        }
        else
        {
            TtsVoiceLabel.Text = " ----- ";
        }

        #endregion
    }

    public void Init()
    {
        bool SavedOptsFound = false;
        if (SavedOptsFound) { }
        else
        {
            TTS.FetchVoices(Utl.EN_STR);
            Reset();
        }
    }

    public override void _Ready()
    {
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
        TtsTest.ButtonUp += () => TTS.ForceSpeak(TTS_TEST_STR);

        AccOpts.Visible = false;

        Ftmr.Start(1, Init);
    }
}
