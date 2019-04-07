using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    public void startGame()
    {
        SoundManager.instance.PlaySFX(3);
        SoundManager.instance.StopBGM();
        SceneManager.LoadScene("StageSelect");
    }

    public void Quit()
    {
        SoundManager.instance.PlaySFX(3);
        Application.Quit();
    }
}
