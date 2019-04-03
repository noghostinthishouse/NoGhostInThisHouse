using UnityEngine;
using UnityEngine.SceneManagement;

public class startLevelButton : MonoBehaviour
{
    public void startLevel()
    {
        PlayerTurn.Restart();
        SceneManager.LoadScene("Level_Protototype");
        Debug.Log("HEEErer");
    }
}
