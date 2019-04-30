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

    public GameObject loadingScreen;
    public Slider slider;

    void Start()
    {
        my_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player_movement = GameObject.Find("RotatingCollider").GetComponent<PlayerMovement>();
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
        SceneManager.LoadScene("StageSelect");
    }

    public void Victory()
    {
        SoundManager.instance.StopBGM();
        ingameMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        victoryScreen.SetActive(true);
    }

    public void Lose()
    {
        SoundManager.instance.StopBGM();
        ingameMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        loseScreen.SetActive(true);
    }

    public void NextLevel()
    {
        PlayerTurn.Clear();
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySFX(3);
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
}
