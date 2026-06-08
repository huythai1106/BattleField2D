using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Battlefield
{
    public class SourceDamage
    {
        public BaseCharacter sourceCharacter;
        public WeaponName weaponName;

        public float GetDamage()
        {
            // return 0;
            return ItemManager.Instance.itemDict[weaponName].setting.damage;
        }
    }

    public enum TeamType
    {
        Red,
        Blue,
    }

    public enum ClassPlayer
    {
        Assault,
        Support,
        Medic,
        Scout,
        Elite,
    }

    public enum WeaponName
    {
        AK47,
        M16,
        Scal,
        Groza,
        M24,
        K98,
        S686,
        Ump,
        Uzi,
        Gatling,
        FlameThrower,
        Genade,
        Smoke,
        Molotov,
        Rocket,
    }

    public static class Common
    {
        public const string ITEM_TAG = "Food";
        public const string PLAYER_TAG = "Player";
        public const string BULLET_TAG = "Bullet";
        public const string WALL_TAG = "Wall";
        public const string BOX_TAG = "";

        public static string FormatMinutesToTime(int totalMinutes)
        {
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;
            return $"{hours}:{minutes:D2}";
        }

        public static bool IsInView(Camera camera, Vector3 worldPosition)
        {
            Vector3 viewportPoint = camera.WorldToViewportPoint(worldPosition);

            bool isInView = viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                            viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
                            viewportPoint.z >= 0;

            return isInView;
        }

        public static bool CheckRaycastWall(Transform target, Transform gunPoint, LayerMask layer)
        {
            RaycastHit2D hit = Physics2D.Raycast(gunPoint.position, (target.position - gunPoint.position).normalized, (target.position - gunPoint.position).magnitude, layer);

            return hit.collider != null;
        }

        public static void ClampPosition(this Transform t, float minX, float maxX, float minY, float maxY)
        {
            // Kiểm tra nếu transform bị null để tránh lỗi crash
            if (t == null) return;

            // Lấy vị trí hiện tại của đối tượng
            Vector3 currentPosition = t.position;

            // Giới hạn giá trị x và y trong khoảng min/max cho phép
            float clampedX = Mathf.Clamp(currentPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(currentPosition.y, minY, maxY);

            // Cập nhật lại vị trí mới (giữ nguyên trục Z)
            t.position = new Vector3(clampedX, clampedY, currentPosition.z);
        }
    }
}
