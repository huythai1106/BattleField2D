// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


// namespace Minigame.GunMayhem
// {
//     public class Grenade : MonoBehaviour
//     {
//         private Rigidbody2D rb;
//         private void Awake()
//         {
//             rb = GetComponent<Rigidbody2D>();
//             Invoke(nameof(SetExplosion), 2f);
//         }

//         public void ThowNade(float direct)
//         {
//             rb.velocity = new Vector2(direct * 3, 3);
//             SoundManager.instance.PlaySoundEffect("gmh_grenade");
//         }

//         private void SetExplosion()
//         {
//             Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3);
//             foreach (var item in colliders)
//             {
//                 Character c1 = item.GetComponent<Character>();
//                 if (c1)
//                 {
//                     CaculateExplostion(c1);
//                 }
//             }
//             // CaculateExplostion(GameManager.Instance.player1.character);
//             // CaculateExplostion(GameManager.Instance.player2.character);
//             EffectManager.Instance.CreatedEffect("grenadeExplosion", transform);
//             Destroy(gameObject, 0.05f);
//         }

//         private void CaculateExplostion(Character t)
//         {
//             var distance = Mathf.Max(Vector2.Distance(transform.position, t.transform.position), 1);
//             var direct = (t.transform.position - transform.position).normalized;

//             if (distance < 3)
//             {
//                 t.TakeDame(2000 / distance, direct);
//             }
//         }
//     }
// }
