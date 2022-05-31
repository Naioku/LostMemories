using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuCanvasController : MonoBehaviour
{
    public void OnPlay()
    {
        SceneManager.LoadScene("Level");
    }

    public void OnTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
