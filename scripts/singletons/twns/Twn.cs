using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static TwnUtl;

public static class Twn
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Tween
    {
        public int id = -1;
        public float duration;
        public float start;
        public float end;
        public EaseFunction easeFunction;

        public Action onComplete;
        public Action<float> during;

        public Tween()
        {
            id = -1;
            duration = 0f;
            start = 0f;
            end = 0f;
            easeFunction = EaseFunction.NULL;
            onComplete = null;
            during = null;
        }
    }

    public const int MAX_TWN_CT = 32;
    public static int activeTwns = 0;

    public static twn[] pool = new twn[MAX_TWN_CT];

    [MethodImpl(INLINE)]
    private static void Update(ref twn t, in float dt)
    {
        float v;

        t.dt += dt;
        if (t.dt < t.dur)
        {
            v = t.dt * t.invDur;

            switch (t.state.EaseFunction)
            {
                case EaseFunction.QuadOut: v *= (2f - v); break;

                case EaseFunction.QuadIn: v *= v; break;

                case EaseFunction.QuadInOut:
                    v = v < 0.5f ? 2f * v * v : v * (4f - 2f * v) - 1f;
                    break;

                case EaseFunction.CubicOut: v = 1f + (--v) * v * v; break;

                case EaseFunction.CubicIn: v = v * v * v; break;

                case EaseFunction.CubicInOut:
                    v = v < 0.5f ? 4f * v * v * v : 1f + (--v) * (2f * (--v)) * (2f * v);
                    break;

                case EaseFunction.QuartOut:
                    v = (--v) * v;
                    v = 1f - v * v;
                    break;

                case EaseFunction.QuartIn:
                    v *= v;
                    v *= v;
                    break;

                case EaseFunction.QuartInOut:
                    if (v < 0.5f)
                    {
                        v *= v;
                        v = 8f * v * v;
                    }
                    else
                    {
                        v = (--v) * v;
                        v = 1f - 8f * v * v;
                    }
                    break;

                case EaseFunction.QuintOut:
                    {
                        float v2 = (--v) * v;
                        v = 1f + v * v2 * v2;
                    }
                    break;

                case EaseFunction.QuintIn:
                    {
                        float v2 = v * v;
                        v = v * v2 * v2;
                    }
                    break;

                case EaseFunction.QuintInOut:
                    {
                        float v2;
                        if (v < 0.5f)
                        {
                            v2 = v * v;
                            v = 16f * v * v2 * v2;
                        }
                        else
                        {
                            v2 = (--v) * v;
                            v = 1f + 16f * v * v2 * v2;
                        }
                    }
                    break;

                case EaseFunction.PowOut: v = 1f - MathF.Pow(2f, -8f * v); break;

                case EaseFunction.PowIn:
                    // 0.003921568 == 1.0 / 255
                    v = (MathF.Pow(2f, 8f * v) - 1f) * 0.003921568f;
                    break;

                case EaseFunction.PowInOut:
                    if (v < 0.5f)
                    {
                        // 0.0019607 == 1.0 / 510
                        v = (MathF.Pow(2f, 16f * v) - 1f) * 0.0019607f;
                    }
                    else
                    {
                        v = 1f - 0.5f * MathF.Pow(2f, -16f * (v - 0.5f));
                    }
                    break;

                case EaseFunction.CircleOut: v = MathF.Sqrt(v); break;

                case EaseFunction.CircleIn: v = 1f - MathF.Sqrt(1f - v); break;

                case EaseFunction.CircleInOut:
                    if (v < 0.5f)
                    {
                        v = (1f - MathF.Sqrt(1f - 2f * v)) * 0.5f;
                    }
                    else
                    {
                        v = (1f + MathF.Sqrt(2f * v - 1f)) * 0.5f;
                    }
                    break;

                case EaseFunction.BackOut:
                    v = 1f + (--v) * v * (2.70158f * v + 1.70158f);
                    break;

                case EaseFunction.BackIn: v = v * v * (2.70158f * v - 1.70158f); break;

                case EaseFunction.BackInOut:
                    if (v < 0.5f)
                    {
                        v = v * v * (7f * v - 2.5f) * 2f;
                    }
                    else
                    {
                        v = 1f + (--v) * v * 2f * (7f * v + 2.5f);
                    }
                    break;

                case EaseFunction.ElasticOut:
                    {
                        float v2 = (v - 1f) * (v - 1f);
                        v = 1f - v2 * v2 * MathF.Cos(v * MathF.PI * 4.5f);
                    }
                    break;

                case EaseFunction.ElasticIn:
                    {
                        float v2 = v * v;
                        v = v2 * v2 * MathF.Sin(v * MathF.PI * 4.5f);
                    }
                    break;

                case EaseFunction.ElasticInOut:
                    if (v < 0.45f)
                    {
                        float v2 = v * v;
                        v = 8f * v2 * v2 * MathF.Sin(v * MathF.PI * 9f);
                    }
                    else if (v < 0.55f)
                    {
                        v = 0.5f + (0.75f * MathF.Sin(v * MathF.PI * 4f));
                    }
                    else
                    {
                        float v2 = (v - 1f) * (v - 1f);
                        v = 1f - 8f * v2 * v2 * MathF.Sin(v * MathF.PI * 9f);
                    }
                    break;

                case EaseFunction.BounceOut:
                    v = 1f - (MathF.Pow(2f, -6f * v) * MathF.Abs(MathF.Cos(v * MathF.PI * 3.5f)));
                    break;

                case EaseFunction.BounceIn:
                    v = (MathF.Pow(2f, 6f * (v - 1f)) * MathF.Abs(MathF.Sin(v * MathF.PI * 3.5f)));
                    break;

                case EaseFunction.BounceInOut:
                    if (v < 0.5f)
                    {
                        v = 8f * (MathF.Pow(2f, 8f * (v - 1f)) * MathF.Abs(MathF.Sin(v * MathF.PI * 7f)));
                    }
                    else
                    {
                        v = 1f - 8f * (MathF.Pow(2f, -8f * v) * MathF.Abs(MathF.Sin(v * MathF.PI * 7f)));
                    }
                    break;

                case EaseFunction.SinOut: v = 1f + MathF.Sin(1.5707963f * (--v)); break;

                case EaseFunction.SinIn: v = MathF.Sin(1.5707963f * v); break;

                case EaseFunction.SinInOut:
                    v = 0.5f * (1f + MathF.Sin(MathF.PI * (v - 0.5f)));
                    break;

                case EaseFunction.SmoothStep: v = v * v * (3f - 2f * v); break;

                case EaseFunction.SmootherStep: v = v * v * v * (v * (6f * v - 15f) + 10f); break;

                case EaseFunction.Pulse:
                    v -= 0.5f;
                    v = -4f * v * v;
                    v += 1f;
                    break;

                case EaseFunction.Eslup:
                    v -= 0.5f;
                    v = 4f * v * v;
                    break;

                default: break;
            }

            t.during(t.strtVal + (t.dist * v)); //LrpD
        }
        else
        {
            t.during(t.strtVal + t.dist);
            t.state.RUNNING = false;
            t.onComplete?.Invoke();
        }
    }

    // Start a tween that will execute once and be freed automatically.
    public static void Start(ref Tween t)
    {
        if (activeTwns >= MAX_TWN_CT) { return; }

        pool[activeTwns].strtVal = t.start;
        pool[activeTwns].dist = t.end - t.start;
        pool[activeTwns].dur = t.duration;
        pool[activeTwns].invDur = 1f / t.duration;
        pool[activeTwns].state.EaseFunction = t.easeFunction;

        pool[activeTwns].during = t.during;
        pool[activeTwns].onComplete = t.onComplete;

        pool[activeTwns].dt = 0f;
        pool[activeTwns].state.RUNNING = true;
        t.id = activeTwns++;
    }

    public static void Stop(int id, bool exeOnComplete = true)
    {
        if (activeTwns < 1 || (uint)id >= MAX_TWN_CT) { return; }

        if (pool[id].state.RUNNING)
        {
            pool[id].state.RUNNING = false;
            pool[id] = pool[--activeTwns];
        }

        if (exeOnComplete) { pool[id].onComplete?.Invoke(); }
    }

    public static void Eval(float dt)
    {
        if (activeTwns < 1 || MathF.Abs(dt) < FLT_EPSILON) { return; }

        for (int i = 0; i < activeTwns; ++i)
        {
            Update(ref pool[i], in dt);

            if (pool[i].state.RUNNING == false)
            {
                pool[i] = pool[--activeTwns];
                --i;
            }
        }
    }
}
