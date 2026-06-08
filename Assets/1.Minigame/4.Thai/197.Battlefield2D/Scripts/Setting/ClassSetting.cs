using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Battlefield
{
    [CreateAssetMenu(menuName = "2Player/Battlefield/ClassSetting")]
    public class ClassSetting : ScriptableObject
    {
        public ClassPlayer classPlayer;
        public bool isLimitBullet;
        public WeaponName[] weapons;
        public float scope;
        public Sprite icon;
        public float armor;
    }
}
