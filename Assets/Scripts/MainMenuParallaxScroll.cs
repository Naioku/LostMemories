using UnityEngine;
using UnityEngine.UI;

public class MainMenuParallaxScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float creditsSpeed;

    private Material _material;
    private Transform _children;
    
    void Awake()
    {
        _material = GetComponent<Image>().material;
        _children = transform.GetChild(0);
    }

    void Update()
    {
        _material.mainTextureOffset += new Vector2(0f, -scrollSpeed * Time.deltaTime);
        _children.localPosition += (Vector3) new Vector2(0f,creditsSpeed * Time.deltaTime);
    }

    public void ResetOffset()
    {
        _material.mainTextureOffset = new Vector2(0f, 0f);
    }
}
