using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Thai.Lib;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class Rocket : WeaponBase
    {
        public TeamType teamType;
        public float timeDrop;
        public float radius;
        public Transform sprite;
        public float scaleSprite;

        private void Start()
        {
            Drop();
        }

        private void Drop()
        {
            sprite.DOScale(scaleSprite, 1f);
            Invoke(nameof(Explosion), timeDrop);

            SoundManager.instance.PlaySoundEffect("bombDrop");
        }

        private void Explosion()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, GameManager.Instance.layerPlayer);
            EffectManager.Instance.CreatedEffect("expRocket", transform);
            SoundManager.instance.PlaySoundEffectList("bombExp");

            SourceDamage source = new SourceDamage()
            {
                sourceCharacter = null,
                weaponName = setting.weaponName,
            };

            foreach (var item in colliders)
            {
                BaseCharacter c = item.GetComponent<BaseCharacter>();

                if (c.team.teamType != teamType)
                {
                    c.health.TakenDamage(source);
                }
            }

            CheckIsInCam();

            Destroy(gameObject);
        }

        private void CheckIsInCam()
        {
            foreach (var item in GameManager.Instance.cameras)
            {
                if (Common.IsInView(item, transform.position))
                {
                    // item.GetComponent<SlingAnimal.ShakeCamera>().HandleShakeCamera();
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
