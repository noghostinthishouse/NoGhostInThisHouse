using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class stageSelectController : MonoBehaviour
{
    static public int[] stageClear; // 0 - not cleared, 1 - cleared

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        stageClear = new int[SceneManager.sceneCountInBuildSettings - 2];

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings - 2; i++)
        {
            stageClear[i] = 0;
        }
    }

    public void startLevel(int i)
    {
        PlayerTurn.Restart();
        SceneManager.LoadScene(i);
    }

    public void goBackToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void startGame()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void DisplayStageState(int stageState)
    {
        
    }
}
