using System;
using System.Runtime.CompilerServices;

public static class mth
{
    public const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;

    public const float FLT_EP = 0.001f;
    public const float FLT_EP_SQRD = FLT_EP * FLT_EP;

    [MethodImpl(INLINE)]
    public static int Cap(int x, int max) { return x > max ? max : x; }

    [MethodImpl(INLINE)]
    public static int Plug(int x, int min) { return x < min ? min : x; }

    [MethodImpl(INLINE)]
    public static int Clamp(int x, int min, int max)
    {
        return x < min ? min : (x > max ? max : x);
    }

    [MethodImpl(INLINE)]
    public static float Db2Ln(ref float x) { return MathF.Pow(10f, (x * 0.05f)); }

    [MethodImpl(INLINE)]
    public static float Ln2Db(ref float x)
    {
        if (x > FLT_EP) { return 20f * MathF.Log10(x); }
        return -80f;
    }
}
