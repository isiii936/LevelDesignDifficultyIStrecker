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
    public event Action onWinUI;

    public const string StopWatchID = "GameWatch"; 

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
        GlobalTimer.Timer.CreateStopWatch(StopWatchID);
        GlobalTimer.Timer.ResetStopWatch(StopWatchID);
        GlobalTimer.Timer.PauseStopWatch(StopWatchID, false);
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
            onWinUI?.Invoke();
            gameOverContainer.SetActive(true);
            GlobalTimer.Timer.PauseStopWatch(StopWatchID);
            Debug.Log("Time: "+GlobalTimer.Timer.getStopWatchTime(StopWatchID));
            GlobalTimer.Timer.SetTimer(5f, () => _currentLevel++);
            GlobalTimer.Timer.SetTimer(5f, () => StartUpLevel(_currentLevel));
            GlobalTimer.Timer.SetTimer(5.1f, () => gameOverContainer.SetActive(false));
            GlobalTimer.Timer.SetTimer(5.1f, () => onWin?.Invoke());
            GlobalTimer.Timer.SetTimer(4f, () => deaths = 0);
        }

        Debug.Log(_currentLevel);
    }
}
