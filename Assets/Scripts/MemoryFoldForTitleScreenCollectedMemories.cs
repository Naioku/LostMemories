using UnityEngine;
using UnityEngine.UI;

public class MemoryFoldForTitleScreenCollectedMemories
{
    private readonly GameObject _viewGameObj;
    private readonly Image _imageSlot;
    private readonly Sprite _imageSpriteClosed;
    private readonly Sprite _imageSpriteOpened;

    public MemoryFoldForTitleScreenCollectedMemories(
        GameObject viewGameObj,
        Image imageSlot,
        Sprite imageSpriteClosed,
        Sprite imageSpriteOpened)
    {
        _viewGameObj = viewGameObj;
        _imageSlot = imageSlot;
        _imageSpriteClosed = imageSpriteClosed;
        _imageSpriteOpened = imageSpriteOpened;
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
}