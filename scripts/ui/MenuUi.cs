using Godot;
using NFA.Godot;

public partial class MenuUi : Node
{
    [Export] public AspectRatioContainer TitleCard;

    [Export] public TextureButton Continue;
    [Export] public TextureButton New;
    [Export] public TextureButton Quit;
    [Export] public TextureButton Options;

    [Export] public TextureButton Credits;
    [Export] public AspectRatioContainer CredPanel;
    [Export] public TextureButton QuitCredits;

    public bool mainMenu = true;

    public bool canOpenMenu = false;
    public bool menuOpen = false;

    public void HideMenuBtns()
    {
        Continue.Disabled = true;
        Continue.Visible = false;

        New.Disabled = true;
        New.Visible = false;

        Credits.Disabled = true;
        Credits.Visible = false;

        Options.Disabled = true;
        Options.Visible = false;

        Quit.Disabled = true;
        Quit.Visible = false;

        menuOpen = false;
    }

    public void ShowMenuBtns()
    {
        Continue.Disabled = false;
        Continue.Visible = true;

        New.Disabled = false;
        New.Visible = true;

        Credits.Disabled = false;
        Credits.Visible = true;

        Options.Disabled = false;
        Options.Visible = true;

        Quit.Disabled = false;
        Quit.Visible = true;

        menuOpen = true;
    }

    public void ContinueGame()
    {
#if DEBUG

        TitleCard?.QueueFree();

        mainMenu = false;
        canOpenMenu = true;
        menuOpen = false;
        HideMenuBtns();

        bool loaded = Ldr.LoadLevel(this, "uid://2uuiiakyapd5", out PackedScene dbgLvl);
        if (loaded == false) { GD.PrintErr("Unable to load debug level..."); }

#endif
    }

    public void NewGame()
    {
#if DEBUG
        TitleCard.QueueFree();
        mainMenu = false;
#endif
        GD.Print("Start new game.");
    }

    public void ShowCredits()
    {
        HideMenuBtns();
        CredPanel.Visible = true;
    }

    public void HideCredits()
    {
        ShowMenuBtns();
        CredPanel.Visible = false;
    }

    public void QuitGame()
    {
        /*
			Do what needs to be done to quit the game.
		*/
        GetTree().Quit();
    }

    public override void _Ready()
    {
        Continue.ButtonUp += ContinueGame;
        New.ButtonUp += NewGame;
        Quit.ButtonUp += QuitGame;

        Credits.ButtonUp += ShowCredits;
        QuitCredits.ButtonUp += HideCredits;

        CredPanel.Visible = false;
    }

    public override void _Process(double delta)
    {
        if(mainMenu) { return; }

        if(canOpenMenu && inp.CANCEL_DOWN)
        {
            if (menuOpen) { HideMenuBtns(); }
            else { ShowMenuBtns(); }
        }
    }
}