using System.Linq;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public class CardFactory<TSO, TJSON> where TSO : ScriptableObject where TJSON : class
{
    private readonly Action<TJSON, TSO> _mapper;
    public CardFactory(Action<TJSON, TSO> mapper) {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public TSO[] LoadCards(TextAsset jsonFile) {
        if (jsonFile == null) {
            Debug.LogError("JSON file is null " + jsonFile.name);
            return null;
        }

        try {
            var jsonWrapper = JsonUtility.FromJson<CardListJsonWrapper<TJSON>>(jsonFile.text);
            if (jsonWrapper?.cards == null || jsonWrapper.cards.Length == 0) {
                Debug.LogWarning("JSON data is empty or invalid");
                return null;
            }

            return ConvertJsonToScriptables(jsonWrapper.cards);

        } catch (Exception ex) {
            Debug.LogError($"Failed to load JSON: {ex.Message}");
            return null;
        }
    }
    public TSO[] LoadCards(string filePath) {
        string fullPath = Application.dataPath + "/Main/Resources/" + filePath;
        if (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath)) {
            Debug.LogError($"File path is invalid or file does not exist: {fullPath}");
            return null;
        }

        try {
            string jsonText = File.ReadAllText(fullPath);
            var jsonWrapper = JsonUtility.FromJson<CardListJsonWrapper<TJSON>>(jsonText);
            if (jsonWrapper?.cards == null || jsonWrapper.cards.Length == 0) {
                Debug.LogWarning("JSON data is empty or invalid");
                return null;
            }

            return ConvertJsonToScriptables(jsonWrapper.cards);
        } catch (Exception ex) {
            Debug.LogError($"Failed to load JSON from {fullPath}: {ex.Message}");
            return null;
        }
    }
    private TSO[] ConvertJsonToScriptables(TJSON[] jsonArray) { 

        return jsonArray.Select(json =>
        {
            var so = ScriptableObject.CreateInstance<TSO>();
            _mapper(json, so);
            return so;
        }).ToArray();
    }
}