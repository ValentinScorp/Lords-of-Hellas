using System;

public class ActionControlPanelController
{
    private readonly ActionControlPanel _panel;
    private IRegularAction _regularAction;


    public ActionControlPanelController(ActionControlPanel panel)
    {
        _panel = panel ?? throw new ArgumentNullException(nameof(panel));
        _panel.Show(false);
    }
    public void Start(IRegularAction regularAction)
    {
        _regularAction = regularAction;
        _panel.Show(true);        
    }
    public void OnCancel()
    {
        _panel.Show(false);
        _regularAction?.Cancel();
        _regularAction = null;
    }
    public void OnUndo()
    {
        _regularAction?.Undo();
    }
    public void OnDone()
    {
        _panel.Show(false);
        _regularAction?.Done();
        _regularAction = null;
    }    
}
