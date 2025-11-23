using UnityEngine;

public class Hoplite
{
    private int _id;
    private PlayerColor _color;
    private bool _moved;
    private bool _onBoard;
    private RegionId _regionId;

    public int Id => _id;
    public PlayerColor Color => _color;
    public bool OnBoard => _onBoard;
    public RegionId RegionId => _regionId;

    public Hoplite(int id)
    {
        _id = id;
    }

    public void PlaceOnBoard(RegionId regionId)
    {
        _regionId = regionId;
        _onBoard = true;
        _moved = false;
    }
}
