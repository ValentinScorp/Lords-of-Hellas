
using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new();

    public static void Register<T>(T instance) where T : class => _services[typeof(T)] = instance;
    public static void Unregister<T>() where T : class => _services.Remove(typeof(T));
    public static T Get<T>() where T : class => _services.TryGetValue(typeof(T), out var s) ? (T)s : null;
}
