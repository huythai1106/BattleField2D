using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class TutControll : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public TextMeshProUGUI[] texts;

        public void StartTuT()
        {
            Time.timeScale = 0;
            DOVirtual.DelayedCall(4f, () =>
            {
                Time.timeScale = 1;
                canvasGroup.DOFade(0, 1.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    gameObject.SetActive(false);

                });
            }).SetUpdate(true);
        }

        public void SetText(string value)
        {
            foreach (var item in texts)
            {
                item.text = value;
            }
        }
    }
}
