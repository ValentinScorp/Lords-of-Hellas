using System.Collections.Generic;
using UnityEngine;

public class MoveRoute
{
    private int _stepsLeft;
    public bool Complete => _stepsLeft <= 0;
    public int StepsLeft => Mathf.Max(0, _stepsLeft - 1);
    private List<RouteLink> _routeLinks = new();
    private class Node
    {
        public RegionId RegionId { get; private set; }
        public TokenNest SpawnPoint { get; private set; }        

        public Node(RegionId regionId, TokenNest spawnPoint)
        {
            RegionId = regionId;
            SpawnPoint = spawnPoint;
        }
    }
    private List<Node> _nodes = new List<Node>();
    public void AddRouteNode(RegionId regionId, TokenNest spawnPoint)
    {
        _stepsLeft--;
        if (_stepsLeft >= 0) {
            _nodes.Add(new Node(regionId, spawnPoint));
            if (_nodes.Count > 1) {
                var routeLink = new RouteLink();
                routeLink.Create(_nodes[_nodes.Count - 2].SpawnPoint.Position,
                                _nodes[_nodes.Count - 1].SpawnPoint.Position,
                                PlayerColor.Red);
                _routeLinks.Add(routeLink);
            }
        }
    }
    public void SetSteps(int steps)
    {
        _stepsLeft = steps + 1;
    }
    public void Clear()
    {
        _nodes.Clear();
        foreach (var link in _routeLinks) {
            link.Destroy();
        }
        _routeLinks.Clear();
    }
}
