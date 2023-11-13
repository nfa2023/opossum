using Godot;

public partial class acc : Node
{
	public float timeScale = 1f;

	public bool useDyslexicFont = false;

	public bool useTts;
    public string CurrTtsVoiceId;
    public float ttsRate;

    public readonly string EN_STR = "en";
    public readonly string TTS_PROJ_OPT = "audio/general/text_to_speech";
	public int VOICE_ID_CT;
    public string[] TTS_VOICE_IDXS;
    public string[] TTS_VOICE_NAMES;

	public int TtsNameIndex(in string id)
	{
		int i = id.Length - 1;
        while(i > 0 && id[i - 1] != '\\') 
		{ 
			--i;
		}
		return i;
	}

	public unsafe void Clr(ref char* c, int len)
	{
		if(len < 1) { return; }

		while(len > 0) { c[len--] = '\0'; }
	}

	public unsafe void Fetch_EN_Voices()
	{
		TTS_VOICE_IDXS = DisplayServer.TtsGetVoicesForLanguage(EN_STR);
		VOICE_ID_CT = TTS_VOICE_IDXS.Length;
		TTS_VOICE_NAMES = new string[VOICE_ID_CT];

		int maxStrLen = 64;
		char* voiceName = stackalloc char[maxStrLen];
		
		int startIdx = 0;
		int chIndex = 0;
		int idLen = 0;

		for(int nameIdx = 0; nameIdx < VOICE_ID_CT; ++nameIdx)
		{
			idLen = TTS_VOICE_IDXS[nameIdx].Length;
            startIdx = TtsNameIndex(in TTS_VOICE_IDXS[nameIdx]);
			
			for(chIndex = 0; chIndex < maxStrLen && startIdx < idLen;) 
			{
				if(TTS_VOICE_IDXS[nameIdx][startIdx] != '_')
				{
                    voiceName[chIndex] = TTS_VOICE_IDXS[nameIdx][startIdx];
                }
				else { voiceName[chIndex] = ' '; }

                ++chIndex; 
				++startIdx;
            }
			
			TTS_VOICE_NAMES[nameIdx] = new string(voiceName, 0, chIndex);
		}
	}

	public void UpdateCurrVoiceId(int id) { CurrTtsVoiceId = TTS_VOICE_IDXS[id]; }

	public void PauseTts() { if (DisplayServer.TtsIsSpeaking()) { DisplayServer.TtsPause(); } }

	public void StopTts() { if (DisplayServer.TtsIsSpeaking()) { DisplayServer.TtsStop(); } }

    public void TtsSpeak(string s, bool interrupt = true)
	{
		if (useTts == false || str.Invalid(in s)) { return; }

		int vol = (int)(mgt.snd.TTS * mgt.snd.MASTER * 100f);
		if (vol <= 0) { return; }

		DisplayServer.TtsSpeak(s, CurrTtsVoiceId, vol, 1f, ttsRate, 0, interrupt);
	}

	public void EnableTts()
	{
		useTts = true;
		ProjectSettings.SetSetting(TTS_PROJ_OPT, true);
	}

    public void DisableTts()
    {
        useTts = false;
        ProjectSettings.SetSetting(TTS_PROJ_OPT, false);
    }

	public void UpdateTts(bool enabled)
	{
		useTts = enabled;
        ProjectSettings.SetSetting(TTS_PROJ_OPT, enabled);

		if(enabled == false && DisplayServer.TtsIsSpeaking()) { DisplayServer.TtsStop(); }
    }
}
