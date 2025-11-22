using System;
using System.Collections.Generic;

public class GameLogger
{
    private static GameLogger _instance;
    private List<string> _messages;
    public static GameLogger Instance => _instance ??= new GameLogger();
    public List<string> Messages => _messages;

    public event Action<string> OnEventTrigger;

    private GameLogger() {
        _messages = new List<string>();
    }
    public void Event(string message) {
        _messages.Add(message);
        OnEventTrigger?.Invoke(message);
    }
}
