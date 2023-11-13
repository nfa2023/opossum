using Godot;

public enum AudioType { MASTER = 0, EFFECTS, AMBIENCE, MUSIC, TTS }

public partial class Audio : Node
{
    public const int MASTER_IDX = (int)AudioType.MASTER;
    public const int EFFECTS_IDX = (int)AudioType.EFFECTS;
    public const int AMBIENCE_IDX = (int)AudioType.AMBIENCE;
    public const int MUSIC_IDX = (int)AudioType.MUSIC;
    public const int TTS_IDX = (int)AudioType.TTS;

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

        AudioServer.SetBusVolumeDb(MASTER_IDX, masterVol_DB);
        AudioServer.SetBusVolumeDb(EFFECTS_IDX, effectsVol_DB);
        AudioServer.SetBusVolumeDb(AMBIENCE_IDX, ambienceVol_DB);
        AudioServer.SetBusVolumeDb(MUSIC_IDX, musicVol_DB);
        AudioServer.SetBusVolumeDb(TTS_IDX, ttsVol_DB);
    }

    public void SetVol(AudioType sndType, float vol)
    {
        switch (sndType)
        {
            case AudioType.MASTER:    MASTER = vol; return;
            case AudioType.EFFECTS:   EFFECTS = vol; return;
            case AudioType.AMBIENCE:  AMBIENCE = vol; return;
            case AudioType.MUSIC:     MUSIC = vol; return;
            case AudioType.TTS:       TTS = vol; return;
            default: return;
        }
    }

    public float GetVol(AudioType sndType)
    {
        switch (sndType)
        {
            case AudioType.MASTER:    return MASTER;
            case AudioType.EFFECTS:   return EFFECTS;
            case AudioType.AMBIENCE:  return AMBIENCE;
            case AudioType.MUSIC:     return MUSIC;
            case AudioType.TTS:       return TTS;
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
