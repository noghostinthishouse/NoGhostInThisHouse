using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class cutscenesController : MonoBehaviour
{
    public GameObject[] cutscenePanels;
    public GameObject loadingScreen;
    int runScene;

    void Start()
    {
        for (int i = 0; i < cutscenePanels.Length; i++)
        {
            cutscenePanels[i].SetActive(false);
        }

        runScene = PlayerPrefs.GetInt("runCutscene");
        playCutscene(runScene);
    }

    public void playCutscene(int cutsceneNum)
    {
        cutscenePanels[cutsceneNum].SetActive(true);
    }

    public void LoadGameStage()
    {
        PlayerTurn.Restart();
        SoundManager.instance.PlaySFX(3);
        loadingScreen.SetActive(true);

        if (runScene == 0)
        {
            SceneManager.LoadScene(3);
        }
        else if (runScene == 1)
        {
            SceneManager.LoadScene("StageSelect");
        }
        else if (runScene == 2)
        {
            SceneManager.LoadScene(10);
        }
        else if (runScene == 3)
        {
            SceneManager.LoadScene("StageSelect");
        }
        else if (runScene == 4)
        {
            SceneManager.LoadScene(15);
        }
        else if (runScene == 5)
        {
            SceneManager.LoadScene("StageSelect");
        }

    }
}
