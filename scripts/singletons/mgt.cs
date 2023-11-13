using Godot;
using System;

public partial class mgt : Node
{
    public static gme gme;
    public static Utl utl;
    public static Audio snd;
    public static acc acc;
    public static opts opts;
    public static ui ui;

    public static void InitGameMgt(in Node gmeNode)
	{
        if (gmeNode == null)
        {
            GD.PrintErr("MGT COULD NOT GET THE GAME CONTROLLER NODE!");
            return;
        }

        gme = (gme)gmeNode;
        if(gme == null)
        {
            GD.PrintErr("MGT COULD NOT GET THE GAME CONTROLLER SCRIPT!");
            return;
        }

        utl = gme.utl;
        snd = gme.snd;
        acc = gme.acc;
        opts = gme.opts;
        ui = gme.ui;
	}
}
