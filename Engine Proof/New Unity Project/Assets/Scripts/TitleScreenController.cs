using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    public GameObject controlsScreen;
    public GameObject creditsScreen;
   
    // DEV TOOLS COMMENTED OUT
    /*
    void Update()
    {
        
        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("Delete All PlayerPrefs");
            PlayerPrefs.DeleteAll();
        }
    }
    */

    public void startGame()
    {
        // PlayerPrefs.DeleteAll();
        SoundManager.instance.PlaySFX(3);
        SoundManager.instance.StopBGM();
        SceneManager.LoadScene("StageSelect");
    }

    public void Quit()
    {
        SoundManager.instance.PlaySFX(3);
        Application.Quit();
    }

    public void showControlsScreen()
    {
        controlsScreen.SetActive(true);
    }

    public void showCreditsScreen()
    {
        creditsScreen.SetActive(true);
    }

    public void goBack()
    {
        creditsScreen.SetActive(false);
        controlsScreen.SetActive(false);
    }
}
