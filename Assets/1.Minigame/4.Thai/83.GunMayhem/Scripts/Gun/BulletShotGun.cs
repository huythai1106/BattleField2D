// using System.Collections;
// using System.Collections.Generic;
// using DG.Tweening;
// using UnityEngine;


// namespace Minigame.GunMayhem
// {
//     public class BulletShotGun : Bullet
//     {
//         private float timeDestroy;
//         public override void Show()
//         {
//             Vector3 startLocalScale = transform.localScale;
//             transform.localScale *= 0.01f;
//             timeDestroy = Random.Range(0.1f, 0.11f);
//             transform.DOScale(startLocalScale * Random.Range(0.8f, 1.1f), timeDestroy - 0.01f);

//             Destroy(gameObject, timeDestroy);
//         }
//     }
// }

