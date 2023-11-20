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

	public static bool FORWARD_HELD = false;
	public static bool BACKWARD_HELD = false;
	public static bool LEFT_HELD = false;
	public static bool RIGHT_HELD = false;
	public static bool SELECT_HELD = false;
	public static bool EXAMINE_HELD = false;
	public static bool CANCEL_HELD = false;
	public static bool TTS_HELD = false;

    public static bool FORWARD_DOWN = false;
    public static bool BACKWARD_DOWN = false;
    public static bool LEFT_DOWN = false;
    public static bool RIGHT_DOWN = false;
    public static bool SELECT_DOWN = false;
    public static bool EXAMINE_DOWN = false;
    public static bool CANCEL_DOWN = false;
    public static bool TTS_DOWN = false;

    public static bool FORWARD_UP = false;
    public static bool BACKWARD_UP = false;
    public static bool LEFT_UP = false;
    public static bool RIGHT_UP = false;
    public static bool SELECT_UP = false;
    public static bool EXAMINE_UP = false;
    public static bool CANCEL_UP = false;
    public static bool TTS_UP = false;

    public static bool MOVE_HELD = false;
    public static bool MOVE_DOWN = false;
    public static bool MOVE_UP = false;

    public static void CaptureInput()
	{
        FORWARD_DOWN = Input.IsActionJustPressed(UP_STR);
        BACKWARD_DOWN = Input.IsActionJustPressed(DOWN_STR);
        LEFT_DOWN = Input.IsActionJustPressed(LEFT_STR);
        RIGHT_DOWN = Input.IsActionJustPressed(RIGHT_STR);
        SELECT_DOWN = Input.IsActionJustPressed(SELECT_STR);
        EXAMINE_DOWN = Input.IsActionJustPressed(EXAMINE_STR);
        CANCEL_DOWN = Input.IsActionJustPressed(CANCEL_STR);
        TTS_DOWN = Input.IsActionJustPressed(TTS_SPEAK_STR);

		MOVE_DOWN = FORWARD_DOWN || BACKWARD_DOWN || LEFT_DOWN || RIGHT_DOWN;

        FORWARD_HELD = Input.IsActionPressed(UP_STR);
		BACKWARD_HELD = Input.IsActionPressed(DOWN_STR);
		LEFT_HELD = Input.IsActionPressed(LEFT_STR);
		RIGHT_HELD = Input.IsActionPressed(RIGHT_STR);
		SELECT_HELD = Input.IsActionPressed(SELECT_STR);
		EXAMINE_HELD = Input.IsActionPressed(EXAMINE_STR);
		CANCEL_HELD = Input.IsActionPressed(CANCEL_STR);
		TTS_HELD = Input.IsActionPressed(TTS_SPEAK_STR);

		MOVE_HELD = FORWARD_HELD || BACKWARD_HELD || LEFT_HELD || RIGHT_HELD;

        FORWARD_UP = Input.IsActionJustReleased(UP_STR);
		BACKWARD_UP = Input.IsActionJustReleased(DOWN_STR);
		LEFT_UP = Input.IsActionJustReleased(LEFT_STR);
		RIGHT_UP = Input.IsActionJustReleased(RIGHT_STR);
		SELECT_UP = Input.IsActionJustReleased(SELECT_STR);
		EXAMINE_UP = Input.IsActionJustReleased(EXAMINE_STR);
		CANCEL_UP = Input.IsActionJustReleased(CANCEL_STR);
		TTS_UP = Input.IsActionJustReleased(TTS_SPEAK_STR);

		MOVE_UP = FORWARD_UP || BACKWARD_UP || LEFT_UP || RIGHT_UP;
    }
}
