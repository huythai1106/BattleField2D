using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Minigame.Battlefield
{
    [System.Serializable]
    public class APIModule : BaseModule
    {
        List<Transform> targetPlayers = new List<Transform>();

        public APIModule(BaseCharacter character) : base(character)
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                var a = character.team.teamType == TeamType.Red ? GameManager.Instance.teamBlue : GameManager.Instance.teamRed;

                foreach (var item in a.members)
                {
                    targetPlayers.Add(item.transform);
                }
            });
        }

        public void MoveAPI(Vector2 direction)
        {
            if (character.isDead || !character.isCanMove)
            {
                character.move.moveDirection = Vector2.zero;
                return;
            }

            character.move.moveDirection = direction;

            if (!character.isRotating)
            {
                character.move.rotateDirection = direction;
            }
        }

        public void RotateAPI(Vector2 dir)
        {
            if (character.isDead) return;

            if (character.controller is HumanController && character.isShooting)
            {
                if (dir.magnitude <= 0.5f)
                {
                    var dir1 = CalculateAutoAim();
                    dir = dir1 != Vector2.zero ? dir1 : dir;
                }
            }

            if (dir.magnitude <= 0.3f)
            {
                character.isRotating = false;
                return;
            }
            else
            {
                character.isRotating = true;
                character.move.rotateDirection = dir;
            }
        }

        public void ShootAPI(Vector2 dir)
        {
            if (character.isDead) return;
            if (dir != Vector2.zero)
            {
                character.weapon.TurnOnShooting();
            }
            else
            {
                character.weapon.TurnOffShooting();
            }
        }

        public Vector2 CalculateAutoAim()
        {
            if (character.controller is not HumanController) return Vector2.zero;

            float minDis = Mathf.Infinity;

            Transform targetShoot = null;

            var contrl = character.controller as HumanController;

            for (int i = 0; i < targetPlayers.Count; i++)
            {
                if (targetPlayers[i] == null || !targetPlayers[i].gameObject.activeSelf) continue;

                if (Common.IsInView(contrl.cameraFollow.GetComponent<Camera>(), targetPlayers[i].position) && !Common.CheckRaycastWall(targetPlayers[i], character.gunPoint, GameManager.Instance.layerWall))
                {
                    float distance = Vector2.Distance(character.transform.position, targetPlayers[i].position);
                    if (distance < minDis)
                    {
                        minDis = distance;
                        targetShoot = targetPlayers[i];
                    }
                }
            }

            if (targetShoot != null)
            {
                return (targetShoot.position - character.transform.position).normalized;
            }
            else
            {
                return Vector2.zero;
            }
        }
    }
}
