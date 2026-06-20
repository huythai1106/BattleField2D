using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using UnityEngine;

namespace Minigame.Battlefield
{
    public enum StylePlayer
    {
        Defend,
        Attack,
    }

    [RequireComponent(typeof(Seeker))]
    public class AIController : Controller
    {
        internal Seeker seeker;
        public Path path;
        public float nextWayPointDistance = 1;
        protected int currentWayPoint = 0;

        public Transform targetMove;
        public Transform targetShoot;
        protected float currentRotateZ = 0;
        protected float timeDelayFindPath = 0.7f;
        public float rangeAttack;
        public bool isReachEndPath;

        public StylePlayer stylePlayer;

        public List<Transform> targetPlayers = new List<Transform>();

        protected override void Start()
        {
            seeker = GetComponent<Seeker>();

            gameObject.name = GameManager.Instance.GetNameAI();

            stylePlayer = StylePlayer.Attack;

            var a = team.teamType == TeamType.Red ? GameManager.Instance.teamBlue : GameManager.Instance.teamRed;

            foreach (var item in a.members)
            {
                targetPlayers.Add(item.transform);
            }

            GameManager.Instance.currentMap.baseManager.onChangeBase += OnBaseChange;

            DOVirtual.DelayedCall(1f, () =>
            {
                UpdatePath();
            });
        }

        protected void UpdatePath()
        {
            CaculateTargetMove();
            if (targetMove && character)
            {
                seeker.StartPath(character.transform.position, targetMove.position, OnPathChange);
            }
        }

        public virtual void CaculateTargetMove()
        {
            if (stylePlayer == StylePlayer.Attack)
            {
                var listTargetMove = team.teamType == TeamType.Red ? GameManager.Instance.currentMap.baseManager.targetRed : GameManager.Instance.currentMap.baseManager.targetBlue;
                if (listTargetMove.Count == 0)
                {
                    targetMove = null;
                    return;
                }

                targetMove = listTargetMove[Random.Range(0, listTargetMove.Count)].transform;
            }
            else
            {
                var listTargetMove = team.teamType == TeamType.Red ? GameManager.Instance.currentMap.pointDefendRed : GameManager.Instance.currentMap.pointDefendBlue;
                if (listTargetMove.Count == 0)
                {
                    targetMove = null;
                    return;
                }

                targetMove = listTargetMove[team.defendPoint % listTargetMove.Count].transform;
                team.defendPoint++;
            }
        }

        private void OnPathChange(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWayPoint = 0;
            }
        }

        protected override void Controll()
        {
            base.Controll();

            HandleMove();
            HandleAttack();
            HandleUseSkill();
        }

        #region Move
        private void HandleMove()
        {
            if (path == null) return;
            Vector2 direction = Vector2.zero;

            if (currentWayPoint >= path.vectorPath.Count)
            {
                OnPathComplete();
                MoveDirect(direction);
                isReachEndPath = true;
                return;
            }

            direction = (Vector2)(path.vectorPath[currentWayPoint] - character.transform.position).normalized;
            float distance = Vector2.Distance(character.transform.position, path.vectorPath[currentWayPoint]);

            if (distance < nextWayPointDistance)
            {
                currentWayPoint++;
            }

            if (direction != Vector2.zero)
            {
                MoveDirect(direction);
            }
        }

        private void MoveDirect(Vector2 direction)
        {
            float rotateZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (Mathf.Abs(currentRotateZ - rotateZ) < 120)
            {
                character.APIModule.MoveAPI(direction);
            }
            currentRotateZ = rotateZ;
        }
        #endregion

        #region Attack
        Coroutine shootCoroutine;
        public virtual void HandleAttack()
        {
            if (GameManager.Instance.currentMap.typeMap == TypeMap.Defend) return;

            CaculateTargetShoot();

            if (targetShoot)
            {
                character.APIModule.RotateAPI((targetShoot.position - character.transform.position).normalized);
                shootCoroutine ??= StartCoroutine(ShootCouroutine());
            }
            else
            {
                if (shootCoroutine != null)
                {
                    StopCoroutine(shootCoroutine);
                    shootCoroutine = null;
                }

                character.APIModule.RotateAPI(Vector2.zero);
                character.isShooting = false;
            }
        }

        private IEnumerator ShootCouroutine()
        {
            yield return new WaitForSeconds(0.3f);

            while (targetShoot)
            {
                float burstInterval = Random.Range(0.2f, 0.4f);
                float burstPause = Random.Range(0.2f, 0.5f);

                character.isShooting = true;
                yield return new WaitForSeconds(burstInterval);
                character.isShooting = false;
                yield return new WaitForSeconds(burstPause);
            }

            character.isShooting = false;
            character.APIModule.RotateAPI(Vector2.zero);

            // if (targetShoot)
            // {
            //     character.isShooting = true;
            //     //cha racter.APIModule.RotateAPI((targetShoot.position - transform.position).normalized);
            // }
            // else
            // {
            //     character.isShooting = false;
            //     character.APIModule.RotateAPI(Vector2.zero);
            // }
        }

        public virtual void CaculateTargetShoot()
        {
            float minDis = Mathf.Infinity;

            targetShoot = null;

            for (int i = 0; i < targetPlayers.Count; i++)
            {
                if (targetPlayers[i] == null || !targetPlayers[i].gameObject.activeSelf) continue;

                float distance = Vector2.Distance(character.transform.position, targetPlayers[i].position);
                if (distance <= rangeAttack * character.classSetting.scope && !CheckRaycastWall(targetPlayers[i], character.gunPoint))
                {
                    if (distance < minDis)
                    {
                        minDis = distance;
                        targetShoot = targetPlayers[i];
                    }
                }
            }
        }
        #endregion
        protected bool isChangingTargetMove = false;
        public void ChangePosTargetMove()
        {
            if (isChangingTargetMove) return;

            isChangingTargetMove = true;

            DOVirtual.DelayedCall(2f, () =>
            {
                UpdatePath();
                isChangingTargetMove = false;
            });
        }

        protected virtual void OnPathComplete()
        {
            if (stylePlayer == StylePlayer.Defend)
            {
                ChangePosTargetMove();
            }
        }

        private bool CheckRaycastWall(Transform target, Transform gunPoint)
        {
            RaycastHit2D hit = Physics2D.Raycast(gunPoint.position, (target.position - gunPoint.position).normalized, (target.position - gunPoint.position).magnitude, GameManager.Instance.layerWall);

            return hit.collider != null;
        }

        private void OnBaseChange(Base b)
        {
            if (targetMove != null && targetMove.position == b.transform.position)
            {
                UpdatePath();
            }
        }

        protected override void OnSpawnCharacter(BaseCharacter c)
        {
            base.OnSpawnCharacter(c);
            RandomStyle();
            UpdatePath();
        }

        private void RandomStyle()
        {
            if (team.numberOfPlayerDefend == team.maxPlayerDefend)
            {
                stylePlayer = StylePlayer.Attack;
                return;
            }
            else
            {
                stylePlayer = (StylePlayer)Random.Range(0, 2);
                if (stylePlayer == StylePlayer.Defend)
                {
                    team.numberOfPlayerDefend++;
                }
            }
        }

        public override void OnCharacterDead()
        {
            base.OnCharacterDead();
            if (stylePlayer == StylePlayer.Defend)
            {
                team.numberOfPlayerDefend--;
            }
        }

        private void HandleUseSkill()
        {
            if (!character) return;

            if ((character.classSetting.classPlayer == ClassPlayer.Medic || character.classSetting.classPlayer == ClassPlayer.Support) && character.isCanUseSkill)
            {
                OnUseSkill();
            }
        }
    }
}
