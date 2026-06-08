using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager Instance;
        public WeaponBase[] guns;
        public Dictionary<WeaponName, WeaponBase> itemDict = new();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            Setup();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Setup()
        {
            foreach (var item in guns)
            {
                itemDict.Add(item.setting.weaponName, item);
            }
        }

        public WeaponBase CreateItem(WeaponName itemName)
        {
            if (itemDict.TryGetValue(itemName, out WeaponBase item))
            {
                return Instantiate(item);
            }
            return null;
        }
    }
}
