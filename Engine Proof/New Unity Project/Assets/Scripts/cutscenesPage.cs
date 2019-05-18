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

        cutscenesCon = GameObject.FindGameObjectWithTag("CutsceneController").GetComponent<cutscenesController>();

        for (int i = 0; i < cutscenes.Length; i++)
        {
            cutscenes[i].SetActive(false);
        }

        cutscenes[0].SetActive(true);
        if (cutscenes[0].GetComponent<PanelSFX>().hasSFX)
        {
            SoundManager.instance.PlaySFX(cutscenes[0].GetComponent<PanelSFX>().soundIndex);
            Debug.Log("playsfx");
        }
        else if (cutscenes[0].GetComponent<PanelSFX>().hasBGM)
        {
            Debug.Log("playbgm");
            SoundManager.instance.PlayBGM(cutscenes[0].GetComponent<PanelSFX>().soundIndex);
        }
        currentScene = 0;
    }

    public void nextScene()
    {
        Debug.Log("WW");

        currentScene++;

        if (currentScene != cutscenes.Length)
        {
            cutscenes[currentScene].SetActive(true);
            if (cutscenes[currentScene].GetComponent<PanelSFX>().hasSFX)
            {
                SoundManager.instance.PlaySFX(cutscenes[currentScene].GetComponent<PanelSFX>().soundIndex);
                Debug.Log("playsfx");
            }
            else if (cutscenes[currentScene].GetComponent<PanelSFX>().hasBGM)
            {
                Debug.Log("playbgm");
                SoundManager.instance.PlayBGM(cutscenes[currentScene].GetComponent<PanelSFX>().soundIndex);
            }
        }
        else
        {
            cutscenesCon.LoadGameStage();
        }
    }
}
