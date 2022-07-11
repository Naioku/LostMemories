using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    [SerializeField] private List<MemoryScrollController> memoryScrolls;
    [SerializeField] private int howManyAtOnce = 1;

    private Queue<MemoryScrollController> _memoryScrollsQueue;

    private void Start()
    {
        _memoryScrollsQueue = new Queue<MemoryScrollController>(memoryScrolls);
    }

    private void OnGetScroll()
    {
        for (int i = 0; i < howManyAtOnce; i++)
        {
            if (_memoryScrollsQueue.Count <= 0) return;
            
            var scroll = _memoryScrollsQueue.Dequeue();
            scroll.GetScrollToPlayer();
        }
    }
}
