using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class stageSelectController : MonoBehaviour
{
    // static public int[] stageClear = { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // 0 - not cleared, 1 - cleared
    //public Image stageTitle;
    //public Sprite[] stageTitleSprites;
    public Image levelImage;
    public Sprite[] levelImageSprites;
    public GameObject stageLocked;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public Text stageName;
    public GameObject loadingScreen;
    public Slider slider;

    private int currentSelect;
    private int levelUnlocked;

    // Start is called before the first frame update
    /*
    void Awake()
    {
        stageClear = new int[SceneManager.sceneCountInBuildSettings - 2];

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings - 2; i++)
        {
            stageClear[i] = 0;
        }
        currentSelect = 0;

        stageClear[0] = 1;
    }
    */

    void Start()
    {
        currentSelect = 0;
        if (!PlayerPrefs.HasKey("stageCompleted"))
        {
            PlayerPrefs.SetInt("stageCompleted", 0);
            levelUnlocked = PlayerPrefs.GetInt("stageCompleted");
        }
        else
        {
            levelUnlocked = PlayerPrefs.GetInt("stageCompleted");
        }
    }

    void Update()
    {
        showLocked();
        showArrows();
        showStageArt();
        showStageName();
    }

    public void startLevel()
    {
        if (currentSelect <= levelUnlocked)
        {
            PlayerTurn.Restart();
            SoundManager.instance.PlaySFX(3);
            loadingScreen.SetActive(true);

            if (currentSelect == 0)
            {
                PlayerPrefs.SetInt("runCutscene", 0);
                SceneManager.LoadScene("Cutscenes");
            }
            else if (currentSelect == 7)
            {
                PlayerPrefs.SetInt("runCutscene", 2);
                SceneManager.LoadScene("Cutscenes");
            }
            else if (currentSelect == 12)
            {
                PlayerPrefs.SetInt("runCutscene", 4);
                SceneManager.LoadScene("Cutscenes");
            }
            else
            {
                SceneManager.LoadScene(currentSelect + 3);
            }
        }
    }
    
    /*
    public void startLevel()
    {
        if (currentSelect <= levelUnlocked)
        {
            PlayerTurn.Restart();
            SoundManager.instance.PlaySFX(3);
            StartCoroutine(LoadStage(currentSelect + 2));
        }
    }

    IEnumerator LoadStage(int stageIndex)
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

    public void goBackToTitle()
    {
        SoundManager.instance.PlaySFX(3);
        SceneManager.LoadScene("TitleScreen");
    }

    public void startGame()
    {
        SoundManager.instance.PlaySFX(3);
        SceneManager.LoadScene("StageSelect");
    }

    public void Quit()
    {
        SoundManager.instance.PlaySFX(3);
        Application.Quit();
    }

    public void goLeft()
    {
        SoundManager.instance.PlaySFX(3);
        if (currentSelect > 0)
        {
            currentSelect--;
        }
    }

    public void goRight()
    {
        SoundManager.instance.PlaySFX(3);
        if (currentSelect < 15)
        {
            currentSelect++;
        }
    }

    private void showArrows()
    {
        if (currentSelect == 0)
        {
            leftArrow.SetActive(false);
        }
        else
        {
            leftArrow.SetActive(true);
        }
        if (currentSelect == 15)
        {
            rightArrow.SetActive(false);
        }
        else
        {
            rightArrow.SetActive(true);
        }

    }

    private void showLocked()
    {
        if (currentSelect > levelUnlocked)
        {
            stageLocked.SetActive(true);
        }
        else
        {
            stageLocked.SetActive(false);
        }
    }

    private void showStageArt()
    {
        // stageTitle.sprite = stageTitleSprites[currentSelect];
        levelImage.sprite = levelImageSprites[currentSelect];
        
        var tempColor = levelImage.color;
        if (currentSelect > levelUnlocked)
        {
            tempColor.a = 0.5f;
        }
        else
        {
            tempColor.a = 1f;
        }
        levelImage.color = tempColor;
        
    }

    private void showStageName()
    {
        int chapterNum;
        int stageNum;

        if (currentSelect < 7)
        {
            stageNum = currentSelect + 1;
            chapterNum = 1;
        }
        else if (currentSelect < 12)
        {
            stageNum = currentSelect - 6;
            chapterNum = 2;
        }
        else
        {
            stageNum = currentSelect - 11;
            chapterNum = 3;
        }

        stageName.text = chapterNum.ToString() + " - " + stageNum.ToString();
    }
}
