// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace Minigame.GunMayhem
// {
//     public class BulletBazooka : Bullet
//     {
//         public ParticleSystem rocketTrail;

//         protected override void OnTriggerEnter2D(Collider2D collider)
//         {
//             if (collider.gameObject.CompareTag("Player"))
//             {
//                 var player = collider.gameObject.GetComponent<Character>();
//                 player.TakeDame(gun.gunSetting.damage * Mathf.Sign(rb.velocity.x), new Vector2(1, 0), gun.gunSetting.nameGun);
//                 Explosion(player);
//             }
//             else if (collider.gameObject.CompareTag("OneWayPlatform") || collider.gameObject.CompareTag("Ground"))
//             {
//                 Explosion();
//             }
//         }

//         public void Explosion(Character c = null)
//         {
//             EffectManager.Instance.CreatedEffect("grenadeExplosion", transform, new() { scaleRate = 1.5f });
//             Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3);

//             foreach (var item in colliders)
//             {
//                 Character c1 = item.GetComponent<Character>();
//                 if (item.CompareTag("Player") && c != c1)
//                 {
//                     var distance = Mathf.Max(Vector2.Distance(transform.position, item.transform.position), 2);
//                     var direct = (item.transform.position - transform.position).normalized;

//                     item.GetComponent<Character>().TakeDame(gun.gunSetting.damage / distance / 2, direct, gun.gunSetting.nameGun);
//                 }
//             }

//             if (gameObject)
//             {
//                 Destroy(gameObject);
//             }
//         }
//     }
// }
