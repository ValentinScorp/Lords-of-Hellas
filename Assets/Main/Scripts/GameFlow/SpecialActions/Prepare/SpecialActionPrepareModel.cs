public class SpecialActionPrepareModel
{
    private int _optionsCounter;
    private const int OptionsMax = 2;
    internal bool NoOptionsLeft => _optionsCounter >= OptionsMax;
    internal void NextOption()
    {
        if (_optionsCounter >= OptionsMax) return;
        _optionsCounter++;
    }
    internal void Reset()
    {
        _optionsCounter = 0;
    }
}
