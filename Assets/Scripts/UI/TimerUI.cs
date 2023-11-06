using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TMP_Text _TimerText;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.s_instance.onTimerUpdate += UpdateTime;
    }

    private void UpdateTime(string myTimeToUpdate)
    {
        _TimerText.text = myTimeToUpdate; 
    }
}
