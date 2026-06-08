
using UnityEngine;

namespace Minigame.Battlefield
{
    [System.Serializable]
    public class MoveModule : BaseModule
    {
        public Rigidbody2D body;
        public Vector2 moveDirection;
        public Vector2 rotateDirection;

        public MoveModule(BaseCharacter character) : base(character)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            body = character.rb;
        }

        public void FixedUpdate()
        {
            Move();
            Rotate();
        }

        public void Move()
        {
            body.velocity = moveDirection * character.properties.speed;
        }

        public void Rotate()
        {
            if (rotateDirection.magnitude >= 0.3f)
            {
                RotateFollowVector(rotateDirection);
            }
        }

        private void RotateFollowVector(Vector2 v)
        {
            float rotateZ = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            character.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(character.transform.rotation.eulerAngles.z, character.transform.rotation.eulerAngles.z + Mathf.DeltaAngle(character.transform.rotation.eulerAngles.z, rotateZ), 0.5f));
        }
    }
}