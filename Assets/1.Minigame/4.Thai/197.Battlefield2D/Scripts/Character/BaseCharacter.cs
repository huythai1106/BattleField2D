using System.Collections;
using System.Collections.Generic;
// using Bonbongame.Extensions;
using DG.Tweening;
using Spine.Unity;
using Thai.Lib;
using UnityEngine;

namespace Minigame.Battlefield
{
    [System.Serializable]
    public class Property
    {
        public BindableProperty<float> health = new();
        public BindableProperty<float> armor = new();
        public float speed;
    }

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class BaseCharacter : MonoBehaviour
    {
        internal Team team;
        internal Controller controller;

        [Header("Setting")]
        public CharacterSetting characterSetting;
        public Rigidbody2D rb;
        public Collider2D col;
        public Property properties = new();
        public SkeletonAnimation anim;
        public Transform gunPoint;
        public ParticleSystem muzzleFlash;
        public ClassSetting classSetting;
        public GameObject iconRed;
        public GameObject iconBlue;
        public GameObject crosshair;
        public GameObject aura;

        [Header("Module")]
        public MoveModule move;
        public StateModule state;
        public HealthModule health;
        public APIModule APIModule;
        public WeaponModule weapon;


        [Header("Foot step")]
        public AudioSource footstepAudio;
        public ParticleSystem footStepLeft;
        public ParticleSystem footStepRight;

        [Header("Status")]
        public bool isDead;
        public bool isCanMove = true;
        public bool isShooting = false;
        public bool isRotating = false;
        public bool isCanUseSkill = true;


        [Header("Effect")]
        public FlameThrowerControll flameCrl;
        public ParticleSystem healAreaEffect;
        public ParticleSystem rechargeBulletEffect;
        public BindableProperty<string> bulletValue = new();

        public float cooldown;

        #region Setup
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            GameManager.Instance.OnFinishGame += OnFinishGame;
            SetupModule();
        }

        public void Spawn()
        {
            gameObject.SetActive(true);

            var r = Random.Range(0, 2);
            if (GameManager.Instance.isStart && r == 0)
            {
                var v = GameManager.Instance.currentMap.GetRandomStartPoint(team.teamType);
                if (v != Vector3.zero)
                {
                    transform.position = v;
                }
                else
                {
                    SetPosChar();
                }
            }
            else
            {
                SetPosChar();
            }

            SetupWeapon();
            SetupProperty();

            if (controller is HumanController)
            {
                crosshair.SetActive(true);
                aura.SetActive(true);
            }
            else
            {
                crosshair.SetActive(false);
                aura.SetActive(false);
            }
        }

        private void SetPosChar()
        {
            if (controller is HumanController)
            {
                transform.position = GameManager.Instance.currentMap.GetRandomSpawnPointPlayer();
            }
            else
            {
                transform.position = GameManager.Instance.currentMap.GetRandomSpawnPoint(team.teamType);
            }
        }

        private void SetupModule()
        {
            move = new MoveModule(this);
            state = new StateModule(this);
            health = new HealthModule(this);
            APIModule = new APIModule(this);
            weapon = new WeaponModule(this);
        }

        private void SetupProperty()
        {
            isDead = false;
            properties.health.Value = characterSetting.maxHealth;
            properties.armor.Value = classSetting.armor;
            properties.speed = characterSetting.speed - weapon.currentGun.GunSetting.weight;

            if (GameManager.Instance.currentMap.typeMap == TypeMap.Defend && GameManager.Instance.isModeAI && team.teamType == TeamType.Blue)
            {
                properties.speed *= 0.55f;
            }
        }
        #endregion

        #region Update
        private void FixedUpdate()
        {
            move.FixedUpdate();
            LimitPosOnCam();
        }

        private void Update()
        {
            UpdateFootStep();
            weapon.Update();
        }

        private void LimitPosOnCam()
        {
            if (controller is HumanController && GameManager.Instance.currentMap.isLimitCam)
            {
                var cam = (controller as HumanController).cameraFollow.GetComponent<Camera>();

                float halfHeight = cam.orthographicSize;
                float halfWidth = halfHeight * cam.aspect;
                Vector3 camPos = cam.transform.position;
                float minX = camPos.x - halfWidth;
                float maxX = camPos.x + halfWidth;
                float minY = camPos.y - halfHeight;
                float maxY = camPos.y + halfHeight;

                transform.ClampPosition(minX, maxX, minY, maxY);
            }
        }
        #endregion

        #region Foot Step
        public void UpdateFootStep()
        {
            if (GameManager.Instance.isFinishGame) return;

            bool shouldShow = rb.velocity.magnitude != 0 && !isDead && !isInWater;

            if (footStepLeft.gameObject.activeSelf != shouldShow)
            {
                footStepLeft.gameObject.SetActive(shouldShow);

                if (footstepAudio)
                {
                    if (shouldShow)
                    {
                        footstepAudio.Play();
                    }
                    else
                    {
                        footstepAudio.Stop();
                    }
                }
            }

            if (footStepRight.gameObject.activeSelf != shouldShow)
            {
                footStepRight.gameObject.SetActive(shouldShow);
            }

            if (shouldShow)
            {
                float rotation = (-transform.eulerAngles.z + 90) * Mathf.Deg2Rad;

                var mainLeft = footStepLeft.main;
                mainLeft.startRotation = rotation;

                var mainRight = footStepRight.main;
                mainRight.startRotation = rotation;
            }
        }
        #endregion

        public void PlayAnimHit()
        {
            if (isDead) return;

            ChangeColorHit();
        }

        private void ChangeColorHit()
        {
            // state?.anim.Extensions_DOColor(new Color(1, 0.3f, 0.3f), 0.1f).OnComplete(() =>
            // {
            //     state?.anim.skeleton.SetColor(Color.white);
            // });
        }

        public void SetupWeapon()
        {
            WeaponName nameGun = classSetting.weapons[Random.Range(0, classSetting.weapons.Length)];
            if (GameManager.Instance.currentMap.typeMap == TypeMap.Defend && team.teamType == TeamType.Red)
            {
                nameGun = WeaponName.Gatling;
            }

            Gun w = ItemManager.Instance.CreateItem(nameGun) as Gun;
            weapon.EquipWeapon(w);

            w.SetBulletValue();
        }

        public void ClearOnDeath()
        {
            properties.health.Clear();
            properties.armor.Clear();
            bulletValue.Clear();
        }

        public bool CheckWall()
        {
            var dir = gunPoint.position - transform.position;
            var cols = Physics2D.Raycast(transform.position, dir, dir.magnitude, GameManager.Instance.layerWall);

            return cols.collider != null;
        }

        #region WaterControll
        public bool isInWater = false;
        private void HandleWaterWave()
        {
            if (GameManager.Instance.isFinishGame) return;

            if (rb.velocity.magnitude > 0)
            {
                SoundManager.instance.PlaySoundEffectList("waterWalk");
                // EffectManager.Instance.CreatedEffect("waterWave", transform);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("WaterGirl"))
            {
                isInWater = true;
                InvokeRepeating(nameof(HandleWaterWave), 0.3f, 0.7f);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("WaterGirl"))
            {
                isInWater = false;
                CancelInvoke(nameof(HandleWaterWave));
            }
        }
        #endregion

        #region SpecificSkills

        // for medic

        public void OnUseSkill()
        {
            if (!isCanUseSkill) return;

            if (classSetting.classPlayer == ClassPlayer.Medic)
            {
                var allCharacter = Physics2D.OverlapCircleAll(transform.position, 6, GameManager.Instance.layerPlayer);

                foreach (var item in allCharacter)
                {
                    var c = item.GetComponent<BaseCharacter>();
                    if (c.team == team)
                    {
                        c.health.Health(50);
                    }
                }
                isCanUseSkill = false;

                healAreaEffect.Play();
                GameManager.Instance.StartCoroutine(ResetCooldown());
            }
            else if (classSetting.classPlayer == ClassPlayer.Support)
            {
                var allCharacter = Physics2D.OverlapCircleAll(transform.position, 5, GameManager.Instance.layerPlayer);

                foreach (var item in allCharacter)
                {
                    var c = item.GetComponent<BaseCharacter>();
                    if (c.team == team)
                    {
                        c.weapon.currentGun.RecoverBullet();
                    }
                }
                isCanUseSkill = false;
                rechargeBulletEffect.Play();
                GameManager.Instance.StartCoroutine(ResetCooldown());
            }
        }

        public IEnumerator ResetCooldown()
        {
            yield return new WaitForSeconds(cooldown);
            isCanUseSkill = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, 5);
        }
        #endregion

        private void OnFinishGame()
        {
            isCanMove = false;
            isShooting = false;
            isRotating = false;
            weapon.TurnOffShooting();
            rb.velocity = Vector2.zero;
            footstepAudio.Stop();
        }

        public void ChangeClassSkin()
        {
            string n = classSetting.classPlayer.ToString() + "_" + team.teamType.ToString();
            anim.skeleton.SetSkin(n);
            anim.skeleton.SetSlotsToSetupPose();
        }
    }
}
