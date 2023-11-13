using Godot;
using Godot.Collections;

public partial class ctrlUi : Node
{
    [Export] public AspectRatioContainer CtrlOpts;
    [Export] public ColorRect InputKeyRect;
    [Export] public TextureButton CtrlOptsBtn;
    [Export] public TextureButton ResetVidBtn;
    [Export] public TextureButton Ret2OptsBtn;
    [Export] public VBoxContainer CtrlsContainer;

    public TextureButton[] CtrlBtns = new TextureButton[inp.ACT_CT];
    public Label[] CtrlLabels = new Label[inp.ACT_CT];
    public TextureButton CurrCtrlBtn;
    public Label CurrCtrlLabel;

    public bool pollingInput = false;
    public bool acceptingInput = false;

    public void StopInputPoll() { pollingInput = false; }

    public void ShowCtrls()
    {
        mgt.opts.OptsMenu.Visible = false;
        CtrlOpts.Visible = true;
    }

    public void HideCtrls()
    {
        mgt.opts.OptsMenu.Visible = true;
        CtrlOpts.Visible = false;
    }

    public void Reset()
    {

    }

    public void Init()
    {
        bool SavedOptsFound = false;
        if (SavedOptsFound) { }
        else { Reset(); }
    }

    public void AddActBtn(int pressedBtnIdx)
    {
        if (pollingInput) { return; }

        CurrCtrlBtn = CtrlBtns[pressedBtnIdx];
        CurrCtrlLabel = CtrlLabels[pressedBtnIdx];

        acceptingInput = true;
        pollingInput = true;
        InputKeyRect.Visible = true;
    }

    public override void _Ready()
	{
        CtrlOpts.Visible = false;
        InputKeyRect.Visible = false;
        CtrlOptsBtn.ButtonUp += ShowCtrls;
        Ret2OptsBtn.ButtonUp += HideCtrls;
        ResetVidBtn.ButtonUp += Reset;

        Array<Node> n = CtrlsContainer.GetChildren();
        for(int i = 0; i < inp.ACT_CT; ++i)
        {
            CtrlBtns[i] = (TextureButton)n[i].GetChild(1);
            CtrlLabels[i] = (Label)CtrlBtns[i].GetChild(0);
        }

        // not sure if there is a more efficient way to do this
        CtrlBtns[0].ButtonUp += () => AddActBtn(0);
        CtrlBtns[1].ButtonUp += () => AddActBtn(1);
        CtrlBtns[2].ButtonUp += () => AddActBtn(2);
        CtrlBtns[3].ButtonUp += () => AddActBtn(3);
        CtrlBtns[4].ButtonUp += () => AddActBtn(4);
        CtrlBtns[5].ButtonUp += () => AddActBtn(5);
        CtrlBtns[6].ButtonUp += () => AddActBtn(6);
        CtrlBtns[7].ButtonUp += () => AddActBtn(7);

        Ftmr.Start(1, Init);
    }

    public override void _Input(InputEvent @event)
    {
        if (pollingInput == false) { return; }

        if (acceptingInput && @event.IsPressed())
        {
            CurrCtrlLabel.Text = @event.AsText();
            CurrCtrlBtn = null;
            CurrCtrlLabel = null;

            InputKeyRect.Visible = false;
            acceptingInput = false;
            Tmr.Start(0.33f, StopInputPoll);
        }
    }
}
