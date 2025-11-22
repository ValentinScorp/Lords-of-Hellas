using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;
using static Board;

public class TokenPlacementRecorder
{
    private class Step {
        public TokenType TokenType { get; }
        public RegionId RegionId { get; }
        public PlayerColor PlayerColor { get; }
        public Vector3 ObjectPosition { get; }
        public Step(PlayerColor playerColor, TokenType tokenType, RegionId regionId, Vector3 objectPosition) {
            PlayerColor = playerColor;
            TokenType = tokenType;
            RegionId = regionId;
            ObjectPosition = objectPosition;
        }
    }
    private List<Step> _steps = new();
    private Step _lastStep;
    public RegionId LastStepRegionId => _lastStep.RegionId;
    public TokenType LastStepTokenType => _lastStep.TokenType;
    public PlayerColor LastStepPlayerColor => _lastStep.PlayerColor;

    public void AddStep(PlayerColor playerColor, TokenType tokenType, RegionId regionId, Vector3 objectPosition) {
        _lastStep = new Step(playerColor, tokenType, regionId, objectPosition);
        _steps.Add(_lastStep);
        //Debug.Log("Recording add token: " + tokenType.ToString() + " " + RegionIdParser.IdToString(regionId));
    }
    public void RemoveLastStep() {
        if (_steps.Count == 0) {
            _lastStep = null;
            return;
        }
        Debug.Log("Remove last step from recorder: " + _lastStep.TokenType.ToString() + " " + RegionIdParser.IdToString(_lastStep.RegionId));
        _steps.RemoveAt(_steps.Count - 1);
        _lastStep = _steps.Count > 0 ? _steps[^1] : null;
    }
}
