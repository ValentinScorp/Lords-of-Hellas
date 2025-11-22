using UnityEngine;

public class TokenPlacementTracker
{
    private int _maxHoplites;
    private int _maxHeroes;
    private int _placedHoplites;
    private int _placedHeroes;

    public void SetPlacementTargets(int heroesCount, int hoplitesCount) {
        _maxHeroes = heroesCount;
        _maxHoplites = hoplitesCount;
        _placedHeroes = 0;
        _placedHoplites = 0;
    }
    public void Reset() {
        _placedHoplites = 0;
        _placedHeroes = 0;
    }
    public void CountToken(TokenType? type) {
        if (type == TokenType.Hoplite) {
            _placedHoplites++;
        }
        if (type == TokenType.Hero) {
            _placedHeroes++;
        }
    }
    public bool CanPlace(TokenType type) {
        return type switch {
            TokenType.Hoplite => _placedHoplites < _maxHoplites,
            TokenType.Hero => _placedHeroes < _maxHeroes,
            _ => false
        };
    }
    public bool AllPlaced =>
        _placedHeroes >= _maxHeroes && _placedHoplites >= _maxHoplites;

    public int PlacedHoplites => _placedHoplites;
    public int PlacedHeroes => _placedHeroes;
    public int TotalHoplites => _maxHoplites;
    public int TotalHeroes => _maxHeroes;
}
