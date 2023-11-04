using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] public SettingsScriptable[] bombSettings;

    void Update()
    {

        if ((Camera.main.transform.position - transform.position).magnitude <= bombSettings[GameController.instance.currentLevel].BombRadius)
        {
            GameController.instance.AddDeath();
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < bombSettings.Length; i++)
        {
            Gizmos.DrawWireSphere(transform.position, bombSettings[i].BombRadius);
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
}
