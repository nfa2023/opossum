using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public enum EaseFunction : byte
{
    QuadOut = 0,
    QuadIn,
    QuadInOut,

    CubicOut,
    CubicIn,
    CubicInOut,

    QuartOut,
    QuartIn,
    QuartInOut,

    QuintOut,
    QuintIn,
    QuintInOut,

    PowOut,
    PowIn,
    PowInOut,

    CircleOut,
    CircleIn,
    CircleInOut,

    BackOut,
    BackIn,
    BackInOut,

    ElasticOut,
    ElasticIn,
    ElasticInOut,

    BounceOut,
    BounceIn,
    BounceInOut,

    SinOut,
    SinIn,
    SinInOut,

    SmoothStep,
    SmootherStep,

    Pulse, // 1 → 0 → 1
    Eslup, // 0 → 1 → 0

    Linear = 254,

    NULL = 255
}

public static class TwnUtl
{
    public const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;
    public const float FLT_EPSILON = 0.0001f;

    // Basic tween structure for tweens that have a constant duration.
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct constDurTwn // 32 bytes in size
    {
        // 16 bytes
        public float dt;
        public float strtVal;
        public float dist;
        public state state;

        // 16 bytes
        public Action onComplete;
        public Action<float> during;
    }

    // Basic tween structure for tweens that have a variable duration.
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct twn // 40 bytes in size
    {
        // 24 bytes
        public float dt;
        public float strtVal;
        public float dist;
        public float dur;
        public float invDur;
        public state state;

        // 16 bytes
        public Action onComplete;
        public Action<float> during;

        [MethodImpl(INLINE)]
        public static void Swap(ref twn a, ref twn b, bool preserveID = false)
        {
            twn tmp = a;
            a = b;
            b = tmp;

            if (preserveID)
            {
                // since we just swapped them, swap them back
                ushort a_ID = b.state.id;
                ushort b_ID = a.state.id;
                a.state.id = a_ID;
                b.state.id = b_ID;
            }
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct state // 4 bytes in size.
    {
        [FieldOffset(0)] public int full;
        [FieldOffset(0)] public bool RUNNING;
        [FieldOffset(1)] public EaseFunction EaseFunction;
        [FieldOffset(2)] public ushort id;
    }
}
