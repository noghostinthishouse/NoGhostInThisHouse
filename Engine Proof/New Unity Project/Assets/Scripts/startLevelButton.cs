using UnityEngine;
using UnityEngine.SceneManagement;

public class startLevelButton : MonoBehaviour
{
    public void startLevel()
    {
        SceneManager.LoadScene("Level_Protototype");
        Debug.Log("HEEErer");
    }
}
