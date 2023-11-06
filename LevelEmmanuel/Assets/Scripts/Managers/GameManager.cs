using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Base;

public class GameManager : MonoBehaviour
{
    public static GameManager s_instance;

    [SerializeField] SettingsScriptable[] _bombSettings;
    public SettingsScriptable[] bombSettings => _bombSettings;

    [SerializeField] GameObject[] _bombPlacement;

    [SerializeField] Transform _SpawnPoint;
    public Transform SpawnPoint => _SpawnPoint;

    public event Action<string> onTimerUpdate;
    public event Action<bool> onGameStateChange;

    public event Action onDeath;
    public event Action onWin;

    string _BombTimerStr;
    public string BombTimerStr
    {
        get { return _BombTimerStr; }
        private set
        {
            _BombTimerStr = value;
            onTimerUpdate?.Invoke(value);
        }
    }

    float _BombTimer => _bombSettings[currentLevel].LevelTimer;

    Action _BombAction;

    [HideInInspector] public int deaths;

    int _currentLevel = 0;
    public int currentLevel => _currentLevel;

    public GameObject pauseContainer, menuContainer, gameOverContainer, uiContainer;


    //Gameplay
    public TextMeshProUGUI deathText, timerText;
    //EndScreen
    public TextMeshProUGUI finalDeaths;

    void Awake()
    {
        if (s_instance is null) s_instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        onGameStateChange?.Invoke(true);
        _BombAction = new Action(Death);
        StartUpLevel(_currentLevel);
    }

    private void LateUpdate()
    {
        int Minutes = (int)(GlobalTimer.Timer.GetTimerStatus(_BombAction) / 60);
        int Seconds = (int)GlobalTimer.Timer.GetTimerStatus(_BombAction)%60;
        BombTimerStr = Minutes.ToString("00")+":"+Seconds.ToString("00");
    }

    void StartUpLevel(int myLevelIndex)
    {
        for (int i = 0; i < _bombPlacement.Length; i++) _bombPlacement[i].SetActive(false);
        _bombPlacement[_currentLevel].SetActive(true);
        GlobalTimer.Timer.DeleteTimer(_BombAction);
        GlobalTimer.Timer.SetTimer(_BombTimer, _BombAction);
    }
    public void PauseGame()
    {
        onGameStateChange?.Invoke(false);
        GlobalTimer.Timer.PauseTimer(_BombAction);
        uiContainer.SetActive(false);
        pauseContainer.SetActive(true);
    }

    public void ResumeGame()
    {
        onGameStateChange?.Invoke(true);
        GlobalTimer.Timer.PauseTimer(_BombAction, false);
        uiContainer.SetActive(true);
        pauseContainer.SetActive(false);
    }

    public void Death()
    {
        deaths++;
        GlobalTimer.Timer.DeleteTimer(_BombAction);
        Debug.Log(_bombSettings[_currentLevel].LevelTimer);
        GlobalTimer.Timer.SetTimer(_BombTimer, _BombAction);
        onDeath?.Invoke();
    }

    public void Win()
    {
        if(_bombPlacement.Length > 0 && _bombPlacement.Length-1 >= _currentLevel)
        {
            deaths = 0;
            _currentLevel++;
            StartUpLevel(_currentLevel);
            onWin?.Invoke();
        }

        Debug.Log(_currentLevel);
    }
}
