using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    private Player my_player;
    private PlayerMovement player_movement;
    public GameObject pauseMenuUI;
    public GameObject ingameMenuUI;
    public GameObject victoryScreen;
    public GameObject loseScreen;
    public GameObject popupScreen;
    public GameObject popupImage;
    public GameObject Xbutton;
    public Sprite[] popups;
    public GameObject allpopupScreen;
    public GameObject allpopupImage;
    public GameObject allXbutton;
    public Sprite[] allPopups;

    private int currentPopupPage;
    private int currentAllPopupPage;
    public GameObject loadingScreen;
    public Slider slider;

    void Start()
    {
        my_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player_movement = GameObject.Find("RotatingCollider").GetComponent<PlayerMovement>();

        if (popups.Length > 0)
        {
            if (!PlayerPrefs.HasKey("tutorialRan"))
            {
                PlayerPrefs.SetInt("tutorialRan", 1);
                currentPopupPage = 0;
                popupImage.GetComponent<Image>().sprite = popups[currentPopupPage];
                Invoke("showPopups", 1.2f);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (popupScreen.activeSelf == true)
        {
            popupImage.GetComponent<Image>().sprite = popups[currentPopupPage];

            if (currentPopupPage == popups.Length - 1)
                Xbutton.SetActive(true);
        }

        if (allpopupScreen.activeSelf == true)
        {
            allpopupImage.GetComponent<Image>().sprite = allPopups[currentAllPopupPage];

            if (currentAllPopupPage == PlayerPrefs.GetInt("allPopupLength") - 1)
                allXbutton.SetActive(true);
        }
    }

    public void Resume()
    {
        // maybe buggy?
        my_player.enabled = true;
        player_movement.enabled = true;
        SoundManager.instance.PlaySFX(3);

        pauseMenuUI.SetActive(false);
        ingameMenuUI.SetActive(true);
        Time.timeScale = 1f;
        gameIsPaused = false;
        PlayerTurn.Pause = false;
    }

    void Pause()
    {
        // maybe buggy?
        my_player.enabled = false;
        player_movement.enabled = false;

        pauseMenuUI.SetActive(true);
        ingameMenuUI.SetActive(false);
        Time.timeScale = 0f;
        gameIsPaused = true;
        PlayerTurn.Pause = true;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        PlayerTurn.Restart();
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySFX(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySFX(3);
        PlayerTurn.Pause = false;
        SceneManager.LoadScene("StageSelect");

        if (PlayerPrefs.HasKey("tutorialRan"))
        {
            PlayerPrefs.DeleteKey("tutorialRan");
        }
    }

    public void Title()
    {
        Time.timeScale = 1f;
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySFX(3);
        PlayerTurn.Pause = false;
        SceneManager.LoadScene("TitleScreen");

        if (PlayerPrefs.HasKey("tutorialRan"))
        {
            PlayerPrefs.DeleteKey("tutorialRan");
        }
    }

    public void Victory()
    {
        SoundManager.instance.StopBGM();
        ingameMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        victoryScreen.SetActive(true);
        PlayerTurn.Pause = true;
    }

    public void Lose()
    {
        SoundManager.instance.StopBGM();
        ingameMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        loseScreen.SetActive(true);
        PlayerTurn.Pause = true;
    }

    public void NextLevel()
    {
        PlayerTurn.Clear();
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySFX(3);
        loadingScreen.SetActive(true);
        if (SceneManager.GetActiveScene().buildIndex == 9)
        {
            PlayerPrefs.SetInt("runCutscene", 1);
            SceneManager.LoadScene("Cutscenes");
        }
        else if (SceneManager.GetActiveScene().buildIndex == 14)
        {
            PlayerPrefs.SetInt("runCutscene", 3);
            SceneManager.LoadScene("Cutscenes");
        }
        else if (SceneManager.GetActiveScene().buildIndex == 19)
        {
            PlayerPrefs.SetInt("runCutscene", 5);
            SceneManager.LoadScene("Cutscenes");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (PlayerPrefs.HasKey("tutorialRan"))
        {
            PlayerPrefs.DeleteKey("tutorialRan");
        }
    }

    /*
    public void NextLevel()
    {
        PlayerTurn.Clear();
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySFX(3);
        PlayerTurn.Pause = false;
        StartCoroutine(LoadNext(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadNext(int stageIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(stageIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;

            yield return null;
        }
    }
    */

    public void showPopups()
    {
        currentPopupPage = 0;
        popupImage.GetComponent<Image>().sprite = popups[currentPopupPage];
        Xbutton.SetActive(false);

        my_player.enabled = false;
        player_movement.enabled = false;

        popupScreen.SetActive(true);
        ingameMenuUI.SetActive(false);
        Time.timeScale = 0f;
        PlayerTurn.Pause = true;   
    }

    public void nextPopupPage()
    {
        if (currentPopupPage < popups.Length - 1)
            currentPopupPage++;
    }

    public void closePopup()
    {
        my_player.enabled = true;
        player_movement.enabled = true;
        SoundManager.instance.PlaySFX(3);

        popupScreen.SetActive(false);
        ingameMenuUI.SetActive(true);
        Time.timeScale = 1f;
        PlayerTurn.Pause = false;
    }

    public void showAllPopups()
    {
        currentAllPopupPage = 0;
        allpopupImage.GetComponent<Image>().sprite = allPopups[currentAllPopupPage];
        allXbutton.SetActive(false);

        my_player.enabled = false;
        player_movement.enabled = false;

        allpopupScreen.SetActive(true);
        ingameMenuUI.SetActive(false);
        Time.timeScale = 0f;
        PlayerTurn.Pause = true;
    }

    public void nextAllPopupPage()
    {
        if (currentAllPopupPage < PlayerPrefs.GetInt("allPopupLength") - 1)
            currentAllPopupPage++;
    }

    public void closeAllPopup()
    {
        my_player.enabled = true;
        player_movement.enabled = true;
        SoundManager.instance.PlaySFX(3);

        allpopupScreen.SetActive(false);
        ingameMenuUI.SetActive(true);
        Time.timeScale = 1f;
        PlayerTurn.Pause = false;
    }
}
