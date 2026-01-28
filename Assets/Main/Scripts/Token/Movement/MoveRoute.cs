using System.Collections.Generic;
using UnityEngine;

public class MoveRoute
{
    private List<RouteArc> _arcs = new();
    private class Node
    {
        public RegionId RegionId { get; private set; }
        public TokenNest TokenNest { get; private set; }        

        public Node(RegionId regionId, TokenNest tokenNest)
        {
            RegionId = regionId;
            TokenNest = tokenNest;
        }
    }
    private List<Node> _nodes = new List<Node>();
    public void AddRouteNode(RegionId regionId, TokenNest nest)
    {
        if (nest is null) Debug.LogWarning("Adding node to route with token nest == null!");
        _nodes.Add(new Node(regionId, nest));
        if (_nodes.Count > 1) {
            var arc = new RouteArc();
            arc.Create(_nodes[_nodes.Count - 2].TokenNest.Position,
                        _nodes[_nodes.Count - 1].TokenNest.Position,
                            PlayerColor.Red);
            _arcs.Add(arc);
        }
    }
    public void Clear()
    {
        _nodes.Clear();
        foreach (var link in _arcs) {
            link.Destroy();
        }
        _arcs.Clear();
    }
}
