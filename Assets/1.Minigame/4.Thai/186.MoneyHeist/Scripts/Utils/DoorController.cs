using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Minigame.MoneyHeist
{
    public class DoorController : MonoBehaviour
    {
        public Transform door;
        private List<Transform> playerInDoor = new();
        private Vector3 startPos;
        private bool isActive;

        private void Start()
        {
            startPos = door.position;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Common.PLAYER_TAG))
            {
                playerInDoor.Add(collision.transform);
                if (!isActive)
                {
                    isActive = true;
                    door.DOLocalMove(new Vector2(-3, 0), 0.5f);
                }
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(Common.PLAYER_TAG))
            {
                playerInDoor.Remove(collision.transform);

                if (playerInDoor.Count == 0)
                {
                    isActive = false;
                    door.DOMove(startPos, 0.5f);
                }
            }
        }
    }
}
