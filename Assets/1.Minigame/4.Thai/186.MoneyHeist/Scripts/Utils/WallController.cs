// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Thai.Lib;
// using UnityEngine;

// namespace Minigame.MoneyHeist
// {
//     public class WallController : MonoBehaviour
//     {
//         public float health;
//         public WallController[] wallChildren;
//         public bool isVault;
//         public bool isGrass;
//         public event Action OnHealthChange;
//         public AudioClip breakSound;

//         public void TakeDamage(float damage)
//         {
//             health -= damage;

//             OnHealthChange?.Invoke();

//             if (health <= 0)
//             {
//                 Break();
//             }
//         }

//         public void Break()
//         {
//             if (breakSound != null)
//             {
//                 SoundManager.instance.PlaySoundEffect(breakSound);
//             }
//             else
//             {
//                 SoundManager.instance.PlaySoundEffect("breakWall");
//             }

//             gameObject.SetActive(false);

//             if (isVault)
//             {
//                 GameManager.Instance.BreakVaultDoor.Value = true;
//             }

//             for (int i = 0; i < wallChildren.Length; i++)
//             {
//                 wallChildren[i].Break();
//             }

//             if (isGrass)
//             {
//                 EffectManager.Instance.CreatedEffect("grassBreak", transform);
//             }
//             else
//             {
//                 EffectManager.Instance.CreatedEffect("rockHitWall", transform);
//             }

//             GameManager.Instance.astarPath.Scan();
//         }
//     }
// }
