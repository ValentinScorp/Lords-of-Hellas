using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestCard", menuName = "Cards/QuestCard")]
internal class CardQuest : ScriptableObject, CardData
{
    internal string quest;
    internal string region;
    internal string[] stepsCondition;
    internal string reward;

    public string Title => quest;
    internal string Region => region;
    internal string Reward => reward;
}
