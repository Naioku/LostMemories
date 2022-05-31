using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryFold
{
    private readonly GameObject _viewGameObj;
    private readonly Image _imageSlot;
    private readonly Sprite _imageSpriteClosed;
    private readonly Sprite _imageSpriteOpened;
    private readonly int _memoryPartsQuantity;
    private readonly List<GameObject> _wholeMemorySectionContent;
    private int _unlockedMemoryPartsQuantity;

    public MemoryFold(
        GameObject viewGameObj,
        Image imageSlot,
        Sprite imageSpriteClosed,
        Sprite imageSpriteOpened,
        List<GameObject> wholeMemorySectionContent)
    {
        _viewGameObj = viewGameObj;
        _imageSlot = imageSlot;
        _imageSpriteClosed = imageSpriteClosed;
        _imageSpriteOpened = imageSpriteOpened;
        _wholeMemorySectionContent = wholeMemorySectionContent;
        _memoryPartsQuantity = wholeMemorySectionContent.Count;
    }

    public GameObject GetViewGameObject()
    {
        return _viewGameObj;
    }
    
    public Image GetImageSlot()
    {
        return _imageSlot;
    }
    
    public Sprite GetImageSpriteClosed()
    {
        return _imageSpriteClosed;
    }
    
    public Sprite GetImageSpriteOpened()
    {
        return _imageSpriteOpened;
    }
    
    public List<GameObject> GetWholeMemorySectionContent()
    {
        return _wholeMemorySectionContent;
    }
    
    public int GetMemoryPartsQuantity()
    {
        return _memoryPartsQuantity;
    }
    
    public int GetUnlockedMemoryPartsQuantity()
    {
        return _unlockedMemoryPartsQuantity;
    }
    
    public void SetUnlockedMemoryPartsQuantity(int value)
    {
        _unlockedMemoryPartsQuantity = value;
    }

    public bool IsWholeSectionUnlocked()
    {
        return _unlockedMemoryPartsQuantity.Equals(_wholeMemorySectionContent.Count);
    }
}