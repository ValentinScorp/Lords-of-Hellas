using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly Func<T> _createFunc;
    private readonly Queue<T> _pool = new();
    private readonly int _initialSize;

    public ObjectPool(Func<T> createFunc, int initialSize = 10) {
        _createFunc = createFunc ?? throw new ArgumentNullException(nameof(createFunc));
        _initialSize = initialSize;

        // Попереднє створення об’єктів
        for (int i = 0; i < _initialSize; i++) {
            var obj = CreateNewObject();
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public T Get() {
        T obj;
        if (_pool.Count > 0) {
            obj = _pool.Dequeue();
            obj.gameObject.SetActive(true);
        } else {
            obj = CreateNewObject();
        }
        return obj;
    }

    public void Release(T obj) {
        if (obj != null) {
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    private T CreateNewObject() {
        var obj = _createFunc();
        return obj;
    }
}