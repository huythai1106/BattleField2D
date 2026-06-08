using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class BulletUI : MonoBehaviour
    {
        public TextMeshProUGUI bulletText;

        public void RegisterFunc(BaseCharacter character)
        {
            if (character == null) return;

            character.bulletValue.OnValueChanged += UpdateBullet;
            UpdateBullet(character.bulletValue.Value);
        }

        private void UpdateBullet(string value)
        {
            bulletText.text = value;
        }
    }
}
