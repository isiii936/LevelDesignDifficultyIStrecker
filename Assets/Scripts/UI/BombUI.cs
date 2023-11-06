using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombUI : MonoBehaviour
{
    [SerializeField] SettingsScriptable[] _bombSettings;

    [SerializeField] bool _ShowSound = false;

    private void OnDrawGizmos()
    {
        if (_bombSettings is null) return;

        for (int i = 0; i < _bombSettings.Length; i++)
        {
            if(!_ShowSound) Gizmos.DrawWireSphere(transform.position, _bombSettings[i].BombRadius);
            else Gizmos.DrawWireSphere(transform.position, _bombSettings[i].SoundRadius);
            switch (i)
            {
                case 0: Gizmos.color = Color.red; break;
                case 1: Gizmos.color = Color.green; break;
                case 2: Gizmos.color = Color.yellow; break;
                case 3: Gizmos.color = Color.black; break;
                case 4: Gizmos.color = Color.magenta; break;
            }
        }
    }
}
