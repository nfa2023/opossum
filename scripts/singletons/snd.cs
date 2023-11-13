using Godot;

public enum SND { MASTER = 0, EFFECTS, AMBIENCE, MUSIC, TTS }

public partial class snd : Node
{
    public const int MASTER_IDX = (int)SND.MASTER;
    public const int EFFECTS_IDX = (int)SND.EFFECTS;
    public const int AMBIENCE_IDX = (int)SND.AMBIENCE;
    public const int MUSIC_IDX = (int)SND.MUSIC;
    public const int TTS_IDX = (int)SND.TTS;

    // NOTE(RYAN_2023-07-25): As a general rule, we're going to keep audio as
    // a linear quantity. If you want the volume in decibles, you will need to
    // request it as such.
    public float MASTER     = 0.5f;
    public float EFFECTS    = 0.5f;
    public float AMBIENCE   = 0.5f;
    public float MUSIC      = 0.5f;
    public float TTS        = 0.5f;

    public void SetVols2Bus()
    {
        float masterVol_DB      = mth.Ln2Db(ref MASTER);
        float effectsVol_DB     = mth.Ln2Db(ref EFFECTS);
        float ambienceVol_DB    = mth.Ln2Db(ref AMBIENCE);
        float musicVol_DB       = mth.Ln2Db(ref MUSIC);
        float ttsVol_DB         = mth.Ln2Db(ref TTS);

        AudioServer.SetBusVolumeDb(MASTER_IDX,      masterVol_DB);
        AudioServer.SetBusVolumeDb(EFFECTS_IDX,     effectsVol_DB);
        AudioServer.SetBusVolumeDb(AMBIENCE_IDX,    ambienceVol_DB);
        AudioServer.SetBusVolumeDb(MUSIC_IDX,       musicVol_DB);
        AudioServer.SetBusVolumeDb(TTS_IDX,         ttsVol_DB);
    }

    public void SetVol(SND sndType, float vol)
    {
        switch (sndType)
        {
            case SND.MASTER:    MASTER = vol; return;
            case SND.EFFECTS:   EFFECTS = vol; return;
            case SND.AMBIENCE:  AMBIENCE = vol; return;
            case SND.MUSIC:     MUSIC = vol; return;
            case SND.TTS:       TTS = vol; return;
            default: return;
        }
    }

    public float GetVol(SND sndType)
    {
        switch (sndType)
        {
            case SND.MASTER:    return MASTER;
            case SND.EFFECTS:   return EFFECTS;
            case SND.AMBIENCE:  return AMBIENCE;
            case SND.MUSIC:     return MUSIC;
            case SND.TTS:       return TTS;
            default:            return -1f;
        }
    }

    public void DeserializeVols()
    {
        MASTER = SaveLoad.OptionsData.SOUND.MASTER;
        EFFECTS = SaveLoad.OptionsData.SOUND.EFFECTS;
        AMBIENCE = SaveLoad.OptionsData.SOUND.AMBIENCE;
        MUSIC = SaveLoad.OptionsData.SOUND.MUSIC;
        TTS = SaveLoad.OptionsData.SOUND.TTS;
    }

    public void SerializeVols()
    {
        SaveLoad.OptionsData.SOUND.MASTER = MASTER;
        SaveLoad.OptionsData.SOUND.EFFECTS = EFFECTS;
        SaveLoad.OptionsData.SOUND.AMBIENCE = AMBIENCE;
        SaveLoad.OptionsData.SOUND.MUSIC = MUSIC;
        SaveLoad.OptionsData.SOUND.TTS = TTS;
    }
}
