using Godot;

public partial class menu : Node
{
    /*
    [Export] public Button contBtn;
    [Export] public Button newBtn;
    [Export] public Button quitBtn;
    [Export] public Button optsBtn;

    [Export] public Button credBtn;
    [Export] public AspectRatioContainer credPanel;
    [Export] public Button closeCredBtn;
    */

    [Export] public TextureButton contBtn;
    [Export] public TextureButton newBtn;
    [Export] public TextureButton quitBtn;
    [Export] public TextureButton optsBtn;

    [Export] public TextureButton credBtn;
    [Export] public AspectRatioContainer credPanel;
    [Export] public TextureButton closeCredBtn;

    public void HideMenuBtns()
    {
        contBtn.Disabled = true;
        contBtn.Visible = false;

        newBtn.Disabled = true;
        newBtn.Visible = false;

        credBtn.Disabled = true;
        credBtn.Visible = false;

        optsBtn.Disabled = true;
        optsBtn.Visible = false;

        quitBtn.Disabled = true;
        quitBtn.Visible = false;
    }

    public void ShowMenuBtns()
    {
        contBtn.Disabled = false;
        contBtn.Visible = true;

        newBtn.Disabled = false;
        newBtn.Visible = true;

        credBtn.Disabled = false;
        credBtn.Visible = true;

        optsBtn.Disabled = false;
        optsBtn.Visible = true;

        quitBtn.Disabled = false;
        quitBtn.Visible = true;
    }

    public void ContinueGame()
    {
        GD.Print("Continue game.");
    }

    public void NewGame()
    {
        GD.Print("Start new game.");
    }

    public void ShowCredits()
    {
        HideMenuBtns();
        credPanel.Visible = true;
    }

    public void HideCredits()
    {
        ShowMenuBtns();
        credPanel.Visible = false;
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
        contBtn.ButtonUp += ContinueGame;
        newBtn.ButtonUp += NewGame;
        quitBtn.ButtonUp += QuitGame;

        credBtn.ButtonUp += ShowCredits;
        closeCredBtn.ButtonUp += HideCredits;

        credPanel.Visible = false;
    }
}