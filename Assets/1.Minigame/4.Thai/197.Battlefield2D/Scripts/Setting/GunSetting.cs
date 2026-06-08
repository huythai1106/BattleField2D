using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Battlefield
{
    [System.Serializable]
    public struct GunProperties
    {
        public int totalBullet;
        public int bulletRemainingInMage;
        public float reloadTime;
    }
    public enum StatusWeapon
    {
        Ready,
        Waiting
    }

    [CreateAssetMenu(menuName = "2Player/Battlefield/Gun")]
    public class GunSetting : WeaponSetting
    {
        public string nameSlot;
        public float fireRate;
        public float recoil;
        public float reloadTime;
        public int sizeClip;
        public int numberOfTotalBullet;
        public float weight;
        public float speedBullet;
        public float rangePoint;
        public AudioClip[] soundFire;
        public AudioClip soundReload;
    }
}
