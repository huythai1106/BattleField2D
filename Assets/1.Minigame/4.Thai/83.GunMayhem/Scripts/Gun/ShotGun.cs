// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace Minigame.GunMayhem
// {
//     public class ShotGun : Gun
//     {
//         protected override void FireBullet()
//         {
//             // base.FireBullet();
//             TurnOnMuzzuleFlash();

//             float step = 2f;
//             for (int i = 0; i < 4; i++)
//             {
//                 CreateBullet(i * step);
//             }

//             for (int i = 0; i < 4; i++)
//             {
//                 CreateBullet(-i * step);
//             }
//         }
//     }
// }
