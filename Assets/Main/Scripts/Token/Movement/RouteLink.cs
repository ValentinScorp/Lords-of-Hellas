using UnityEngine;

public class RouteLink
{
    private LineRenderer _lineRenderer;
    public Vector3 FromPosition => _lineRenderer.GetPosition(0);
    public Vector3 ToPosition => _lineRenderer.GetPosition(1);
    public void Create(Vector3 fromPosition, Vector3 toPosition, PlayerColor playerColor)
    {
        if (_lineRenderer != null) return;
        
        GameObject lineObject = new GameObject("RouteLinkLine");
        lineObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        lineObject.transform.position += new Vector3(0f, 0.1f, 0f);
        _lineRenderer = lineObject.AddComponent<LineRenderer>();
        _lineRenderer.alignment = LineAlignment.TransformZ;
        _lineRenderer.material = GameData.GlobalMaterials.routeLine;
        _lineRenderer.material.color = GameData.PlayerColorPalette.GetColor(playerColor);
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;

        var segments = 16;
        var heightFactor = Vector3.Distance(fromPosition, toPosition) / 1f;

        _lineRenderer.positionCount = segments + 1;

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 mid = Vector3.Lerp(fromPosition, toPosition, 0.5f) + heightFactor * Vector3.up;
            Vector3 p0 = Vector3.Lerp(fromPosition, mid, t);
            Vector3 p1 = Vector3.Lerp(mid, toPosition, t);
            _lineRenderer.SetPosition(i, Vector3.Lerp(p0, p1, t));
        }
    }
}
