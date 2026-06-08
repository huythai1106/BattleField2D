using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class FlameThrowerControll : MonoBehaviour
    {
        public BoxCollider2D box;
        public ParticleSystem flameEffect;
        public Vector2 size;
        public Vector2 offset;

        public RaycastHit2D[] CheckRange()
        {
            // Lấy thông tin box collider
            float angle = transform.eulerAngles.z;
            Vector2 rotatedOffset = Quaternion.Euler(0, 0, angle) * offset;

            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position + (Vector3)rotatedOffset, size, angle, Vector2.right, 0, GameManager.Instance.layerPlayer);

            return hits;
        }

        private void OnDrawGizmos()
        {
            float angle = transform.eulerAngles.z;
            Vector2 rotatedOffset = Quaternion.Euler(0, 0, angle) * offset;
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(transform.position + (Vector3)rotatedOffset, Quaternion.Euler(0, 0, angle), Vector3.one);
            Gizmos.DrawWireCube(Vector2.zero, size);
        }
    }
}
