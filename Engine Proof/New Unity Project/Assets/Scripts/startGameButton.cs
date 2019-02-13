using UnityEngine;
using UnityEngine.SceneManagement;

public class startGameButton : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("StageSelect");
    }
}
