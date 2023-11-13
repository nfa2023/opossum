using Godot;
using System.Runtime.CompilerServices;

public static class shdr
{
    public const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;

    public static readonly string FONT = "Label/fonts/font";
    public static readonly string FONT_SIZE = "Label/font_sizes/font_size";
    public static readonly string FONT_COLOR = "Label/colors/font_color";
    public static readonly string OUTLINE_SIZE = "Label/constants/outline_size";
    public static readonly string OUTLINE_COLOR = "Label/colors/font_outline_color";
    public static readonly string OUTLINE_SHADOW_SIZE = "Label/constants/shadow_outline_size";
    public static readonly string OUTLINE_SHADOW_COLOR = "Label/colors/font_shadow_color";
    
    public static readonly string SELF_MOD = "self_modulate";

    #region COLORS

    public static readonly Color BLACK =    Color.Color8(0, 0, 0);
    public static readonly Color WHITE =    Color.Color8(255, 255, 255);
    
    public static readonly Color RED =      Color.Color8(255, 0, 0);
    public static readonly Color GREEN =    Color.Color8(0, 255, 0);
    public static readonly Color BLUE =     Color.Color8(0, 0, 255);

    public static readonly Color CYAN =     Color.Color8(0, 255, 255);
    public static readonly Color MAGENTA =  Color.Color8(255, 0, 255);
    public static readonly Color YELLOW =   Color.Color8(255, 255, 0);

    [MethodImpl(INLINE)]
    public static Color Blk(byte opacity) 
    { 
        return Color.Color8(0, 0, 0, opacity); 
    }

    [MethodImpl(INLINE)]
    public static Color Wht(byte opacity) 
    { 
        return Color.Color8(255, 255, 255, opacity); 
    }

    [MethodImpl(INLINE)]
    public static Color Red(byte opacity) 
    { 
        return Color.Color8(255, 0, 0, opacity); 
    }

    [MethodImpl(INLINE)]
    public static Color Grn(byte opacity)
    {
        return Color.Color8(0, 255, 0, opacity);
    }

    [MethodImpl(INLINE)]
    public static Color Blue(byte opacity)
    {
        return Color.Color8(0, 0, 255, opacity);
    }

    [MethodImpl(INLINE)]
    public static Color Cyan(byte opacity)
    {
        return Color.Color8(0, 255, 255, opacity);
    }

    [MethodImpl(INLINE)]
    public static Color Mag(byte opacity)
    {
        return Color.Color8(255, 0, 255, opacity);
    }

    [MethodImpl(INLINE)]
    public static Color Yellow(byte opacity)
    {
        return Color.Color8(255, 255, 0, opacity);
    }

    #endregion
}
