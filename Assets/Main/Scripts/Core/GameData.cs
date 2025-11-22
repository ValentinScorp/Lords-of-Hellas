using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Board;

public class GameData
{
    private static GameData _instance;
    public static GameData Instance => _instance ??= new GameData();
    private GameData() { }

    private List<PlayerSetupConfig> _playerConfigs = new();

    private List<CardMonsterAttack> _monsterAttackCards;
    private List<CardCombat> _combatCards;
    private List<CardArtifact> _artifactCards;
    private List<CardBlessing> _blessingCards;
    private List<CardMonster> _monsterCards;
    private List<CardQuest> _questCards;
    
    public IReadOnlyList<PlayerSetupConfig> PlayerConfigs => _playerConfigs.AsReadOnly();
   
    public static PlayerColorPalette PlayerColorPalette { get; private set; }
    public static TokenMaterialPalette TokenMaterialPalette { get; private set; }
    public List<CardMonsterAttack> MonsterAttackCards => _monsterAttackCards;
    public List<CardCombat> CombatCards => _combatCards;
    public List<CardArtifact> ArtifactCards => _artifactCards;
    public List<CardBlessing> BlessingCards => _blessingCards;
    public List<CardMonster> MonsterCards => _monsterCards;
    public List<CardQuest> QuestCards => _questCards;

    private List<RegionStaticData> _regionStaticData = new();
    public List<RegionStaticData> RegionStaticData => _regionStaticData;

    private bool _initialized = false;

    public void Initialize() {
        if (_initialized) return;
        _initialized = true;

        PlayerColorPalette = Resources.Load<PlayerColorPalette>("ScriptableObjects/GameSetup/PlayerColorPalette");
        if (PlayerColorPalette == null) {
            Debug.Log("ColorPalette not loaded!");
        }
        TokenMaterialPalette = Resources.Load<TokenMaterialPalette>("ScriptableObjects/GameSetup/TokenMaterialPalette");
        if (TokenMaterialPalette == null) {
            Debug.Log("ColorPalette not loaded!");
        }
        CardLoader.Instance.LoadAllCards();

        _monsterAttackCards = new List<CardMonsterAttack>(CardLoader.Instance.MonsterAttackCards);
        _combatCards = new List<CardCombat>(CardLoader.Instance.CombatCards);
        _artifactCards = new List<CardArtifact>(CardLoader.Instance.ArtifactCards);
        _blessingCards = new List<CardBlessing>(CardLoader.Instance.BlessingCards);
        _monsterCards = new List<CardMonster>(CardLoader.Instance.MonsterCards);
        _questCards = new List<CardQuest>(CardLoader.Instance.QuestCards);

        _playerConfigs = GameConfig.Instance.GetPlayers();        
    }
    public Hero.Id GetPlayerHeroId(PlayerColor color) {
        var player = _playerConfigs.FirstOrDefault(p => p.PlayerColor == color);
        if (player == null) {
            Debug.LogError($"[GameContext] Player with color {color} not found!");
            return default;
        }
        return player.HeroId;
    }
}
