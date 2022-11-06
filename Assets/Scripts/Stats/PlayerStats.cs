using System;
using System.Collections;
using UnityEngine;

namespace Stats
{
    public class PlayerStats : MonoBehaviour
    {
        public event Action<float> JumpBarValueChangeEvent;
        
        [SerializeField] private float jumpBarLoadingSpeed = 2;

        public float _jumpBarValue;
        private Coroutine _jumpBarCoroutine;

        public bool IsJumpBarLoaded => Mathf.Approximately(_jumpBarValue, 1);
        public bool IsJumpBarUnloaded => Mathf.Approximately(_jumpBarValue, 0);

        public static PlayerStats GetPlayerStats()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<PlayerStats>();
        }
        
        public void ResetJumpBar()
        {
            _jumpBarValue = 0;
            UpdateUI();
        }

        public void LoadJumpBar()
        {
            StopJumpBar();

            _jumpBarCoroutine = StartCoroutine(JumpLoadingCoroutine());
        }
        
        public void UnloadJumpBar()
        {
            StopJumpBar();

            _jumpBarCoroutine = StartCoroutine(JumpUnloadingCoroutine());
        }
        
        public void StopJumpBar()
        {
            if (_jumpBarCoroutine != null)
            {
                StopCoroutine(_jumpBarCoroutine);
            }
        }
        
        private void UpdateUI()
        {
            JumpBarValueChangeEvent?.Invoke(_jumpBarValue);
        }

        private IEnumerator JumpLoadingCoroutine()
        {
            while (!IsJumpBarLoaded)
            {
                _jumpBarValue = Mathf.Min(1, _jumpBarValue + Time.deltaTime * jumpBarLoadingSpeed);
                UpdateUI();
                yield return null;
            }
        }
        
        private IEnumerator JumpUnloadingCoroutine()
        {
            while (!IsJumpBarUnloaded)
            {
                _jumpBarValue = Mathf.Max(0, _jumpBarValue - Time.deltaTime * jumpBarLoadingSpeed);
                UpdateUI();
                yield return null;
            }
        }
    }
}
