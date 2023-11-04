using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "SettingsScriptable", menuName = "Custom/Settings")]
public class SettingsScriptable : ScriptableObject
{
    [Header("Bomb Settings")]
    [SerializeField] private float levelTimer;
    public float LevelTimer { get { return levelTimer; } }

    [SerializeField] private float bombRadius;
    public float BombRadius { get {  return bombRadius; } }

    [SerializeField][Range(0, 1f)] private float tickVolume;
    public float TickVolume { get { return tickVolume; } }
}
