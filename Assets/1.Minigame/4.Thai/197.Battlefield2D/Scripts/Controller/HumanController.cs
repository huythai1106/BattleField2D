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

        // Đã xóa joystickShoot vì chuyển sang dùng chuột

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

        private Camera cachedCamera; // Tối ưu: Cache Camera để tránh gọi GetComponent liên tục

        private void Awake()
        {
            // Lưu lại tham chiếu để tránh GC/overhead
            cachedCamera = Camera.main;
            cameraSize = cachedCamera.orthographicSize;
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
            HandleAimAndShoot();

            if (Input.GetKeyDown(keyBoard.skillOne))
            {
                OnReload();
            }
        }

        public override void SetCharacter(BaseCharacter c)
        {
            base.SetCharacter(c);

            ZoomIn();

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

        private void HandleAimAndShoot()
        {
            if (character == null || cachedCamera == null) return;

            // 1. Lấy tọa độ chuột trên màn hình và chuyển đổi sang World Space
            Vector3 mouseWorldPos = cachedCamera.ScreenToWorldPoint(Input.mousePosition);

            // Ép trục Z về 0 để đảm bảo tính toán vector chuẩn xác trên mặt phẳng 2D
            mouseWorldPos.z = 0f;

            // 2. Tính toán hướng từ nhân vật đến vị trí chuột
            Vector2 dir = (mouseWorldPos - character.transform.position).normalized * revert;

            // 3. Cập nhật góc xoay của nhân vật liên tục theo vị trí chuột
            character.APIModule.RotateAPI(dir);

            // 4. Thực hiện lệnh bắn khi nhấn giữ chuột trái (Input.GetMouseButton(0))
            if (Input.GetMouseButton(0))
            {
                character.APIModule.ShootAPI(dir);
            }
            else
            {
                character.APIModule.ShootAPI(Vector2.zero);
            }
        }

        public void ChangeCamSize(float mul)
        {
            scopeMultiplier = mul;

            if (cameraFollow)
            {
                cachedCamera.orthographicSize = cameraSize * mul;
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

            if (joystickMove != null && joystickMove.Direction != Vector2.zero)
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

            cachedCamera.DOOrthoSize(camsize, 1);

            cachedCamera.transform.DOMove(new Vector3(character.transform.position.x, character.transform.position.y, -10), 1).OnComplete(() =>
            {
                cameraFollow.target = character.transform;
            });
        }

        private void ZoomOut()
        {
            cachedCamera.DOOrthoSize(sizeCamDead, 1);
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