using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerColorPalette", menuName = "Colors/PlayerColorPalette")]
public class PlayerColorPalette : ScriptableObject
{
    public Color Grey;
    public Color Red;
    public Color Blue;
    public Color Green;
    public Color Yellow;
    public Color Purple;
    public Color Brown;

    public List<Color> GetAvailableColors() {
        return new List<Color>
        {
            Red,
            Blue,
            Green,
            Yellow,
            Purple,
            Brown
        };
    }
    public List<PlayerColor> GetAvailablePlayerColors() {
        return new List<PlayerColor>
        {
            PlayerColor.Red,
            PlayerColor.Blue,
            PlayerColor.Green,
            PlayerColor.Yellow,
            PlayerColor.Purple,
            PlayerColor.Brown
        };
    }
}
