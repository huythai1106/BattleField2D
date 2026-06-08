using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Minigame.CS2D;
using Minigame.DragonFight;
using UnityEngine;
using UnityEngine.UI;

namespace Minigame.Battlefield
{
    public class HumanController : Controller
    {
        public FixedJoystick joystickMove;
        public FixedJoystick joystickShoot;

        private KeyBoardSetting.KeyBoard keyBoard;
        public CS2D.CameraFollow cameraFollow;
        public int revert = 1;

        public float scopeMultiplier = 1;
        public float cameraSize;

        [Header("UI")]
        public HealthUI healthUI;
        public ReloadUI reloadUI;
        public Image weaponImg;
        public BulletUI bulletUI;
        public Button healthIcon;
        public Button chargeBulletIcon;
        public InfoUI infoUI;
        public float sizeCamDead = 14f;

        private void Awake()
        {
            cameraSize = cameraFollow.GetComponent<Camera>().orthographicSize;
        }

        protected override void Start()
        {
            base.Start();

            if (name == "Red")
            {
                keyBoard = GameManager.Instance.keyBoard.player1;
            }
            else
            {
                keyBoard = GameManager.Instance.keyBoard.player2;
            }
        }

        protected override void Controll()
        {
            HandleMove();
            HandleShoot();
        }

        public override void SetCharacter(BaseCharacter c)
        {
            base.SetCharacter(c);
            // cameraFollow.target = character.transform;

            ZoomIn();

            // ChangeCamSize(c.classSetting.scope);

            healthUI.RegisterFunc(c);
            bulletUI.RegisterFunc(c);
            infoUI.RegisterFunc(c);

            weaponImg.sprite = c.weapon.currentGun.GunSetting.weaponSprite;
            weaponImg.SetNativeSize();

            healthIcon.gameObject.SetActive(false);
            chargeBulletIcon.gameObject.SetActive(false);

            if (c.classSetting.classPlayer == ClassPlayer.Medic)
            {
                healthIcon.gameObject.SetActive(true);
            }
            if (c.classSetting.classPlayer == ClassPlayer.Support)
            {
                chargeBulletIcon.gameObject.SetActive(true);
            }
        }

        private void HandleMove()
        {
            var moveDirection = CaculateRotate();
            character.APIModule.MoveAPI(moveDirection * revert);
        }

        private void HandleShoot()
        {
            var dir = joystickShoot.Direction * revert;
            character.APIModule.RotateAPI(dir);

            character.APIModule.ShootAPI(dir);
        }

        public void ChangeCamSize(float mul)
        {
            scopeMultiplier = mul;

            if (cameraFollow)
            {
                cameraFollow.GetComponent<Camera>().orthographicSize = cameraSize * mul;
            }
        }

        public override void OnCharacterDead()
        {
            base.OnCharacterDead();
            cameraFollow.target = null;
            ZoomOut();
        }

        protected Vector2 CaculateRotate()
        {
            Vector2 rotate = Vector2.zero;

            if (joystickMove.Direction != Vector2.zero)
            {
                rotate += joystickMove.Direction.normalized;
            }

            if (Input.GetKey(keyBoard.left))
            {
                rotate += Vector2.left;
            }

            if (Input.GetKey(keyBoard.right))
            {
                rotate += Vector2.right;
            }

            if (Input.GetKey(keyBoard.up))
            {
                rotate += Vector2.up;
            }

            if (Input.GetKey(keyBoard.down))
            {
                rotate += Vector2.down;
            }

            return rotate.normalized;
        }

        private void ZoomIn()
        {
            var camsize = character.classSetting.scope * cameraSize;
            var cam = cameraFollow.GetComponent<Camera>();

            cam.DOOrthoSize(camsize, 1);

            cam.transform.DOMove(new Vector3(character.transform.position.x, character.transform.position.y, -10), 1).OnComplete(() =>
            {
                cameraFollow.target = character.transform;
            });

        }

        private void ZoomOut()
        {
            cameraFollow.GetComponent<Camera>().DOOrthoSize(sizeCamDead, 1);
        }

        public void OnReload()
        {
            if (character)
            {
                character.weapon.currentGun.AcitveReload();
            }
        }
    }
}
