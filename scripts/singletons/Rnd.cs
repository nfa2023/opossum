using System;

public static class Rnd
{
    // Explanation: 
    // https://lemire.me/blog/2019/07/03/a-fast-16-bit-random-number-generator/
    private static uint seed = 37; // decent results from the prime 37
    private const uint SEED_INCREMENT = 0xfc15; // 64533
    private static ushort _16()
    {
        seed += SEED_INCREMENT;
        uint r = seed * 0x2ab; // seed * 683
        return (ushort)((r >> 16) ^ r);
    }
}
