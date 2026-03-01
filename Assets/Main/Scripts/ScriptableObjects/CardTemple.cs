using UnityEngine;

[CreateAssetMenu(fileName = "NewTempleCard", menuName = "Cards/TempleCard")]
internal class CardTemple : ScriptableObject, CardData
{
    internal enum CardId {
        AttributesRaise,
        DrawCombatCards,
        DrawNeutralArtifact,
        PriestAndDraft,
        SpecialActionPlay
    }

    internal string cardName;
    internal CardId Id;
    internal Sprite Image;
    internal bool[] drafts = new bool[8];
    internal PlayerAbitilyAsset OracleBonus;
    internal string Title => cardName;

    string CardData.Title => Title;
}
