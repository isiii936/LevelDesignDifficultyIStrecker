using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "SettingsScriptable", menuName = "Custom/Settings")]
public class SettingsScriptable : ScriptableObject
{
    [Header("Bomb Settings")]
    [SerializeField] private float _levelTimer;
    public float LevelTimer => _levelTimer;

    [SerializeField] private float _bombRadius;
    public float BombRadius => _bombRadius;

    [Header("Bomb Audio Settings")]
    [SerializeField] float _SoundRadius;
    public float SoundRadius => _SoundRadius;

    [SerializeField] [Range(0, 1f)] float _tickVolume;
    public float tickVolume => _tickVolume;

    public void SetDefaultSetting()
    {
        _levelTimer = 60f;
        _bombRadius = 3f;
        _SoundRadius = 3.5f;
        _SoundRadius = 3.5f;
        _tickVolume = 1f;
    }
}
