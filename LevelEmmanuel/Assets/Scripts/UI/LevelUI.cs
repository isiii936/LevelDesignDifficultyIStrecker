using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUI : MonoBehaviour
{
    [SerializeField] TMP_Text _LevelText;


    int _levelIndex => GameManager.s_instance.currentLevel + 1;
    int _DeathAmount => GameManager.s_instance.deaths;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.s_instance.onWin += UpdateLevelUI;
        GameManager.s_instance.onDeath += UpdateLevelUI;
        UpdateLevelUI();
    }

    private void UpdateLevelUI()
    {
        _LevelText.text = "Level: "+_levelIndex+" Death: "+_DeathAmount; 
    }
}
