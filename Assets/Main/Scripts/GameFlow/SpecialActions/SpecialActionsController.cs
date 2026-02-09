using System;

public class SpecialActionsController
{
    private Player _player;
    private Action<Player> _Completed;
    
    public void Launch(Player player, Action<Player> Completed)
    {
        _player = player;
        _Completed = Completed;
    }
}
