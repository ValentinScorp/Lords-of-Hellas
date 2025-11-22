using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestCard", menuName = "Cards/QuestCard")]
public class CardQuest : ScriptableObject, CardData
{
    public string quest;
    public string region;
    public string[] stepsCondition;
    public string reward;

    public string Title => quest;
    public string Region => region;
    public string Reward => reward;
}
