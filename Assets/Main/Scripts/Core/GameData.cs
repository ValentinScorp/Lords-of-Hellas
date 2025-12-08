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
    public static GlobalMaterials GlobalMaterials { get; private set; }
    public List<CardMonsterAttack> MonsterAttackCards => _monsterAttackCards;
    public List<CardCombat> CombatCards => _combatCards;
    public List<CardArtifact> ArtifactCards => _artifactCards;
    public List<CardBlessing> BlessingCards => _blessingCards;
    public List<CardMonster> MonsterCards => _monsterCards;
    public List<CardQuest> QuestCards => _questCards;

    private List<RegionConfig> _regionStaticData = new();
    public List<RegionConfig> RegionStaticData => _regionStaticData;

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
        GlobalMaterials = Resources.Load<GlobalMaterials>("ScriptableObjects/GameSetup/GlobalMaterials");
        if (GlobalMaterials == null) {
            Debug.Log("GlobalMaterials not loaded!");
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
    public LandId GetLandColor(RegionId regionId) {
        var regionData = _regionStaticData.FirstOrDefault(r => RegionIdParser.Parse(r.RegionName) == regionId);
        if (regionData == null) {
            Debug.LogError($"[GameData] Region data not found for id {regionId}");
            return LandId.Red;
        }
        return ParseLandColor(regionData.LandColor);
    }
    private LandId ParseLandColor(string landColor) {
        switch (landColor.ToLowerInvariant()) {
            case "red": return LandId.Red;
            case "yellow": return LandId.Yellow;
            case "blue": return LandId.Blue;
            case "green": return LandId.Green;
            case "brown": return LandId.Brown;
            default:
                Debug.LogError($"[GameData] Unknown land color: {landColor}");
                return LandId.Red;
        }
    }
    public Color GetPlayerColor(PlayerColor playerColor)
    {
        if (PlayerColorPalette == null)
        {
            Debug.LogError("PlayerColorPalette is not loaded.");
            return Color.white;
        }

        switch (playerColor)
        {
            case PlayerColor.Red:    return PlayerColorPalette.Red;
            case PlayerColor.Blue:   return PlayerColorPalette.Blue;
            case PlayerColor.Green:  return PlayerColorPalette.Green;
            case PlayerColor.Yellow: return PlayerColorPalette.Yellow;
            case PlayerColor.Purple: return PlayerColorPalette.Purple;
            case PlayerColor.Brown:  return PlayerColorPalette.Brown;
            case PlayerColor.Gray:
            default:                 return PlayerColorPalette.Grey;
        }
    }
}

