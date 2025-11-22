using UnityEngine;

public class RegularActionManager : PureEventListener<RegularAction>
{
    private RegularAction _regularAction;
    protected override void HandleEvent(RegularAction evt) {
        Debug.Log($"[LOG] Regular action");
    }

    public void Start(Player player) {
        _regularAction = new RegularAction(player);
        EventBus.SendEvent(_regularAction);
    }
}
