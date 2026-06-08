using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigame.Battlefield
{
    public class InfoUI : MonoBehaviour
    {
        public Image classIcon;
        public Image healthBar;
        public Image armorBar;

        private BaseCharacter character;

        public void RegisterFunc(BaseCharacter character)
        {
            if (character == null) return;
            this.character = character;

            classIcon.sprite = character.classSetting.icon;

            character.properties.health.OnValueChanged += UpdateHealth;
            character.properties.armor.OnValueChanged += UpdateArrmor;

            UpdateHealth(character.properties.health.Value);
            UpdateArrmor(character.properties.armor.Value);
        }

        private void UpdateHealth(float value)
        {
            healthBar.fillAmount = Mathf.Clamp01(value / 100);
        }

        private void UpdateArrmor(float value)
        {
            armorBar.fillAmount = Mathf.Clamp01(value / character.classSetting.armor);
        }
    }
}
