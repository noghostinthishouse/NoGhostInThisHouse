using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class cutscenesPage : MonoBehaviour
{
    public GameObject[] cutscenes;
    private cutscenesController cutscenesCon;
    int currentScene;

    void Start()
    {
        for (int i = 0; i < cutscenes.Length; i++)
        {
            cutscenes[i].SetActive(false);
        }

        cutscenes[0].SetActive(true);
        currentScene = 0;

        cutscenesCon = GameObject.FindGameObjectWithTag("CutsceneController").GetComponent<cutscenesController>();
    }

    public void nextScene()
    {
        currentScene++;

        if (currentScene != cutscenes.Length)
        {
            cutscenes[currentScene].SetActive(true);
        }
        else
        {
            cutscenesCon.LoadGameStage();
        }
    }
}
