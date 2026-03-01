using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "NewArtifactCard", menuName = "Cards/ArtifactCard")]
internal class CardArtifact : ScriptableObject, CardData
{
    internal enum Type {
        None, God, Neutral, Monster
    }
    internal Type ArtifactType { get; set; }
    internal string Origin { get; set; }
    public string Title { get; set; }
    internal CardArtifactId Id {  get; set; }
    internal string Description { get; set; }
    internal bool Neutral { get; set; }
    internal Player PlayerOwner { get; set; }
    internal bool Charged { get; set; }
}
