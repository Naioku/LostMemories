using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryViewController : MonoBehaviour
{
    [Header("GreenFold")]
    [SerializeField] private GameObject greenFoldGameObject;
    [SerializeField] private Image greenFoldImageSlot;
    [SerializeField] private Sprite greenFoldClosed;
    [SerializeField] private Sprite greenFoldOpened;
    [SerializeField] private List<GameObject> greenContent;

    [Header("VioletFold")]
    [SerializeField] private GameObject violetFoldGameObject;
    [SerializeField] private Image violetFoldImageSlot;
    [SerializeField] private Sprite violetFoldClosed;
    [SerializeField] private Sprite violetFoldOpened;
    [SerializeField] private List<GameObject> violetContent;

    [Header("RedFold")]
    [SerializeField] private GameObject redFoldGameObject;
    [SerializeField] private Image redFoldImageSlot;
    [SerializeField] private Sprite redFoldClosed;
    [SerializeField] private Sprite redFoldOpened;
    [SerializeField] private List<GameObject> redContent;

    private readonly List<MemoryFold> _folds = new List<MemoryFold>();
    private int _currentIndex;
    private Canvas _canvasComponent;
    private WinMenuCanvasController _winMenuCanvasController;

    private void Awake()
    {
        _winMenuCanvasController = FindObjectOfType<WinMenuCanvasController>();
    }

    private void Start()
    {
        var greenFold = new MemoryFold(
            greenFoldGameObject,
            greenFoldImageSlot,
            greenFoldClosed,
            greenFoldOpened,
            greenContent);

        var violetFold = new MemoryFold(
            violetFoldGameObject,
            violetFoldImageSlot,
            violetFoldClosed,
            violetFoldOpened,
            violetContent);

        var redFold = new MemoryFold(
            redFoldGameObject,
            redFoldImageSlot,
            redFoldClosed,
            redFoldOpened,
            redContent);
        
        _folds.Add(greenFold);
        _folds.Add(violetFold);
        _folds.Add(redFold);
        
        _canvasComponent = GetComponent<Canvas>();
        
        SetScoreValues();
        HideWholeView();
        SwitchViewToTheIndex(_currentIndex);
    }

    public void ShowWholeView()
    {
        _canvasComponent.enabled = true;
    }

    public void HideWholeView()
    {
        _canvasComponent.enabled = false;
    }

    public void SwitchViewToTheRight()
    {
        _currentIndex++;
        FixCurrentIndex();
        SwitchViewToTheIndex(_currentIndex);
    }

    public void SwitchViewToTheLeft()
    {
        _currentIndex--;
        FixCurrentIndex();
        SwitchViewToTheIndex(_currentIndex);
    }

    public void RefreshScore()
    {
        var sectionsUnlocked = true;
        foreach (var fold in _folds)
        {
            fold.SetUnlockedMemoryPartsQuantity(fold.GetWholeMemorySectionContent().Count(oneContent => oneContent.activeSelf));
            if (!fold.IsWholeSectionUnlocked())
            {
                sectionsUnlocked = false;
            }
        }

        SetScoreValues();
        
        if (sectionsUnlocked)
        {
            _winMenuCanvasController.Show();
            TitleScreenCanvasController.UnlockMemoryCollection();
        }
    }

    private void SetScoreValues()
    {
        foreach (var fold in _folds)
        {
            fold.GetImageSlot().GetComponentInChildren<TextMeshProUGUI>().text = fold.GetUnlockedMemoryPartsQuantity() +
                "/" +
                fold.GetMemoryPartsQuantity();
        }
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
        
        MemoryFold memoryFold = _folds[index];
        ShowFoldView(memoryFold);
        SwitchFoldImageToOpened(memoryFold);
    }

    private static void HideFoldView(MemoryFold fold)
    {
        fold.GetViewGameObject().SetActive(false);
    }

    private static void ShowFoldView(MemoryFold fold)
    {
        fold.GetViewGameObject().SetActive(true);
    }

    private static void SwitchFoldImageToClosed(MemoryFold fold)
    {
        fold.GetImageSlot().sprite = fold.GetImageSpriteClosed();
    }

    private static void SwitchFoldImageToOpened(MemoryFold fold)
    {
        fold.GetImageSlot().sprite = fold.GetImageSpriteOpened();
    }
}
