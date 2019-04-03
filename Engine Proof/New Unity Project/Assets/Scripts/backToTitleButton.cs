using UnityEngine;
using UnityEngine.SceneManagement;

public class backToTitleButton : MonoBehaviour
{
    public void goBackToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
