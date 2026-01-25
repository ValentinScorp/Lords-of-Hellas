using System;

public class HopliteStackViewModel : TokenViewModel
{
    public event Action<int> CountChanged;
    public HopliteStackViewModel(HopliteStackModel hopliteStack) : base(hopliteStack)
    {
        CountChanged?.Invoke(hopliteStack.Count);
        hopliteStack.OnCountChanged += HandleCountChanged;
    }
    private void HandleCountChanged(int value) => CountChanged?.Invoke(value);
    public void Add(HopliteModel hoplite)
    {
        if (Model is HopliteStackModel hopliteStack) {
            hopliteStack.AddHoplite(hoplite);
        }
    }
    
    public override void Dispose()
    {
        if (Model is HopliteStackModel hoplite) {
            hoplite.OnCountChanged -= HandleCountChanged;
        }
        base.Dispose();
    }
}
