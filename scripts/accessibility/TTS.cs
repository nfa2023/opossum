using Godot;

namespace NFA.Accessibility
{
    public unsafe static class TTS
    {
        public static bool usable = false;
        public static bool enabled;
        public static float rate;
        public static string currVoice;
        public static int volume;

        public static readonly string PROJ_OPT = "audio/general/text_to_speech";
        public static int voiceCt;
        public static string[] VOICE_IDS;
        public static string[] VOICE_NAMES;

        public static string say;

        public static void UpdateVol()
        {
            volume = (int)(mgt.snd.TTS * mgt.snd.MASTER * 100f);
        }

        private static int NameIdx(in string id, int len)
        {
            int i = len - 1;
            while (i > 0 && id[i - 1] != '\\')
            {
                --i;
            }
            return i;
        }

        public static void FetchVoices(in string locale)
        {
            bool ttsOptEnabled = ProjectSettings.GetSetting(PROJ_OPT).AsBool();
            if (ttsOptEnabled == false)
            {
#if DEBUG
                GD.PrintErr("MUST HAVE TTS ENABLED IN PROJECT SETTINGS!");
#endif
                usable = false;
                return;
            }

            VOICE_IDS = DisplayServer.TtsGetVoicesForLanguage(locale);
            if (VOICE_IDS.Length < 1) { usable = false; return; }

            voiceCt = VOICE_IDS.Length;
            VOICE_NAMES = new string[voiceCt];

            const int maxStrLen = 128;
            char* voiceName = stackalloc char[maxStrLen];

            int startIdx = 0;
            int charIdx = 0;
            int idLen = 0;

            for (int nameIdx = 0; nameIdx < voiceCt; ++nameIdx)
            {
                idLen = VOICE_IDS[nameIdx].Length;
                startIdx = NameIdx(in VOICE_IDS[nameIdx], idLen);

                for (charIdx = 0; charIdx < maxStrLen && startIdx < idLen;)
                {
                    if (VOICE_IDS[nameIdx][startIdx] != '_')
                    {
                        voiceName[charIdx] = VOICE_IDS[nameIdx][startIdx];
                    }
                    else { voiceName[charIdx] = ' '; }

                    ++charIdx;
                    ++startIdx;
                }

                VOICE_NAMES[nameIdx] = new string(voiceName, 0, charIdx);
            }
            usable = true;
        }

        public static void SetVoice(int id)
        {
            currVoice = VOICE_IDS[mth.Clamp(id, 0, voiceCt)];
        }

        public static void Pause()
        {
            if (DisplayServer.TtsIsSpeaking()) { DisplayServer.TtsPause(); }
        }

        public static void Stop()
        {
            if (DisplayServer.TtsIsSpeaking()) { DisplayServer.TtsStop(); }
        }

        public static void Speak()
        {
            if (usable == false || enabled == false || volume <= 0 ||
                Str.Invalid(in say)) { return; }

            DisplayServer.TtsSpeak(say, currVoice, volume, 1f, rate, 0, true);
        }

        public static void ForceSpeak(string s)
        {
            if (usable == false || enabled == false || volume <= 0 ||
                Str.Invalid(in s)) { return; }

            DisplayServer.TtsSpeak(s, currVoice, volume, 1f, rate, 0, true);
        }

        public static void Enable()
        {
            enabled = true;
            ProjectSettings.SetSetting(PROJ_OPT, true);
        }

        public static void Disable()
        {
            enabled = false;
            if (DisplayServer.TtsIsSpeaking()) { DisplayServer.TtsStop(); }
            ProjectSettings.SetSetting(PROJ_OPT, false);
        }
    }
}
