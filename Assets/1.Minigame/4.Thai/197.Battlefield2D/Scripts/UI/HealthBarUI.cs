using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigame.Battlefield
{
    public class HealthBarUI : MonoBehaviour
    {
        public Image healthBarImg;

        public void RegisterFunc(BaseCharacter character)
        {
            if (character == null) return;

            character.properties.health.OnValueChanged += UpdateHealthBar;

            UpdateHealthBar(character.properties.health.Value);
        }

        private void UpdateHealthBar(float value)
        {
            healthBarImg.fillAmount = Mathf.Clamp01(value / 100);
        }
    }
}
