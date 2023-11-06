using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bomb : MonoBehaviour
{
    void FixedUpdate()
    {
        if ((Camera.main.transform.position - transform.position).magnitude <= GameManager.s_instance.bombSettings[GameManager.s_instance.currentLevel].BombRadius)
        {
            GameManager.s_instance.Death();
        }
    }
}
