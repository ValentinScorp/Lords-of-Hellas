using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameContent
{
    private static GameContent _instance;
    public static GameContent Instance => _instance ??= new GameContent();
    private GameContent() { }
    private List<CardMonsterAttack> _monsterAttackCards;
    private List<CardCombat> _combatCards;
    private List<CardArtifact> _artifactCards;
    private List<CardBlessing> _blessingCards;
    private List<CardMonster> _monsterCards;
    private List<CardQuest> _questCards;
      
    public static PlayerColorPalette PlayerColorPalette { get; private set; }
    public static TokenMaterialPalette TokenMaterialPalette { get; private set; }
    public static GlobalMaterials GlobalMaterials { get; private set; }
    public IReadOnlyList<CardTemple> TempleCards { get; private set; }
    public List<CardMonsterAttack> MonsterAttackCards => _monsterAttackCards;
    public List<CardCombat> CombatCards => _combatCards;
    public List<CardArtifact> ArtifactCards => _artifactCards;
    public List<CardBlessing> BlessingCards => _blessingCards;
    public List<CardMonster> MonsterCards => _monsterCards;
    public List<CardQuest> QuestCards => _questCards;
    public List<RegionConfig> RegionsConfig { get; private set; }
    
    private bool _initialized = false;

    public void Initialize() {
        if (_initialized) return;
        _initialized = true;

        PlayerColorPalette = Resources.Load<PlayerColorPalette>("ScriptableObjects/GameSetup/PlayerColorPalette");
        if (PlayerColorPalette == null) {
            Debug.LogWarning("ColorPalette not loaded!");
        }
        TokenMaterialPalette = Resources.Load<TokenMaterialPalette>("ScriptableObjects/GameSetup/TokenMaterialPalette");
        if (TokenMaterialPalette == null) {
            Debug.LogWarning("ColorPalette not loaded!");
        }
        GlobalMaterials = Resources.Load<GlobalMaterials>("ScriptableObjects/GameSetup/GlobalMaterials");
        if (GlobalMaterials == null) {
            Debug.LogWarning("GlobalMaterials not loaded!");
        }
        TempleCards = Resources.LoadAll<CardTemple>("ScriptableObjects/Cards/Temple");
        if (TempleCards == null) {
            Debug.LogWarning("Temple Cards not loaded!");
        }

        CardLoader.Instance.LoadAllCards();

        _monsterAttackCards = new List<CardMonsterAttack>(CardLoader.Instance.MonsterAttackCards);
        _combatCards = new List<CardCombat>(CardLoader.Instance.CombatCards);
        _artifactCards = new List<CardArtifact>(CardLoader.Instance.ArtifactCards);
        _blessingCards = new List<CardBlessing>(CardLoader.Instance.BlessingCards);
        _monsterCards = new List<CardMonster>(CardLoader.Instance.MonsterCards);
        _questCards = new List<CardQuest>(CardLoader.Instance.QuestCards);

        var regConfLoader = new RegionConfigLoader();
        RegionsConfig = new List<RegionConfig>();

        if (regConfLoader.TryLoadRegions(out var regCfgs)) {
            RegionsConfig = regCfgs;
        }
    }
    public Hero.Id GetPlayerHeroId(PlayerColor color) {
        var player = GameConfig.Instance.Players.FirstOrDefault(p => p.PlayerColor == color);
        if (player == null) {
            Debug.LogError($"[GameContext] Player with color {color} not found!");
            return default;
        }
        return player.HeroId;
    }
    public LandId GetLandColor(RegionId regionId) {
        var regionData = RegionsConfig.FirstOrDefault(r => RegionIdParser.Parse(r.RegionName) == regionId);
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
    public bool TryGetTempleCard(int index, out CardTemple card)
    {
        card = null;
        if (TempleCards == null || index < 0 || index >= TempleCards.Count) {
            return false;
        }
        card = TempleCards[index];
        return card != null;
    }
}

