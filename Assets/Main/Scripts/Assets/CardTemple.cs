using UnityEngine;

[CreateAssetMenu(fileName = "NewTempleCard", menuName = "Cards/TempleCard")]
public class CardTemple : ScriptableObject, CardData
{
    public enum CardId {
        AttributesRaise,
        DrawCombatCards,
        DrawNeutralArtifact,
        PriestAndDraft,
        SpecialActionPlay
    }

    public string cardName;
    public CardId Id;
    public Sprite Image;
    public bool[] drafts = new bool[8];
    public PlayerAbitilyAsset OracleBonus;
    public string Title => cardName;
}
