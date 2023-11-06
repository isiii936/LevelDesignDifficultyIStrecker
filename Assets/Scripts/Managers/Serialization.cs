using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*
 * Author: Simon Grunwald
 * Created: 06.11.2023
 * Edited: 06.11.2023
 */

public static class Serialization
{
    static string _filePath;

    static int _currentLevel => GameManager.s_instance.currentLevel;
    static int _deathCounter;



    public static void SaveGame()
    {
        StreamWriter writer = new StreamWriter(_filePath, true);
        writer.WriteLine("Deaths:");
        writer.WriteLine(_deathCounter);
        writer.WriteLine("Level:");
        writer.WriteLine(_currentLevel);
        writer.Close();
    }

    public static void LoadGame()
    {

    }
}
