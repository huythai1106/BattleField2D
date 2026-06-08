using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Minigame.CS2D;
using Minigame.DragonFight;
using UnityEngine;

namespace Minigame.Battlefield
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("Setting")]
        public KeyBoardSetting keyBoard;
        public int killToSpawnPlane;
        public Camera[] cameras;
        public Map[] maps;

        [Header("Layer")]
        public LayerMask layerPlayer;
        public LayerMask layerWall;
        public LayerMask layerDoor;
        public LayerMask layerHit;
        public LayerMask layerBullet;
        public GameObject extiBtn;

        public Map currentMap;
        public AstarPath astarPath;
        public event Action OnPlayGameStart;
        public Bullet bulletPrefab;
        public FighterAircrafeCrl fighterAircraftPrefab;

        public Team teamRed;
        public Team teamBlue;

        public bool isModeAI;
        public GameObject bloodDeadPrefab;
        public BindableProperty<int> remainingTimePlaying = new(0);
        public bool isFinishGame = false;
        public bool isStartGame = false;

        public event Action OnFinishGame;

        public bool isStart = true;
        public GameObject[] baseInfos;
        public TutControll tutControll;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            Physics2D.IgnoreLayerCollision(9, 9, true);

            if (!Instance) Instance = this;
        }

        private void OnDestroy()
        {
            Physics2D.IgnoreLayerCollision(9, 9, false);
            Instance = null;
        }

        public void StartGame(int value)
        {
            int mapIndex;
            if (value == -1)
            {
                mapIndex = UnityEngine.Random.Range(0, maps.Length);
            }
            else if (value == 3)
            {
                mapIndex = UnityEngine.Random.Range(3, 5);
            }
            else
            {
                mapIndex = value;
            }

            if (isModeAI)
            {
                Screen.orientation = ScreenOrientation.LandscapeRight;
            }

            InvokeRepeating(nameof(ChangeRemainingTimePlaying), 1f, 1f);

            CreateMap(mapIndex);

            teamRed.Init();
            teamBlue.Init();

            DOVirtual.DelayedCall(2f, () =>
            {
                isStart = false;
            });

            tutControll.StartTuT();
            isStartGame = true;

            // extiBtn.GetComponent<CanvasGroup>().alpha = 0.5f;
            // extiBtn.GetComponent<CanvasGroup>().blocksRaycasts = true;
            OnPlayGameStart?.Invoke();
        }

        private void ChangeRemainingTimePlaying()
        {
            remainingTimePlaying.Value--;
            if (remainingTimePlaying.Value <= 0)
            {
                Debug.Log("Game Over");
                FinishGame(GetValueEndGame());
            }
        }

        public int GetValueEndGame()
        {
            if (currentMap.typeMap == TypeMap.Attack)
            {
                return 0;
            }
            else if (currentMap.typeMap == TypeMap.Defend)
            {
                return 1;
            }
            else
            {
                var bCrl = currentMap.baseManager;

                int countRed = bCrl.baseRed.Count;
                int countblue = bCrl.baseBlue.Count;

                if (countRed > countblue) return 1;
                else if (countblue == countRed) return teamRed.TotalKill > teamBlue.TotalKill ? 1 : teamRed.TotalKill < teamBlue.TotalKill ? 2 : 0;
                else return 0;
            }
        }

        public void CreateMap(int index)
        {
            currentMap = Instantiate(maps[index]);
            remainingTimePlaying.Value = currentMap.timePlay;

            tutControll.SetText(currentMap.tutText);

            if (currentMap.typeMap == TypeMap.Attack || currentMap.typeMap == TypeMap.Defend)
            {
                teamRed.maxPlayerDefend = 0;
                teamBlue.maxPlayerDefend = 0;
            }

            if (currentMap.typeMap != TypeMap.Defend)
            {
                DOVirtual.DelayedCall(2f, () =>
                {
                    PlayVoiceSound();
                });
            }
            else
            {
                foreach (var item in baseInfos)
                {
                    item.SetActive(false);
                }
            }
        }

        public void FinishGame(int team)
        {
            isFinishGame = true;
            // CancelInvoke(nameof(ChangeRemainingTimePlaying));
            CancelInvoke();

            OnFinishGame?.Invoke();
            // Game2PlayerReplayCanvas.instance.ShowReplayCoop(team);
        }

        #region Utils
        [Header("Cartouche")]
        [SerializeField] private Rigidbody2D cartouche;
        public List<GameObject> inactiveCartouches = new();
        public Transform cartoucheParent;

        public void CreateCartouche(Vector3 position)
        {
            Rigidbody2D a;
            if (inactiveCartouches.Count > 0)
            {
                a = inactiveCartouches[0].GetComponent<Rigidbody2D>();
                a.gameObject.SetActive(true);
                inactiveCartouches.Remove(a.gameObject);
                a.GetComponent<SpriteRenderer>().color = Color.white;
                a.GetComponent<Cartouche>().DoFadeImage();
            }
            else
            {
                a = Instantiate(cartouche);
                a.transform.SetParent(cartoucheParent);
            }

            a.transform.position = position;
            a.velocity = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) * 5f;
            a.AddTorque(UnityEngine.Random.Range(-300f, 300f));
            // Destroy(a.gameObject, 3f);
            StartCoroutine(KillCarouche(a));
        }

        public IEnumerator KillCarouche(Rigidbody2D a)
        {
            yield return new WaitForSeconds(2);
            a.gameObject.SetActive(false);
            inactiveCartouches.Add(a.gameObject);
        }

        public List<string> nameAI = new List<string>()
        {
            "Tha1CH","L0ngB1","Dem1ne","S1nh","M0nesy","Faker","S1mple","Dev1ce","Shr0ud","Ker0n","TL33","M1tu","TuD3v","Kar1k","420GG","B1nZ","Sh1k1","Kion","K1zz",
        };

        public Bullet CreateBullet(SourceDamage source, Vector2 position, Vector2 direction, float offset = 0f, float speed = 0f)
        {
            var bullet = Instantiate(bulletPrefab, position, Quaternion.identity);

            Vector2 dir = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-offset, offset)) * direction;
            bullet.Init(source, dir, speed);

            return bullet;
        }

        public string GetNameAI()
        {
            string a = nameAI[UnityEngine.Random.Range(0, nameAI.Count)];
            nameAI.Remove(a);
            return a;
        }

        public void CreateBlood(Transform t)
        {
            GameObject a = Instantiate(bloodDeadPrefab);
            a.transform.position = t.position;

            Destroy(a, 1f);
        }

        private void PlayVoiceSound()
        {
            // Game2PlayerSoundManager.instance.PlaySoundEffectList("voice");
            Invoke(nameof(PlayVoiceSound), UnityEngine.Random.Range(6, 9));
        }

        public void SpawnFighterAircraft(TeamType teamType)
        {
            var obj = Instantiate(fighterAircraftPrefab);
            obj.Init(teamType);
        }
        #endregion
    }
}
