using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "NewArtifactCard", menuName = "Cards/ArtifactCard")]
public class CardArtifact : ScriptableObject, CardData
{
    public enum Type {
        None, God, Neutral, Monster
    }
    public Type ArtifactType { get; set; }
    public string Origin { get; set; }
    public string Title { get; set; }
    public CardArtifactId Id {  get; set; }
    public string Description { get; set; }
    public bool Neutral { get; set; }
    public Player PlayerOwner { get; set; }
    public bool Charged { get; set; }
}
