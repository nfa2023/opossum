using Godot;
using System;
using System.IO;

public static class SaveLoad
{
    public static readonly string FILE_EXT = ".pssm";
    public static readonly string OPTS_FILE = string.Concat("options", FILE_EXT);

    public static readonly string BASE_PATH = ProjectSettings.GlobalizePath("user://");
    public static readonly string OPTIONS_PATH = Path.Combine(BASE_PATH, "meta", OPTS_FILE);

    public static SerialGame GameData = new SerialGame();
    public static SerialOptions OptionsData = new SerialOptions();

    public static void SaveOptions()
    {
        
    }
}
