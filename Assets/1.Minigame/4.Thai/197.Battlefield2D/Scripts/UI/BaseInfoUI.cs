using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigame.Battlefield
{
    public class BaseInfoUI : MonoBehaviour
    {
        public Image redBar;
        public RectTransform partical;
        public float width;
        public TextMeshProUGUI infoRed;
        public TextMeshProUGUI infoBlue;

        private void Start()
        {
            GameManager.Instance.OnPlayGameStart += () =>
            {
                GameManager.Instance.currentMap.baseManager.onChangeBase += OnChangeBar;
                ResetBarUI();
            };
        }

        private void OnChangeBar(Base b)
        {
            ResetBarUI();
        }

        private void ResetBarUI()
        {
            var bCrl = GameManager.Instance.currentMap.baseManager;

            int countRed = bCrl.baseRed.Count;
            int countblue = bCrl.baseBlue.Count;

            var value = (float)countRed / (countRed + countblue);
            redBar.fillAmount = value;

            infoRed.text = (value * 100).ToString("F0") + "%";
            infoBlue.text = ((1 - value) * 100).ToString("F0") + "%";

            partical.anchoredPosition = new Vector2((value - 0.5f) * width, partical.anchoredPosition.y);
        }
    }
}
