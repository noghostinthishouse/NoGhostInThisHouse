﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class startLevelButton : MonoBehaviour
{
    public void startLevel()
    {
        SceneManager.LoadScene("SampleLevel");
    }
}
