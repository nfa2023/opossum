using System.Runtime.CompilerServices;
using Godot;

public partial class utl : Node
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

    public static readonly string _50 = "50";
    public static readonly string _100_PCT = "100%";
    public static readonly string _10 = "10";

    public const int CLR_MIN = 0;   public const int CLR_MAX = 8;
    public const int BLACK = 0;     public const int WHITE = 1;
    public const int RED = 2;       public const int GREEN = 3;
    public const int BLUE = 4;      public const int CYAN = 5;
    public const int MAGENTA = 6;   public const int YELLOW = 7;
    public static readonly Color[] Clrs = new Color[CLR_MAX] 
    { 
        shdr.BLACK,     shdr.WHITE,     shdr.RED,       shdr.GREEN, 
        shdr.BLUE,      shdr.CYAN,      shdr.MAGENTA,   shdr.YELLOW 
    };

    public static string i2sPercent(int number)
    {
        if(number == 150) { return "150%"; }
        if(number == 100) { return "100%"; }
        if(number == 75) { return "75%"; }
        return string.Concat(str.i2s(number), '%');
    }

    [MethodImpl(INLINE)]
    public float NORM2VP_X(float x) { return x * INV_VP_SZ.X; }

    [MethodImpl(INLINE)]
    public float NORM2VP_Y(float y) { return y * INV_VP_SZ.Y; }

    [MethodImpl(INLINE)]
    public Vector2 NORM2VP(in Vector2 v) { return v * INV_VP_SZ; }

    [MethodImpl(INLINE)]
    public static void UpdateGameWin()
    {
        float winAspect = WIN.X / WIN.Y;
        float gmeAspect = GAME_WIN.X / GAME_WIN.Y;

        Vector2 scaledWin;
        if(winAspect > gmeAspect)
        {
            float scaledX = (float)WIN.Y / (float)GAME_WIN.Y;
            scaledWin.X = GAME_WIN.X * scaledX;
            scaledWin.Y = WIN.Y;
        }
        else
        {
            scaledWin.X = WIN.X;
            float scaledY = (float)WIN.X / (float)GAME_WIN.X;
            scaledWin.Y = GAME_WIN.Y * scaledY;
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
	}

    public override void _PhysicsProcess(double delta)
    {
        uFdt = (float)delta;
        invUfdt = 1F / uFdt;

        fdt = uFdt * dtScale * mgt.acc.timeScale;
        invFdt = 1f / fdt;
    }

    public override void _Input(InputEvent @event)
    {
        inp.CaptureInput(@event);

        if (inp.UP_PRESSED) { GD.Print("Up pressed."); }
    }
}
