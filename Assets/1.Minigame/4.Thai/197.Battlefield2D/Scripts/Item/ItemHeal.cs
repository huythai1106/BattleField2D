using System.Collections;
using System.Collections.Generic;
using Thai.Lib;
using UnityEngine;

namespace Minigame.Battlefield
{
    public enum TypeItem
    {
        Health,
        Bullet,
    }

    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class ItemHeal : MonoBehaviour
    {
        public TypeItem typeItem;
        protected bool isTrigger = false;

        public Sprite[] img;
        private SpriteRenderer sr;

        private void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            if (typeItem == TypeItem.Health)
            {
                sr.sprite = img[0];
            }
            else
            {
                sr.sprite = img[1];
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(Common.PLAYER_TAG))
            {
                if (collider.gameObject.GetComponent<BaseCharacter>() && !isTrigger)
                {
                    isTrigger = true;

                    var player = collider.GetComponent<BaseCharacter>();
                    ItemCollected(player);

                    GameManager.Instance.StartCoroutine(ResetItem());
                }
            }
        }

        private void ItemCollected(BaseCharacter player)
        {
            if (typeItem == TypeItem.Health)
            {
                EffectManager.Instance.CreatedEffect("healing", player.transform);
                player.health.Health(100);
            }
            else
            {
                EffectManager.Instance.CreatedEffect("armor", player.transform);
                player.weapon.currentGun.RecoverBullet();
            }
        }

        public IEnumerator ResetItem()
        {
            gameObject.SetActive(false);
            yield return new WaitForSeconds(10f);

            isTrigger = false;
            gameObject.SetActive(true);
        }
    }
}
