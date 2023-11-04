using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform defaultPos;
    public GameObject player;
    [HideInInspector] public bool gamePlaying, gamePaused;
    [HideInInspector] public int deaths;
    [HideInInspector] public float elapsedTime, startTime, pauseTime;
    TimeSpan timePlaying;
    string filePath;

    public GameObject[] level;
    [HideInInspector]public int currentLevel = 0;

    public GameObject pauseContainer, menuContainer, gameOverContainer, uiContainer;


    //Gameplay
    public TextMeshProUGUI deathText, timerText;
    //EndScreen
    public TextMeshProUGUI finalDeaths;

    #region Singleton Setup
    public static GameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        filePath = Application.persistentDataPath + (" - Results.txt");
    }
    #endregion

    private void Start()
    {
        gamePlaying = true;
        startTime = Time.time;
        
    }

    private void Update()
    {
        if(gamePlaying)
        {
            elapsedTime = Time.time - startTime - pauseTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingString = timePlaying.ToString("ss':'ff");
        }
        if (gamePaused)
        {
            pauseTime = Time.time - startTime - elapsedTime;
        }

    }


    public void PauseGame()
    {
        gamePlaying = false;
        gamePaused = true;
        uiContainer.SetActive(false);
        pauseContainer.SetActive(true);
    }
    public void ResumeGame()
    {
        gamePlaying = true;
        gamePaused = false;
        uiContainer.SetActive(true);
        pauseContainer.SetActive(false);
    }

    public void AddDeath()
    {
        deaths++;
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = defaultPos.position;
        player.GetComponent<CharacterController>().enabled = true;
        Debug.Log(deaths);
    }

    public void ChangeLevel()
    {
        if(level.Length > 0 && level.Length-1 >= currentLevel)
        {
            level[currentLevel].SetActive(false);
            currentLevel++;
            level[currentLevel].SetActive(true);
        }
        if(currentLevel <= level.Length-1)
        {
            SaveGame();
        }
        Debug.Log(currentLevel);
    }

    public void SaveGame()
    {
        StreamWriter writer = new StreamWriter(filePath, true);
        writer.WriteLine("~~~");
        writer.WriteLine("Deaths:");
        writer.WriteLine(deaths);
        writer.WriteLine("Level:");
        writer.WriteLine(currentLevel);
        writer.Close();
    }
}
