using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class Arrow : MonoBehaviour
    {
        private void Start()
        {
            DOVirtual.DelayedCall(10f, () =>
            {
                GetComponent<SpriteRenderer>().DOFade(0, 1f);
            });
        }
    }
}
