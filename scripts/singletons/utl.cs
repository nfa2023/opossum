using System.Runtime.CompilerServices;
using Godot;

public partial class Utl : Node
{
    public const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;

    public static Vector2I WIN; // The OS window size of the game window. Does NOT correspond to game units.
    public static Vector2I CNTR_WIN;
    public static Vector2I GAME_WIN; // The size of the drawn game window. Does NOT correspond with game units.
    public static Vector2 VP_SZ; // The viewport's size. Correspond with game units.
    public static Vector2 INV_VP_SZ; // Inverse of viewport size.
    public static float dtScale = 1f;
    public static float uDt = 0f;       // (float)_Process(double delta)
    public static float invUdt = 0f;    // 1f / uDt
    public static float dt = 0f;        // uDt * dtScale * accessibility time scale
    public static float invDt = 0f;     // 1f / dt
    public static float uFdt = 0f;       // (float)_PhysicsProcess(double delta)
    public static float invUfdt = 0f;    // 1f / uFdt
    public static float fdt = 0f;       // uFdt * dtScale * accessibility time scale
    public static float invFdt = 0f;    // 1f / fdt

    public static bool PAUSED = false;
    public static bool PLR_PAUSED = false;

    public static Viewport VP;

    public static readonly string EN_STR = "en";

    public static readonly string PCT_FRMT = "{0}%";
    public static readonly string FIFTY = "50";
    public static readonly string TEN = "10";
    public static readonly string _75_PERCENT = "75%";
    public static readonly string _100_PERCENT = "100%";
    public static readonly string _150_PERCENT = "150%";

    public static string i2sPercent(int number)
    {
        if(number == 150) { return _150_PERCENT; }
        if(number == 100) { return _100_PERCENT; }
        if(number == 75) { return _75_PERCENT; }
        return string.Format(PCT_FRMT, Str.i2s(number));
    }

    [MethodImpl(INLINE)]
    public static Vector2 NORM2VP(in Vector2 v) { return v * INV_VP_SZ; }

    [MethodImpl(INLINE)]
    public static void UpdateGameWin()
    {
        Vector2 win = new(WIN.X, WIN.Y);
        Vector2 gameWin = new(GAME_WIN.X, GAME_WIN.Y);

        Vector2 scaledWin;

        if(win.Aspect() > gameWin.Aspect())
        {
            float scaledX = win.Y / gameWin.Y;
            scaledWin.X = gameWin.X * scaledX;

            scaledWin.Y = win.Y;
        }
        else
        {
            scaledWin.X = win.X;
            float scaledY = win.X / gameWin.X;

            scaledWin.Y = gameWin.Y * scaledY;
        }

        GAME_WIN = (Vector2I)scaledWin;
    }

    [MethodImpl(INLINE)]
    public static void RefreshViewportSz()
    {
        VP_SZ = VP.GetVisibleRect().Size;
        INV_VP_SZ = new Vector2(1f / VP_SZ.X, 1f / VP_SZ.Y);
    }

    public override void _Ready()
	{
        GAME_WIN = new Vector2I(1280, 720);

        VP = GetViewport();
        VP_SZ = VP.GetVisibleRect().Size;
        INV_VP_SZ = new Vector2(1f / VP_SZ.X, 1f / VP_SZ.Y);
        VP.SizeChanged += RefreshViewportSz;

        WIN = DisplayServer.WindowGetSize();
        CNTR_WIN = new Vector2I(WIN.X >> 1, WIN.Y >> 1);
        UpdateGameWin();
    }

	public override void _Process(double delta)
	{
        Vector2I currWinSz = DisplayServer.WindowGetSize();
        if (currWinSz != WIN)
        {
            WIN = DisplayServer.WindowGetSize();
            CNTR_WIN = new Vector2I(WIN.X >> 1, WIN.Y >> 1);
            UpdateGameWin();
        }

        uDt = (float)delta;
        invUdt = 1F / uDt;

        dt = uDt * dtScale * mgt.acc.timeScale;
        invDt = 1F / dt;

        CDtwn.Eval(uDt);
        Twn.Eval(dt);
        Ftmr.Tick(1);
        Tmr.Tick(dt);

        inp.CaptureInput();
    }

    public override void _PhysicsProcess(double delta)
    {
        uFdt = (float)delta;
        invUfdt = 1F / uFdt;

        fdt = uFdt * dtScale * mgt.acc.timeScale;
        invFdt = 1f / fdt;
    }
}
