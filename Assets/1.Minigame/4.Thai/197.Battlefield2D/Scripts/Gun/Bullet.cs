using System.Collections;
using System.Collections.Generic;
using Thai.Lib;
using UnityEngine;

namespace Minigame.Battlefield
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        public SourceDamage source;
        public Rigidbody2D rb;
        public Vector2 size;
        public Vector2 offset;
        public float speedBase = 10f;

        public void Init(SourceDamage damageSouce, Vector2 vectorDir)
        {
            Init(damageSouce, vectorDir, speedBase);
        }

        public void Init(SourceDamage damageSouce, Vector2 vectorDir, float speed)
        {
            rb = GetComponent<Rigidbody2D>();
            source = damageSouce;

            rb.velocity = speed * vectorDir;

            RotateBullet();

            Destroy(gameObject, 1f);
        }

        private void Update()
        {
            CheckHit();
        }

        private void RotateBullet()
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        public void CheckHit()
        {
            var hit = Physics2D.BoxCast(transform.position + (Vector3)offset, size, transform.eulerAngles.z, Vector2.right, 0, GameManager.Instance.layerHit);
            if (!hit.collider) return;

            if (hit.collider.CompareTag(Common.PLAYER_TAG))
            {
                if (hit.collider.TryGetComponent(out BaseCharacter target))
                {
                    // If the bullet hits a player, apply damage
                    if (target.team.teamType != source.sourceCharacter.team.teamType)
                    {
                        EffectManager.Instance.CreatedEffect("beHit", transform);
                        target.health.TakenDamage(source);
                        Destroy(gameObject);
                        return;
                    }
                }
            }
            else if (hit.collider.CompareTag(Common.WALL_TAG))
            {
                // // If the bullet hits a wall, destroy it
                // if (hit.collider.TryGetComponent(out WallController wall))
                // {
                //     wall.TakeDamage(source.GetDamage());
                // }

                EffectManager.Instance.CreatedEffect("hitInWall", transform);
                Destroy(gameObject);
                return;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(transform.position + (Vector3)offset, Quaternion.Euler(0, 0, transform.eulerAngles.z), Vector3.one);
            Gizmos.DrawWireCube(Vector2.zero, size);
        }
    }
}
