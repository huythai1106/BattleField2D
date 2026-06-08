// using System.Collections;
// using System.Collections.Generic;
// using Spine;
// using UnityEngine;

// namespace Minigame.GunMayhem
// {
//     public class Pistol : Gun
//     {
//         public override void DropGun()
//         {
//             character.state.anim.state.SetAnimation(1, "Mayhem/Weapon", false);
//         }

//         public void ThrowPistol()
//         {
//             ActivateColorGun();
//             rb = gameObject.AddComponent<Rigidbody2D>();
//             rb.velocity = new Vector2(character.direction * 5, 5);
//             rb.AddTorque(-character.direction * 300, ForceMode2D.Force);

//             var box = gameObject.AddComponent<BoxCollider2D>();
//             box.isTrigger = true;

//             Destroy(gameObject, 3f);
//             character.DropGun();
//             // character.state.anim.state.AddEmptyAnimation(1, 0.5f, 0f);
//         }

//         private void OnTriggerEnter2D(Collider2D other)
//         {
//             if (other.gameObject.CompareTag("Player"))
//             {
//                 var player = other.GetComponent<Character>();

//                 if (player?.team == character.team)
//                 {
//                     return;
//                 }

//                 EffectManager.Instance.CreatedEffect("blood", transform);
//                 player.TakeDame(gunSetting.damage * Mathf.Sign(rb.velocity.x));
//                 if (gameObject != null)
//                 {
//                     Destroy(gameObject);
//                 }
//             }
//         }
//     }
// }
