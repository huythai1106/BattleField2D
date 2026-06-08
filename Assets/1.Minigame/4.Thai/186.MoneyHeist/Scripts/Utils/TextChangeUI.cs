using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Minigame.MoneyHeist
{
    public class TextChangeUI : MonoBehaviour
    {
        public string[] listTexts;
        public TextMeshProUGUI textUI;
        public float timeChange = 0.5f;

        private int index = 0;

        private void Start()
        {
            InvokeRepeating(nameof(ChangeText), 0.5f, timeChange);
        }

        private void ChangeText()
        {
            textUI.text = listTexts[index % listTexts.Length];
            index++;
        }
    }
}
