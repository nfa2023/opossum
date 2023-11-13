using Godot;

namespace NFA.Accessibility
{
    public unsafe static class TTS
    {
        public static bool usable = false;
        public static bool enabled;
        public static float rate;
        public static string currVoiceId;
        public static int volume;

        public static readonly string PROJ_OPT = "audio/general/text_to_speech";
        public static int VOICE_CT;
        public static string[] VOICE_IDS;
        public static string[] VOICE_NAMES;

        public static string say;

        public static void UpdateVolume()
        {
            volume = (int)(mgt.snd.TTS * mgt.snd.MASTER * 100f);
        }

        private static int ExtractName(in string id)
        {
            int i = id.Length - 1;
            while (i > 0 && id[i - 1] != '\\')
            {
                --i;
            }
            return i;
        }

        public static void FetchVoices(in string locale)
        {
            if (ProjectSettings.GetSetting(PROJ_OPT).AsBool() == false)
            {
#if DEBUG
                GD.PrintErr("MUST HAVE TTS ENABLED IN PROJECT SETTINGS!");
#endif
                usable = false;
            }

            VOICE_IDS = DisplayServer.TtsGetVoicesForLanguage(locale);
            if (VOICE_IDS.Length < 1) { usable = false; }

            VOICE_CT = VOICE_IDS.Length;
            VOICE_NAMES = new string[VOICE_CT];

            const int maxStrLen = 128;
            char* voiceName = stackalloc char[maxStrLen];

            int startIdx = 0;
            int charIdx = 0;
            int idLen = 0;

            for (int nameIdx = 0; nameIdx < VOICE_CT; ++nameIdx)
            {
                idLen = VOICE_IDS[nameIdx].Length;
                startIdx = ExtractName(in VOICE_IDS[nameIdx]);

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

        public static void UpdateCurrVoiceId(int id)
        {
            currVoiceId = VOICE_IDS[id];
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

            DisplayServer.TtsSpeak(say, currVoiceId, volume, 1f, rate, 0, true);
        }

        public static void ForceSpeak(string s)
        {
            if (usable == false || enabled == false || volume <= 0 ||
                Str.Invalid(in s)) { return; }

            DisplayServer.TtsSpeak(s, currVoiceId, volume, 1f, rate, 0, true);
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
