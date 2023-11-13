using Godot;
using System;

public partial class gme : Node
{
    public utl utl;
    public snd snd;
    public acc acc;
    public opts opts;
    public ui ui;

    public const int UTL_NODE_IDX = 0;
    public const int SND_NODE_IDX = 1;
    public const int ACC_NODE_IDX = 2;
    public const int OPTS_NODE_IDX = 3;
    public const int UI_NODE_IDX = 4;

    public override void _Ready()
    {
        utl = (utl)GetChild(UTL_NODE_IDX);
        snd = (snd)GetChild(SND_NODE_IDX);
        acc = (acc)GetChild(ACC_NODE_IDX);
        opts = (opts)GetChild(OPTS_NODE_IDX);
        ui = (ui)GetChild(UI_NODE_IDX);

        mgt.InitGameMgt(this);
    }
}
