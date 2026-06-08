using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class HealthUI : MonoBehaviour
    {
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI armorText;

        public void RegisterFunc(BaseCharacter character)
        {
            if (character == null) return;

            character.properties.health.OnValueChanged += UpdateHealth;
            character.properties.armor.OnValueChanged += UpdateArrmor;

            UpdateHealth(character.properties.health.Value);
            UpdateArrmor(character.properties.armor.Value);
        }

        private void UpdateHealth(float value)
        {
            healthText.text = Mathf.Ceil(value).ToString();
        }

        private void UpdateArrmor(float value)
        {
            armorText.text = Mathf.Ceil(value).ToString();
        }
    }
}
