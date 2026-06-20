using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class Gun : WeaponBase
    {
        public GunSetting GunSetting => setting as GunSetting;

        public BaseCharacter characterOwner;
        public Coroutine reload;
        public bool isReloading;
        internal float recoil = 0;

        public StatusWeapon statusWeapon = StatusWeapon.Ready;
        protected float timeDelay;
        protected Coroutine delayStatusWeapon = null;
        public GunProperties gunProperties;
        public bool isShogun;

        [Header("FlameThrower")]
        public bool isFlameThrower;

        private void Start()
        {
            timeDelay = GunSetting.fireRate;
            gunProperties.bulletRemainingInMage = GunSetting.sizeClip;
            gunProperties.totalBullet = GunSetting.numberOfTotalBullet;
            gunProperties.reloadTime = GunSetting.reloadTime;

            SetBulletValue();
        }

        private void FixedUpdate()
        {
            if (recoil > 0)
            {
                recoil = Mathf.Max(recoil - 10f * Time.fixedDeltaTime, 0);
            }
        }

        public virtual void OnAttack()
        {
            if (statusWeapon == StatusWeapon.Ready)
            {
                // Fire
                Fire();

                // reset
                if (delayStatusWeapon != null)
                {
                    StopCoroutine(delayStatusWeapon);
                    delayStatusWeapon = null;
                }

                delayStatusWeapon = GameManager.Instance.StartCoroutine(ResetStatusWeapon());
                statusWeapon = StatusWeapon.Waiting;
            }
        }

        protected virtual IEnumerator ResetStatusWeapon()
        {
            yield return new WaitForSeconds(timeDelay);
            statusWeapon = StatusWeapon.Ready;
        }

        protected virtual void Fire()
        {
            if (GameManager.Instance.isFinishGame) return;

            if (!characterOwner) return;
            if (characterOwner.CheckWall()) return;

            if (gunProperties.bulletRemainingInMage > 0 && !isReloading)
            {
                gunProperties.bulletRemainingInMage--;
                if (characterOwner.classSetting.isLimitBullet && GameManager.Instance.currentMap.typeMap != TypeMap.Defend)
                {
                    gunProperties.totalBullet--;
                }

                SetBulletValue();

                if (!isFlameThrower)
                {
                    characterOwner.muzzleFlash?.Play();
                }

                characterOwner.state.PlayAnimShoot("Shoot_Weapon_0");
                PlaySoundEffect();

                if (isShogun)
                {
                    CreateBulletShotgun();
                }
                else if (isFlameThrower)
                {
                    var cols = characterOwner.flameCrl.CheckRange();
                    var sourceDmg = new SourceDamage()
                    {
                        sourceCharacter = characterOwner,
                        weaponName = WeaponName.FlameThrower,
                    };

                    foreach (var item in cols)
                    {
                        var p = item.collider.GetComponent<BaseCharacter>();

                        if (p.team == characterOwner.team) continue;
                        else
                        {
                            p.health.TakenDamage(sourceDmg);
                        }
                    }
                }
                else
                {
                    CreateBullet();
                }

                recoil = Mathf.Min(recoil + GunSetting.recoil / 5, GunSetting.recoil);
            }
            else
            {
                AcitveReload();
            }
        }

        public void PlaySoundEffect()
        {
            if (GameManager.Instance.isFinishGame) return;

            if (GunSetting.soundFire.Length != 0)
            {
                SoundManager.instance.PlaySoundEffect(GunSetting.soundFire[Random.Range(0, GunSetting.soundFire.Length)]);
            }
        }

        protected virtual void CreateBullet()
        {
            var s = new SourceDamage()
            {
                sourceCharacter = characterOwner,
                weaponName = GunSetting.weaponName,
            };
            GameManager.Instance.CreateBullet(s, characterOwner.gunPoint.position, characterOwner.gunPoint.right, recoil, GunSetting.speedBullet);
        }

        protected virtual void CreateBulletShotgun()
        {
            var s = new SourceDamage()
            {
                sourceCharacter = characterOwner,
                weaponName = GunSetting.weaponName,
            };

            for (int i = 0; i < 7; i++)
            {
                GameManager.Instance.CreateBullet(s, characterOwner.gunPoint.position, characterOwner.gunPoint.right, speed: GunSetting.speedBullet, offset: Random.Range(-35, 35));
            }
            GameManager.Instance.CreateCartouche(characterOwner.gunPoint.position);
        }

        public void AcitveReload()
        {
            if (isReloading || GameManager.Instance.isFinishGame) return;

            isReloading = true;

            if (GunSetting.sizeClip == gunProperties.bulletRemainingInMage || gunProperties.totalBullet <= 0)
            {
                Debug.Log("Not reload");
                isReloading = false;
                return;
            }

            reload = StartCoroutine(Reloading());
        }

        public void StopReload()
        {
            isReloading = false;
            if (reload != null)
            {
                StopCoroutine(reload);
            }
            reload = null;

            if (characterOwner.controller is HumanController)
            {
                (characterOwner.controller as HumanController).reloadUI?.TurnOffReloading();
            }
        }

        private IEnumerator Reloading()
        {
            if (characterOwner.classSetting.isLimitBullet && gunProperties.totalBullet <= 0)
            {
                yield break;
            }

            SoundManager.instance.PlaySoundEffect("reload");
            Debug.Log("Reload");

            if (characterOwner.controller is HumanController)
            {
                (characterOwner.controller as HumanController).reloadUI?.TurnOnReloading(gunProperties.reloadTime);
            }

            yield return new WaitForSeconds(gunProperties.reloadTime);
            isReloading = false;

            if (characterOwner.classSetting.isLimitBullet && GameManager.Instance.currentMap.typeMap != TypeMap.Defend)
            {
                var offset = gunProperties.totalBullet - GunSetting.sizeClip;

                if (offset <= 0)
                {
                    gunProperties.bulletRemainingInMage = gunProperties.totalBullet;
                }
                else
                {
                    gunProperties.bulletRemainingInMage = GunSetting.sizeClip;
                }
            }
            else
            {
                gunProperties.bulletRemainingInMage = GunSetting.sizeClip;
            }

            SetBulletValue();

            // characterOwner.bulletInfo.Value = gunProperties.bulletRemainingInMage.ToString();
            if (characterOwner.controller is HumanController)
            {
                (characterOwner.controller as HumanController).reloadUI?.TurnOffReloading();
            }
        }

        public void SetBulletValue()
        {
            if (characterOwner.classSetting.isLimitBullet && GameManager.Instance.currentMap.typeMap != TypeMap.Defend)
            {
                characterOwner.bulletValue.Value = $"{gunProperties.bulletRemainingInMage}/{gunProperties.totalBullet - gunProperties.bulletRemainingInMage}";
            }
            else
            {
                characterOwner.bulletValue.Value = gunProperties.bulletRemainingInMage.ToString();
            }
        }

        public void RecoverBullet()
        {
            gunProperties.totalBullet = GunSetting.numberOfTotalBullet;

            SetBulletValue();
        }
    }
}
