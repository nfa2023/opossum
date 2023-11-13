using Godot;
using ProtoBuf;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct SERIAL_SND
{
    public float MASTER = 0.5f;
    public float EFFECTS = 0.5f;
    public float AMBIENCE = 0.5f;
    public float MUSIC = 0.5f;
    public float TTS = 0.5f;

    public SERIAL_SND()
    {
        MASTER = 0.5f;
        EFFECTS = 0.5f;
        AMBIENCE = 0.5f;
        MUSIC = 0.5f;
        TTS = 0.5f;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct SERIAL_VIDEO
{
    public Vector2I RESOLUTION = new Vector2I(1280, 720);
    public int BRIGHTNESS = 50;

    public bool FULLSCREEN_ENABLED = true;
    public bool VSYNC_ENABLED = false;
    public bool SCREEN_SHAKE_ENABLED = true;

    public SERIAL_VIDEO()
    {
        RESOLUTION = new Vector2I(1280, 720);
        BRIGHTNESS = 50;

        FULLSCREEN_ENABLED = true;
        VSYNC_ENABLED = false;
        SCREEN_SHAKE_ENABLED = true;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct SERIAL_ACCESSIBILITY
{
    public bool DYSLEXIC_FONT_ENABLED = true;
    public int FONT_SIZE = 36;
    public int FONT_OUTLINE_SIZE = 7;
    public int FONT_COLOR_INDEX = utl.WHITE;
    public int FONT_OUTLINE_COLOR_INDEX = utl.BLACK;

    public bool TTS_ENABLED = false;
    public int TTS_RATE = 10;
    public string LOCALE = "en";
    public int TTS_VOICE_INDEX = 0;

    public SERIAL_ACCESSIBILITY()
    {
        DYSLEXIC_FONT_ENABLED = true;
        FONT_SIZE = 36;
        FONT_OUTLINE_SIZE = 7;
        FONT_COLOR_INDEX = utl.WHITE;
        FONT_OUTLINE_COLOR_INDEX = utl.BLACK;

        TTS_ENABLED = false;
        TTS_RATE = 10;
        LOCALE = "en";
        TTS_VOICE_INDEX = 0;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct SERIAL_CONTROL_BINDING
{

}

[ProtoContract]
public class SerialOptions
{
    [ProtoMember(0)]
    public SERIAL_SND SOUND = new SERIAL_SND();

    [ProtoMember(1)]
    public SERIAL_VIDEO VIDEO = new SERIAL_VIDEO();

    [ProtoMember(2)]
    public SERIAL_ACCESSIBILITY ACCESSIBILITY = new SERIAL_ACCESSIBILITY();

    [ProtoMember(3)]
    public SERIAL_CONTROL_BINDING BUTTON_MAP = new SERIAL_CONTROL_BINDING();
}
