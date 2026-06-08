using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.MoneyHeist
{
    // public class SourceDamage
    // {
    //     public BaseCharacter sourceCharacter;
    //     public ItemName weaponType;

    //     public float GetDamage()
    //     {
    //         return ItemManager.Instance.itemDict[weaponType].itemSetting.damage;
    //     }
    // }

    public enum TypePlayer
    {
        CounterTerrorist,
        Terrorist,
    }

    public enum WeaponType
    {
        MainGun,
        Pistol,
        Grenade,
        Molotov,
        Armor,
    }

    public enum ItemName
    {
        AK47,
        Aug,
        M16,
        Mini14,
        Scal,
        S686,
        SawedOff,
        DE,
        Usp,
        Ump,
        Uzi,
        Vector,
        TommyGun,
        Armor,
        ArmorAndHelmet,
        Genade,
        Smoke,
        Molotov,
        M24,
        Gatling,
    }

    public enum Category
    {
        Equipment,
        Pistol,
        MidTier,
        Rifle,
    }

    public enum ItemType
    {
        Equiment,
        Nade,
        Weapon,
    }

    public static class Common
    {
        public const string ITEM_TAG = "Food";
        public const string PLAYER_TAG = "Player";
        public const string BULLET_TAG = "Bullet";
        public const string WALL_TAG = "Wall";
        public const string BOX_TAG = "";

        public static string FormatMinutesToTime(int totalMinutes)
        {
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;
            return $"{hours}:{minutes:D2}";
        }
    }
}
