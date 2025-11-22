using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCombatCard", menuName = "Cards/CombatCard")]
public class CardCombat : ScriptableObject, CardData
{
    public enum Type {
        None, Torch, Bow, Sword, Mace, Shield, Sickle, Axe, Spear
    }
	public string TacticName;
    public Type WeaponType;
    private int _value;
    private string _use;
    public string Effect;
    private int _casualties;
    private bool _singlePlayer;
    private bool _expansionCard;

    public string Title => name;
    public void Copy(CombatCardJson jsonData) {
        TacticName = jsonData.tactic_name;
        _value = jsonData.value;
        _use = jsonData.use;
        Effect = jsonData.effect;
        _casualties = jsonData.casualties;
        _singlePlayer = jsonData.single_player;
        _expansionCard = jsonData.expansion_card;

        if (!Enum.TryParse<Type>(jsonData.type, ignoreCase: true, out WeaponType)) {
            Debug.LogError($"Unknown WeaponType: {jsonData.type}. Using 'None'.");
            WeaponType = Type.None;
        }
    }
}
