using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class stageSelectController : MonoBehaviour
{
    static public int[] stageClear = { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // 0 - not cleared, 1 - cleared
    public Image stageTitle;
    public Sprite[] stageTitleSprites;
    public Image levelImage;
    public Sprite[] levelImageSprites;
    public GameObject stageLocked;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public Text stageName;

    private int currentSelect;

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
        if (stageClear[currentSelect] == 1)
        {
            PlayerTurn.Restart();
            SoundManager.instance.PlaySFX(3);
            SceneManager.LoadScene(currentSelect + 2);
        }
    }

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
        if (currentSelect < 10)
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
        if (currentSelect == 10)
        {
            rightArrow.SetActive(false);
        }
        if (currentSelect != 0)
        {
            leftArrow.SetActive(true);
        }
        if (currentSelect != 10)
        {
            rightArrow.SetActive(true);
        }

    }

    private void showLocked()
    {
        if (stageClear[currentSelect] == 0)
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
        if (stageClear[currentSelect] == 0)
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

        if (currentSelect < 6)
        {
            stageNum = currentSelect + 1;
            chapterNum = 1;
        }
        else if (currentSelect < 8)
        {
            stageNum = currentSelect - 5;
            chapterNum = 2;
        }
        else
        {
            stageNum = currentSelect - 7;
            chapterNum = 3;
        }

        stageName.text = chapterNum.ToString() + " - " + stageNum.ToString();
    }
}
