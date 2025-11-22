using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardLoader
{
    private static CardLoader _instance;
    public static CardLoader Instance {
        get {
            if (_instance == null) {
                _instance = new CardLoader();
            }
            return _instance;
        }
    }
    private CardArtifact[] _artifactCards;
    private CardQuest[] _questCards;
    private CardMonster[] _monsterCards;
    private CardMonsterAttack[] _monsterAttackCards;
    private CardCombat[] _combatCards;
    private CardBlessing[] _blessingCards;
    public List<CardArtifact> ArtifactCards => _artifactCards.ToList();
    public List<CardQuest> QuestCards => _questCards.ToList();
    public List<CardMonster> MonsterCards => _monsterCards.ToList();
    public List<CardMonsterAttack> MonsterAttackCards => _monsterAttackCards.ToList();
    public List<CardCombat> CombatCards => _combatCards.ToList();
    public List<CardBlessing> BlessingCards => _blessingCards.ToList();

    private bool _loaded = false;
    public void LoadAllCards() {
        if (_loaded) return;
        _loaded = true;

        var artifactCardFactory = new CardFactory<CardArtifact, CardArtifactJson>(CopyArtifactCardData);
        _artifactCards = artifactCardFactory.LoadCards("ArtifactCards.json");

        LoadEventCards();

        var monsterAttackCardFactory = new CardFactory<CardMonsterAttack, MonsterAttackCardJson>(CopyMonsterAttackCardData);
        _monsterAttackCards = monsterAttackCardFactory.LoadCards("MonsterAttackCards.json");

        var combatCardFactory = new CardFactory<CardCombat, CombatCardJson>(CopyCombatCardData);
        _combatCards = combatCardFactory.LoadCards("CombatCards.json");

        var blessingCardFactory = new CardFactory<CardBlessing, BlessingCardJson>(CopyBlessingCardData);
        _blessingCards = blessingCardFactory.LoadCards("BlessingCards.json");

  /*    Debug.Log($"Loaded {_templeCards.Length} temple cards");
        Debug.Log($"Loaded {questCards.Length} quest cards");
        Debug.Log($"Loaded {monsterCards.Length} monster cards");
        Debug.Log($"Loaded {monsterAttackCards.Length} monster attack cards");
        Debug.Log($"Loaded {combatCards.Length} combat cards");
        Debug.Log($"Loaded {artifactCards.Length} artifact cards");
        Debug.Log($"Loaded {blessingCards.Length} blessing cards");*/
    }

    public void LoadEventCards() {
        var questCardFactory = new CardFactory<CardQuest, QuestCardJson>(CopyQuestCardData);
        _questCards = questCardFactory.LoadCards("QuestCards.json");

        var monsterCardFactory = new CardFactory<CardMonster, MonsterCardJson>(CopyMonsterCardData);
        _monsterCards = monsterCardFactory.LoadCards("MonsterCards.json");
    }
    public bool LoadEventCards(out List<CardData> cards) {
        cards = new List<CardData>();

        var questCardFactory = new CardFactory<CardQuest, QuestCardJson>(CopyQuestCardData);
        var questCards = questCardFactory.LoadCards("QuestCards.json");

        var monsterCardFactory = new CardFactory<CardMonster, MonsterCardJson>(CopyMonsterCardData);
        var monsterCards = monsterCardFactory.LoadCards("MonsterCards.json");
        if (questCards == null || monsterCards == null) {
            cards = null;
            return false;
        }
        cards.AddRange(questCards);
        cards.AddRange(monsterCards);
        return true;
    }

    private void CopyQuestCardData(QuestCardJson sourceJson, CardQuest targetCard) {
        targetCard.quest = sourceJson.quest;
        targetCard.region = sourceJson.region;
        targetCard.stepsCondition = sourceJson.steps_condition;
        targetCard.reward = sourceJson.reward;
    }
    private void CopyMonsterCardData(MonsterCardJson sourceJson, CardMonster targetCard) {
        targetCard.monster = sourceJson.monster;
        targetCard.region = sourceJson.region;
        targetCard.evolveText = sourceJson.evolve_text;
        targetCard.evolveHits = sourceJson.evolve_hits;
    }
    private void CopyMonsterAttackCardData(MonsterAttackCardJson sourceJson, CardMonsterAttack targetCard) {
        targetCard.attackName = sourceJson.attack_name;
        targetCard.value = sourceJson.value;
        targetCard.effect = sourceJson.effect;
    }
    private void CopyCombatCardData(CombatCardJson sourceJson, CardCombat targetCard) {
        targetCard.Copy(sourceJson);        
    }
    private void CopyArtifactCardData(CardArtifactJson sourceJson, CardArtifact targetCard) {
        targetCard.Title = sourceJson.title_name;
        targetCard.Description = sourceJson.effect;
        targetCard.Origin = sourceJson.owner;
        targetCard.Neutral = sourceJson.neutral;

        if (targetCard.Neutral == true) {
            targetCard.ArtifactType = CardArtifact.Type.Neutral;
        } else if (targetCard.Origin == "Athena" ||
              targetCard.Origin == "Hermes" ||
              targetCard.Origin == "Zeus" ||
              targetCard.Origin == "Poseidon" ||
              targetCard.Origin == "Hades" ||
              targetCard.Origin == "Hephaesteus") {
            targetCard.ArtifactType = CardArtifact.Type.God;
        } else {
            targetCard.ArtifactType = CardArtifact.Type.Monster;
        }
    }
    private void CopyBlessingCardData(BlessingCardJson sourceJson, CardBlessing targetCard) {
        targetCard.godName = sourceJson.god_name;
        targetCard.titleName = sourceJson.title_name;
        targetCard.effect = sourceJson.effect;
    }
}