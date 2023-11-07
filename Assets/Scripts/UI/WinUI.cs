using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Base;

public class WinUI : MonoBehaviour
{
    [SerializeField] TMP_Text _Level, _StopWatch, _Deaths;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.s_instance.onWinUI += UpdateTexts;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateTexts()
    {
        Debug.Log("Update Text");
        _Level.text = "Level: " + (GameManager.s_instance.currentLevel+1).ToString();

        int Seconds = (int)GlobalTimer.Timer.getStopWatchTime(GameManager.StopWatchID) % 60;
        int Minutes = (int)GlobalTimer.Timer.getStopWatchTime(GameManager.StopWatchID) / 60;

        _StopWatch.text = "Time: " + Minutes.ToString("00") + ":" + Seconds.ToString("00");
        _Deaths.text = "Deaths: " + (GameManager.s_instance.deaths).ToString();
    }
}
