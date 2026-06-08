using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Battlefield
{
    [CreateAssetMenu(menuName = "2Player/Battlefield/Weapon")]
    public class WeaponSetting : ScriptableObject
    {
        public WeaponName weaponName;
        public float damage;
        public Sprite imgWhite;
        public Sprite weaponSprite;
        public float scaleOnNotification;
    }
}
