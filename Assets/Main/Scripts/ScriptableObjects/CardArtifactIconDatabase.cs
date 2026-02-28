using UnityEngine;

[CreateAssetMenu(
    fileName = "ArtifactCardIconDatabase", 
    menuName = "Cards/CombatArtifactIconDatabase"
)]
public class CardArtifactIconDatabase : ScriptableObject
{
    public Sprite God;
    public Sprite Neutral;
    public Sprite Monster;
}
