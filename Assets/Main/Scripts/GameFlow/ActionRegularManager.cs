using UnityEngine;

public class ActionRegularManager : PureEventListener<ActionRegular>
{
    private ActionRegular _regularAction;
    protected override void HandleEvent(ActionRegular evt) {
        Debug.Log($"[LOG] Regular action");
    }

    public void Start(Player player) {
        _regularAction = new ActionRegular(player);
        EventBus.SendEvent(_regularAction);
    }
}
