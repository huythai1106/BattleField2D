using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class Team : MonoBehaviour
    {
        private static readonly WaitForSeconds _waitForSeconds2 = new(3f);
        public TeamType teamType;

        public List<BaseCharacter> menberAvailable = new();
        public List<BaseCharacter> memberInGame = new();
        public List<BaseCharacter> members = new();
        public List<Controller> controllers = new();

        public int numberOfPlayerDefend = 0;
        public int maxPlayerDefend = 4;
        public int defendPoint = 0;
        private int totalKill = 0;

        public bool isStart = true;

        public int TotalKill
        {
            get { return totalKill; }
            set
            {
                totalKill = value;

                if (totalKill == GameManager.Instance.killToSpawnPlane && GameManager.Instance.currentMap.typeMap != TypeMap.Defend)
                {
                    GameManager.Instance.SpawnFighterAircraft(teamType);
                    totalKill = 0;
                }
            }
        }

        public void Init()
        {
            gameObject.SetActive(true);

            DOVirtual.DelayedCall(0.5f, () =>
            {
                foreach (var m in members)
                {
                    m.team = this;
                    m.ChangeClassSkin();
                    menberAvailable.Add(m);
                    m.gameObject.SetActive(false);

                    if (teamType == TeamType.Blue)
                    {
                        m.iconBlue.SetActive(true);
                        m.iconRed.SetActive(false);
                    }
                    else
                    {
                        m.iconBlue.SetActive(false);
                        m.iconRed.SetActive(true);
                    }
                }

                if (GameManager.Instance.currentMap.typeMap == TypeMap.Defend)
                {
                    if (teamType == TeamType.Blue)
                    {
                        StartCoroutine(SpawnCharacterBlue());
                    }
                    else
                    {
                        foreach (var c in controllers)
                        {
                            if (c is AIController)
                            {
                                c.gameObject.SetActive(false);
                            }
                            else
                            {
                                c.SetCharacter(menberAvailable[0]);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var c in controllers)
                    {
                        c.SetCharacter(menberAvailable[Random.Range(0, menberAvailable.Count)]);
                    }
                }
            });
        }

        private IEnumerator SpawnCharacterBlue()
        {
            for (int i = 0; i < controllers.Count; i++)
            {
                var c = controllers[i];
                c.SetCharacter(menberAvailable[Random.Range(0, menberAvailable.Count)]);
                yield return _waitForSeconds2;
            }
        }

        private float timeGame = 0;
        private void Update()
        {
            if (GameManager.Instance.currentMap && GameManager.Instance.currentMap.typeMap == TypeMap.Defend)
            {
                timeGame += Time.deltaTime;
            }
        }

        public void SpawnCharacter(Controller c)
        {
            float timeRevive = 2f;

            if (GameManager.Instance.currentMap.typeMap == TypeMap.Defend)
            {
                timeRevive = Mathf.Max(4 - timeGame / 16, 0.5f);
            }

            DOVirtual.DelayedCall(timeRevive, () =>
            {
                c.SetCharacter(menberAvailable[Random.Range(0, menberAvailable.Count)]);
            });
        }

        public void BackToAvailable(BaseCharacter c)
        {
            c.gameObject.SetActive(false);
            if (!menberAvailable.Contains(c))
            {
                menberAvailable.Add(c);
                memberInGame.Remove(c);
            }
        }
    }
}
