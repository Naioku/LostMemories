using Stats;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class PlayerStatsUI : MonoBehaviour
    {
        [SerializeField] private Slider jumpBar;

        private void Awake()
        {
            PlayerStats.GetPlayerStats().JumpBarValueChangeEvent += UpdateJumpBar;
        }

        private void UpdateJumpBar(float value)
        {
            // jumpBar.value = value;
        }
    }
}
