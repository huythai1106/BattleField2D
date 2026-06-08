using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class TimePlayUI : MonoBehaviour
    {
        public TextMeshProUGUI time;

        private void Start()
        {
            GameManager.Instance.OnPlayGameStart += () =>
            {
                GameManager.Instance.remainingTimePlaying.OnValueChanged += UpdateTimePlaying;

                UpdateTimePlaying(GameManager.Instance.remainingTimePlaying.Value);
            };
        }

        private void UpdateTimePlaying(int value)
        {
            time.text = Common.FormatMinutesToTime(value);
        }
    }
}
