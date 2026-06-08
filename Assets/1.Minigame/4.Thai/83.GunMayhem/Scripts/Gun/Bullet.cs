// using System.Collections;
// using System.Collections.Generic;
// using DG.Tweening;
// using UnityEngine;

// namespace Minigame.GunMayhem
// {
//     public class Bullet : MonoBehaviour
//     {
//         public Gun gun;
//         protected Rigidbody2D rb;
//         private bool isShoot = false;

//         private void Awake()
//         {
//             ScenePlay.Instance.ScaleObj(gameObject);
//             rb = GetComponent<Rigidbody2D>();
//         }

//         public virtual void Show()
//         {
//             Destroy(gameObject, 3f);
//         }

//         private void OnDestroy()
//         {
//             DOTween.Kill(transform);
//         }

//         protected virtual void OnTriggerEnter2D(Collider2D collider)
//         {
//             if (collider.gameObject.CompareTag("Player"))
//             {
//                 var player = collider.gameObject.GetComponent<Character>();
//                 if (player.team == gun.character.team)
//                 {
//                     return;
//                 }
//                 if (!isShoot)
//                 {
//                     isShoot = true;
//                     EffectManager.Instance.CreatedEffect("blood", transform);
//                     player.TakeDame(gun.gunSetting.damage * Mathf.Sign(rb.velocity.x));
//                     if (gameObject)
//                     {
//                         Destroy(gameObject);
//                     }
//                 }
//             }
//         }
//     }
// }
