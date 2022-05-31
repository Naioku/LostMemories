using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenuCanvasController : MonoBehaviour
{
    [SerializeField] private float secondToWaitBeforeStoppingCredits;

    private Coroutine _coroutine;
    private MainMenuParallaxScroll _mainMenuParallaxScroll;

    private void Awake()
    {
        _mainMenuParallaxScroll = FindObjectOfType<MainMenuParallaxScroll>();
    }

    private void OnAbortCredits()
    {
        if (_coroutine != null) return;
        
        _coroutine = StartCoroutine(ReturnToTitleScreen());
    }

    private IEnumerator ReturnToTitleScreen()
    {
        yield return new WaitForSecondsRealtime(secondToWaitBeforeStoppingCredits);
        SceneManager.LoadScene("TitleScreen");
        _mainMenuParallaxScroll.ResetOffset();
    }
}
