using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTurn : MonoBehaviour
{
    public static bool playerTurn = true;
    public static bool GameOver = false;
    public static bool Win = false;
    public static List<bool> ghostFinished = new List<bool>();
    public static int noOfGhost = 0;
    private static bool endTurn;

    //is called by all ghost instances
    //set one value to true when each of them finished their move

    public static void SetGameOver()
    {
        GameObject.FindGameObjectWithTag("Menu Canvas").GetComponent<pauseMenu>().Lose();
    }

    public static void SetGhostTurn(int ghostIndex)
    {
        ghostFinished[ghostIndex] = false;

        endTurn = true;
        for(int i = 0; i < noOfGhost; i++)
        {
            if (ghostFinished[i])
            {
                endTurn = false;
            }
        }
        if (endTurn)
        {
            playerTurn = true;
        }
    }
    
    public static void SetPlayerTurn()
    {
        playerTurn = false;
        for(int i = 0; i < noOfGhost; i++)
        {
            ghostFinished[i] = true;
        }
    }

    public static int AddGhost()
    {
        ghostFinished.Add(false);
        noOfGhost++;
        //Debug.Log(noOfGhost);
        return (noOfGhost - 1);
    }

    public static void Restart()
    {
        playerTurn = true;
        GameOver = false;
        Win = false;
        noOfGhost = 0;
        ghostFinished.Clear();
    }
}
