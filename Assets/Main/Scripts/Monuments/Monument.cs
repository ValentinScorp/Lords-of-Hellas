using System;

public class Monument
{
    private readonly MonumentType _type;
    private int _level = 1;
    public int Level => _level;
    public MonumentType Type => _type;
    public Monument(MonumentType type) {
        _type = type;
    }
    public int IncreaseLevel() => _level = Math.Min(_level + 1, 5);
    public int DecreaseLevel() => _level = Math.Max(_level - 1, 0);
}
