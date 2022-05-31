using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollectedMemoriesCanvasController : MonoBehaviour
{
    [Header("GreenFold")]
    [SerializeField] private GameObject greenFoldGameObject;
    [SerializeField] private Image greenFoldImageSlot;
    [SerializeField] private Sprite greenFoldClosed;
    [SerializeField] private Sprite greenFoldOpened;

    [Header("VioletFold")]
    [SerializeField] private GameObject violetFoldGameObject;
    [SerializeField] private Image violetFoldImageSlot;
    [SerializeField] private Sprite violetFoldClosed;
    [SerializeField] private Sprite violetFoldOpened;

    [Header("RedFold")]
    [SerializeField] private GameObject redFoldGameObject;
    [SerializeField] private Image redFoldImageSlot;
    [SerializeField] private Sprite redFoldClosed;
    [SerializeField] private Sprite redFoldOpened;

    private readonly List<MemoryFoldForTitleScreenCollectedMemories> _folds = new List<MemoryFoldForTitleScreenCollectedMemories>();
    private int _currentIndex;

    void Start()
    {
        var greenFold = new MemoryFoldForTitleScreenCollectedMemories(
            greenFoldGameObject,
            greenFoldImageSlot,
            greenFoldClosed,
            greenFoldOpened);

        var violetFold = new MemoryFoldForTitleScreenCollectedMemories(
            violetFoldGameObject,
            violetFoldImageSlot,
            violetFoldClosed,
            violetFoldOpened);

        var redFold = new MemoryFoldForTitleScreenCollectedMemories(
            redFoldGameObject,
            redFoldImageSlot,
            redFoldClosed,
            redFoldOpened);
        
        _folds.Add(greenFold);
        _folds.Add(violetFold);
        _folds.Add(redFold);
        
        SwitchViewToTheIndex(_currentIndex);
    }

    private void FixCurrentIndex()
    {
        if (_currentIndex >= _folds.Count)
        {
            _currentIndex = 0;
        }
        else if (_currentIndex < 0)
        {
            _currentIndex = _folds.Count - 1;
        }
    }

    private void SwitchViewToTheIndex(int index)
    {
        foreach (var fold in _folds)
        {
            HideFoldView(fold);
            SwitchFoldImageToClosed(fold);
        }
        
        var memoryFold = _folds[index];
        ShowFoldView(memoryFold);
        SwitchFoldImageToOpened(memoryFold);
    }
    
    private static void HideFoldView(MemoryFoldForTitleScreenCollectedMemories fold)
    {
        fold.GetViewGameObject().SetActive(false);
    }

    private static void ShowFoldView(MemoryFoldForTitleScreenCollectedMemories fold)
    {
        fold.GetViewGameObject().SetActive(true);
    }

    private static void SwitchFoldImageToClosed(MemoryFoldForTitleScreenCollectedMemories fold)
    {
        fold.GetImageSlot().sprite = fold.GetImageSpriteClosed();
    }

    private static void SwitchFoldImageToOpened(MemoryFoldForTitleScreenCollectedMemories fold)
    {
        fold.GetImageSlot().sprite = fold.GetImageSpriteOpened();
    }

    private void OnTurnThePage(InputValue value)
    {
        var input = value.Get<Single>();

        _currentIndex += (int) input;
        FixCurrentIndex();
        SwitchViewToTheIndex(_currentIndex);
    }

    private void OnBack()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
