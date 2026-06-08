using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Battlefield
{
    [System.Serializable]
    public class WeaponModule : BaseModule
    {
        public Gun currentGun;

        public WeaponModule(BaseCharacter character) : base(character)
        {
        }

        public void Update()
        {
            if (character.isShooting && currentGun != null)
            {
                currentGun.OnAttack();
            }
        }

        public void TurnOnShooting()
        {
            if (!character.isShooting)
            {
                character.isShooting = true;

                if (character.weapon.currentGun && character.weapon.currentGun.isFlameThrower)
                {
                    character.flameCrl.flameEffect.Play();
                }
            }
        }

        public void TurnOffShooting()
        {
            if (character.isShooting)
            {
                character.isShooting = false;
                if (character.weapon.currentGun && character.weapon.currentGun.isFlameThrower)
                {
                    character.flameCrl.flameEffect.Stop();
                }
            }
        }

        public void EquipWeapon(Gun gun)
        {
            if (currentGun != null)
            {
                Object.Destroy(currentGun.gameObject);
                currentGun = null;
            }

            currentGun = gun;

            gun.characterOwner = character;
            gun.transform.SetParent(character.transform);
            gun.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
            character.gunPoint.transform.localPosition = new(gun.GunSetting.rangePoint, 0, 0);
            character.anim.skeleton.SetAttachment("Weapon_B", gun.GunSetting.nameSlot);
        }
    }
}
