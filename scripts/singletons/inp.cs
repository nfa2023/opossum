using Godot;

public static class inp
{
	public const int ACT_CT = 8;
	public static readonly string UP_STR = "up";
	public static readonly string DOWN_STR = "down";
	public static readonly string LEFT_STR = "left";
	public static readonly string RIGHT_STR = "right";
	public static readonly string SELECT_STR = "interact";
	public static readonly string EXAMINE_STR = "examine";
	public static readonly string CANCEL_STR = "cancel";
	public static readonly string TTS_SPEAK_STR = "tts_speak";

	public static bool UP_PRESSED = false;
	public static bool DOWN_PRESSED = false;
	public static bool LEFT_PRESSED = false;
	public static bool RIGHT_PRESSED = false;
	public static bool SELECT_PRESSED = false;
	public static bool EXAMINE_PRESSED = false;
	public static bool CANCEL_PRESSED = false;
	public static bool TTS_PRESSED = false;

    public static bool UP_RELEASED = false;
    public static bool DOWN_RELEASED = false;
    public static bool LEFT_RELEASED = false;
    public static bool RIGHT_RELEASED = false;
    public static bool SELECT_RELEASED = false;
    public static bool EXAMINE_RELEASED = false;
    public static bool CANCEL_RELEASED = false;
    public static bool TTS_RELEASED = false;

    public static void CaptureInput(InputEvent @ie)
	{
		UP_PRESSED = @ie.IsActionPressed(UP_STR);
		DOWN_PRESSED = @ie.IsActionPressed(DOWN_STR);
		LEFT_PRESSED = @ie.IsActionPressed(LEFT_STR);
		RIGHT_PRESSED = @ie.IsActionPressed(RIGHT_STR);
		SELECT_PRESSED = @ie.IsActionPressed(SELECT_STR);
		EXAMINE_PRESSED = @ie.IsActionPressed(EXAMINE_STR);
		CANCEL_PRESSED = @ie.IsActionPressed(CANCEL_STR);
		TTS_PRESSED = @ie.IsActionPressed(TTS_SPEAK_STR);

		UP_RELEASED = @ie.IsActionReleased(UP_STR);
		DOWN_RELEASED = @ie.IsActionReleased(DOWN_STR);
		LEFT_RELEASED = @ie.IsActionReleased(LEFT_STR);
		RIGHT_RELEASED = @ie.IsActionReleased(RIGHT_STR);
		SELECT_RELEASED = @ie.IsActionReleased(SELECT_STR);
		EXAMINE_RELEASED = @ie.IsActionReleased(EXAMINE_STR);
		CANCEL_RELEASED = @ie.IsActionReleased(CANCEL_STR);
		TTS_RELEASED = @ie.IsActionReleased(TTS_SPEAK_STR);
    }
}
