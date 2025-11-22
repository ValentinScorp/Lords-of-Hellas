using System.Linq;
using UnityEngine;

public static class LayerUtils
{
    public static int GetMask(params string[] layerNames) {
        return layerNames.Aggregate(0, (mask, name) => mask | (1 << LayerMask.NameToLayer(name)));
    }

    public static int AddLayer(int mask, string layerName) {
        return mask | (1 << LayerMask.NameToLayer(layerName));
    }

    public static int RemoveLayer(int mask, string layerName) {
        return mask & ~(1 << LayerMask.NameToLayer(layerName));
    }

    public static bool ContainsLayer(int mask, string layerName) {
        return (mask & (1 << LayerMask.NameToLayer(layerName))) != 0;
    }
}