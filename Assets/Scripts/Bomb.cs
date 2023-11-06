using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bomb : MonoBehaviour
{
    SettingsScriptable[] _bombSettings;

    private void Start()
    {
        _bombSettings = GameManager.s_instance.bombSettings;
    }

    void Update()
    {
        if ((Camera.main.transform.position - transform.position).magnitude <= _bombSettings[GameManager.s_instance.currentLevel].BombRadius)
        {
            GameManager.s_instance.Death();
        }
    }

    
    /*
    private void OnDrawGizmos()
    {
        for (int i = 0; i < _bombSettings.Length; i++)
        {
            Gizmos.DrawWireSphere(transform.position, _bombSettings[i].BombRadius);
            switch (i +1)
            {
                case 0: Gizmos.color = Color.cyan; break;
                case 1: Gizmos.color = Color.green; break;
                case 2: Gizmos.color = Color.yellow; break;
                case 3: Gizmos.color = Color.red; break;
                case 4: Gizmos.color = Color.magenta; break;
            }
        }
    }
    */
}
