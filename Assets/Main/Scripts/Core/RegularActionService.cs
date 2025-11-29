using System;

public class RegularActionService
{
    private readonly RegularActionController _controller;

    public RegularActionService(RegularActionController controller) => _controller = controller;

    public void Launch(RegularAction regularAction, Action onComplete)
    {
        _controller.StartRegularAction(regularAction, onComplete);
    }
}
