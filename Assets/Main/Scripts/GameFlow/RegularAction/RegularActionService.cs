using System;

public class RegularActionService
{
    private readonly RegularActionCtlr _controller;

    public RegularActionService(RegularActionCtlr controller) => _controller = controller;

    public void Launch(RegularAction regularAction, Action onComplete)
    {
        _controller.StartRegularAction(regularAction, onComplete);
    }
}
