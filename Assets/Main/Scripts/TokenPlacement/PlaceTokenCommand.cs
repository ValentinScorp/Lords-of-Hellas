using System;

public class PlaceTokenCommand
{
    private readonly Func<bool> _canExecute;
    private readonly Action _execute;

    public event Action<bool> OnInteractableChanged;

    public PlaceTokenCommand(Action execute, Func<bool> canExecute) {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute => _canExecute?.Invoke() ?? true;

    public void Execute() {
        if (!CanExecute) return;
        _execute?.Invoke();
    }

    public void Refresh() {
        OnInteractableChanged?.Invoke(CanExecute);
    }
}
