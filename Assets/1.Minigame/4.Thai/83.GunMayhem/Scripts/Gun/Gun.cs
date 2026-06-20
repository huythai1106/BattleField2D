// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Spine;
// using UnityEngine;
// using UnityEngine.UIElements;

// namespace Minigame.GunMayhem
// {
//     public enum StatusGun
//     {
//         Ready,
//         Waiting
//     }

//     public delegate void Callback();

//     public class Gun : MonoBehaviour
//     {
//         [SerializeField] protected Transform firePos;
//         protected Rigidbody2D rb;

//         public GunSetting gunSetting;
//         public Character character;
//         public int amountBullet;
//         protected float timeBtwFire = 0.5f;
//         public bool isShooting = false;

//         protected StatusGun statusGun = StatusGun.Waiting;

//         private void Awake()
//         {
//             ScenePlay.Instance.ScaleObj(gameObject);
//             timeBtwFire = gunSetting.fireRate;
//             amountBullet = gunSetting.ammoCapacity;
//             DeactivateColorGun();
//             StartCoroutine(ResetBulletGun(Mathf.Min(0.5f, timeBtwFire)));
//         }

//         public virtual void Fire()
//         {
//             if (statusGun == StatusGun.Ready && amountBullet > 0)
//             {
//                 amountBullet--;
//                 timeBtwFire = gunSetting.fireRate;
//                 character.playerUI?.UpdateUI(TextUI.Bullet, character.currentGun.amountBullet.ToString());
//                 // if (gunSetting.audioClips) SoundManager.instance.PlaySoundEffect(gunSetting.audioClip);
//                 PlaySoundEffectShoot();

//                 GunManager.Instance.CreateCartouche(character.pointHoldGun.position);
//                 FireBullet();
//                 SetAnimationShoot();
//                 StartCoroutine(ResetBulletGun());

//                 if (amountBullet == 0)
//                 {
//                     // drop gun
//                     DropGun();
//                 }
//                 statusGun = StatusGun.Waiting;
//             }
//         }

//         private void PlaySoundEffectShoot()
//         {
//             if (gunSetting.audioClips.Length > 0)
//             {
//                 SoundManager.instance.PlaySoundEffect(gunSetting.audioClips[UnityEngine.Random.Range(0, gunSetting.audioClips.Length)]);
//             }
//         }

//         protected virtual void FireBullet()
//         {
//             TurnOnMuzzuleFlash();
//             CreateBullet();
//         }

//         public void TurnOnMuzzuleFlash()
//         {
//             firePos.gameObject.GetComponent<SpriteRenderer>().sprite = GunManager.Instance.muzzleFlashs[UnityEngine.Random.Range(0, GunManager.Instance.muzzleFlashs.Length)];
//             CancelInvoke(nameof(TurnOffMuzzuleFlash));
//             Invoke(nameof(TurnOffMuzzuleFlash), 0.1f);
//             character.GetComponent<Rigidbody2D>().AddForce(new Vector2(character.direction * gunSetting.gunRecoil, 0));
//         }

//         public void SetAnimationShoot()
//         {
//             switch (gunSetting.typeGun)
//             {
//                 case TypeGun.SingleShot:
//                     {
//                         TrackEntry track = character.state.anim.state.SetAnimation(1, $"Mayhem/Shoot_Weapon/Shoot_{Enum.GetName(typeof(NameGun), character.currentGun.gunSetting.nameGun)}", false);
//                         track.TimeScale = Mathf.Max(0.333f / gunSetting.fireRate, 1);
//                         break;
//                     }
//                 case TypeGun.Machine:
//                     {
//                         if (!isShooting)
//                         {
//                             character.state.anim.state.SetAnimation(1, $"Mayhem/Shoot_Weapon/Shoot_{Enum.GetName(typeof(NameGun), character.currentGun.gunSetting.nameGun)}", true);
//                             isShooting = true;
//                         }
//                         // character.state.anim.state.SetAnimation(1, $"Mayhem/Shoot_Weapon/Shoot_{Enum.GetName(typeof(NameGun), character.currentGun.gunSetting.nameGun)}", false);
//                         break;
//                     }
//             }
//         }

//         public void CreateBullet(float offset = 0)
//         {
//             Bullet b = Instantiate(gunSetting.bulletPrefab, firePos.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + offset));
//             b.transform.localScale = new Vector3(character.direction * b.transform.localScale.x, b.transform.localScale.y, b.transform.localScale.z);
//             b.gun = this;
//             offset += character.direction == 1 ? 0 : 180;
//             // Vector2 f = new (character.direction *  Mathf.Cos(offset * Mathf.Deg2Rad), Mathf.Sin(offset * Mathf.Deg2Rad));
//             b.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(offset * Mathf.Deg2Rad), Mathf.Sin(offset * Mathf.Deg2Rad)) * gunSetting.bulletSpeed;

//             if (b is BulletBazooka)
//             {
//                 (b as BulletBazooka).rocketTrail.transform.localScale = new Vector3((b as BulletBazooka).rocketTrail.transform.localScale.x * character.direction, (b as BulletBazooka).rocketTrail.transform.localScale.y, (b as BulletBazooka).rocketTrail.transform.localScale.z);
//             }

//             b.Show();
//         }

//         protected void TurnOffMuzzuleFlash()
//         {
//             firePos.gameObject.GetComponent<SpriteRenderer>().sprite = null;
//         }

//         protected IEnumerator ResetBulletGun()
//         {
//             yield return new WaitForSeconds(timeBtwFire);
//             statusGun = StatusGun.Ready;
//         }

//         protected IEnumerator ResetBulletGun(float timeDelay)
//         {
//             yield return new WaitForSeconds(timeDelay);
//             statusGun = StatusGun.Ready;
//         }

//         public virtual void DropGun()
//         {
//             ActivateColorGun();
//             rb = gameObject.AddComponent<Rigidbody2D>();
//             rb.velocity = new Vector2(character.direction * 3, 1);
//             Destroy(gameObject, 3f);
//             character.DropGun();
//         }

//         public void ActivateColorGun()
//         {
//             if (this != null)
//                 GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
//         }

//         public void DeactivateColorGun()
//         {
//             if (this != null)
//                 GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
//         }
//     }
// }

