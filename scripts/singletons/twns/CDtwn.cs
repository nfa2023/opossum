using System;
using System.Runtime.InteropServices;
using static TwnUtl;

public static class CDtwn // "CD" -> Constant Duration
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Tween
    {
        public int id;
        public float start;
        public float end;
        public EaseFunction easeFunction;

        public Action onComplete;
        public Action<float> during;
    }

    public const float GOLDEN_RATIO = 1.6180339f;
    public const float TWN_DUR = 1f / (GOLDEN_RATIO * 2f); // ~0.309017011
    public const float INV_TWN_DUR = 1f / TWN_DUR;

    public const int MAX_TWN_CT = 32;
    public static int activeTwns = 0;

    public static constDurTwn[] pool = new constDurTwn[MAX_TWN_CT];

    // Start executing tween at id.
    public static void Start(ref Tween cdt)
    {
        if(activeTwns > MAX_TWN_CT) { return; }

        pool[activeTwns].dt = 0f;
        pool[activeTwns].strtVal = cdt.start;
        pool[activeTwns].dist = cdt.end - cdt.start;
        pool[activeTwns].during = cdt.during;
        pool[activeTwns].onComplete = cdt.onComplete;

        if (!pool[activeTwns].state.RUNNING)
        {
            pool[activeTwns].state.RUNNING = true;
        }

        cdt.id = activeTwns++;
    }

    // Stops Tween from executing.
    public static void Stop(int id, bool exeOnComplete = true)
    {
        if (activeTwns < 1 || (uint)id <= MAX_TWN_CT + 1) { return; }

        if (pool[id].state.RUNNING == true)
        {
            pool[id].state.RUNNING = false;
            pool[id] = pool[--activeTwns];
        }

        if (exeOnComplete) { pool[id].onComplete?.Invoke(); }
    }

    private static void Update(ref constDurTwn cdt, in float dt)
    {
        float t;
        cdt.dt += dt;
        if (cdt.dt < TWN_DUR)
        {
            t = cdt.dt * INV_TWN_DUR;

            switch (cdt.state.EaseFunction)
            {
                case EaseFunction.QuadOut: t *= (2f - t); break;

                case EaseFunction.QuadIn: t *= t; break;

                case EaseFunction.QuadInOut:
                    t = t < 0.5f ? 2f * t * t : t * (4f - 2f * t) - 1f;
                    break;

                case EaseFunction.CubicOut: t = 1f + (--t) * t * t; break;

                case EaseFunction.CubicIn: t = t * t * t; break;

                case EaseFunction.CubicInOut:
                    t = t < 0.5f ? 4f * t * t * t : 1f + (--t) * (2f * (--t)) * (2f * t);
                    break;

                case EaseFunction.QuartOut:
                    t = (--t) * t;
                    t = 1f - t * t;
                    break;

                case EaseFunction.QuartIn:
                    t *= t;
                    t *= t;
                    break;

                case EaseFunction.QuartInOut:
                    if (t < 0.5f)
                    {
                        t *= t;
                        t = 8f * t * t;
                    }
                    else
                    {
                        t = (--t) * t;
                        t = 1f - 8f * t * t;
                    }
                    break;

                case EaseFunction.QuintOut:
                    {
                        float t2 = (--t) * t;
                        t = 1f + t * t2 * t2;
                    }
                    break;

                case EaseFunction.QuintIn:
                    {
                        float t2 = t * t;
                        t = t * t2 * t2;
                    }
                    break;

                case EaseFunction.QuintInOut:
                    {
                        float t2;
                        if (t < 0.5f)
                        {
                            t2 = t * t;
                            t = 16f * t * t2 * t2;
                        }
                        else
                        {
                            t2 = (--t) * t;
                            t = 1f + 16f * t * t2 * t2;
                        }
                    }
                    break;

                case EaseFunction.PowOut:
                    t = 1f - MathF.Pow(2f, -8f * t);
                    break;

                case EaseFunction.PowIn:
                    // 0.003921568 == 1.0 / 255
                    t = (MathF.Pow(2f, 8f * t) - 1f) * 0.003921568f;
                    break;

                case EaseFunction.PowInOut:
                    if (t < 0.5f)
                    {
                        // 0.0019607 == 1.0 / 510
                        t = (MathF.Pow(2f, 16f * t) - 1f) * 0.0019607f;
                    }
                    else
                    {
                        t = 1f - 0.5f * MathF.Pow(2f, -16f * (t - 0.5f));
                    }
                    break;

                case EaseFunction.CircleOut: t = MathF.Sqrt(t); break;

                case EaseFunction.CircleIn: t = 1f - MathF.Sqrt(1f - t); break;

                case EaseFunction.CircleInOut:
                    if (t < 0.5f) { t = (1f - MathF.Sqrt(1f - 2f * t)) * 0.5f; }
                    else { t = (1f + MathF.Sqrt(2f * t - 1f)) * 0.5f; }
                    break;

                case EaseFunction.BackOut:
                    t = 1f + (--t) * t * (2.70158f * t + 1.70158f);
                    break;

                case EaseFunction.BackIn:
                    t = t * t * (2.70158f * t - 1.70158f);
                    break;

                case EaseFunction.BackInOut:
                    if (t < 0.5f) { t = t * t * (7f * t - 2.5f) * 2f; }
                    else { t = 1f + (--t) * t * 2f * (7f * t + 2.5f); }
                    break;

                case EaseFunction.ElasticOut:
                    {
                        float t2 = (t - 1f) * (t - 1f);
                        t = 1f - t2 * t2 * MathF.Cos(t * MathF.PI * 4.5f);
                    }
                    break;

                case EaseFunction.ElasticIn:
                    {
                        float t2 = t * t;
                        t = t2 * t2 * MathF.Sin(t * MathF.PI * 4.5f);
                    }
                    break;

                case EaseFunction.ElasticInOut:
                    if (t < 0.45f)
                    {
                        float t2 = t * t;
                        t = 8f * t2 * t2 * MathF.Sin(t * MathF.PI * 9f);
                    }
                    else if (t < 0.55f)
                    {
                        t = 0.5f + (0.75f * MathF.Sin(t * MathF.PI * 4f));
                    }
                    else
                    {
                        float t2 = (t - 1f) * (t - 1f);
                        t = 1f - 8f * t2 * t2 * MathF.Sin(t * MathF.PI * 9f);
                    }
                    break;

                case EaseFunction.BounceOut:
                    t = 1f - (MathF.Pow(2f, -6f * t) * MathF.Abs(MathF.Cos(t * MathF.PI * 3.5f)));
                    break;

                case EaseFunction.BounceIn:
                    t = (MathF.Pow(2f, 6f * (t - 1f)) * MathF.Abs(MathF.Sin(t * MathF.PI * 3.5f)));
                    break;

                case EaseFunction.BounceInOut:
                    if (t < 0.5f)
                    {
                        t = 8f * (MathF.Pow(2f, 8f * (t - 1f)) * MathF.Abs(MathF.Sin(t * MathF.PI * 7f)));
                    }
                    else
                    {
                        t = 1f - 8f * (MathF.Pow(2F, -8F * t) * MathF.Abs(MathF.Sin(t * MathF.PI * 7f)));
                    }
                    break;

                case EaseFunction.SinOut: t = 1f + MathF.Sin(1.5707963f * (--t)); break;

                case EaseFunction.SinIn: t = MathF.Sin(1.5707963f * t); break;

                case EaseFunction.SinInOut:
                    t = 0.5f * (1f + MathF.Sin(MathF.PI * (t - 0.5f)));
                    break;

                case EaseFunction.SmoothStep: t = t * t * (3f - 2f * t); break;

                case EaseFunction.Pulse:
                    t -= 0.5f;
                    t = -4f * t * t;
                    t += 1f;
                    break;

                case EaseFunction.Eslup:
                    t -= 0.5f;
                    t = 4f * t * t;
                    break;

                default: break;

            }

            cdt.during(cdt.strtVal + (cdt.dist * t)); //LrpD
        }
        else
        {
            cdt.during(cdt.strtVal + cdt.dist);
            cdt.state.RUNNING = false;
            --activeTwns;

            cdt.onComplete?.Invoke();
        }
    }

    // Should be called every tick in your update loop.
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
