using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Minigame.Battlefield
{
    public enum TypeMap
    {
        Default,
        Attack,
        Defend,
    }

    public class Map : MonoBehaviour
    {
        [Header("Setup Camera")]
        public int timePlay;
        public bool isFixedCam;
        public Vector3 topRight;
        public Vector3 downLeft;
        public TypeMap typeMap;
        public bool isLimitCam = false;
        public string tutText;

        public List<Transform> pointSpawnRed;
        public List<Transform> pointSpawnBlue;
        public List<Transform> pointSpawnPlayer;
        public BaseController baseManager;

        public List<Transform> pointDefendRed;
        public List<Transform> pointDefendBlue;

        public List<Transform> pointStartRed;
        public List<Transform> pointStartBlue;

        private void Start()
        {
            if (isFixedCam)
            {
                foreach (var item in GameManager.Instance.cameras)
                {
                    var i = item.GetComponent<CS2D.CameraFollow>();
                    i.isLimitCamera = true;
                    i.SetupBoudary(topRight, downLeft);
                }
            }

            GameManager.Instance.astarPath.Scan();
        }

        public Vector3 GetRandomStartPoint(TeamType typePlayer)
        {
            List<Transform> spawnPoints = typePlayer == TeamType.Red ? pointStartRed : pointStartBlue;
            if (spawnPoints.Count == 0)
            {
                return Vector3.zero;
            }
            int randomIndex = Random.Range(0, spawnPoints.Count);

            Transform t = spawnPoints[randomIndex];
            spawnPoints.Remove(t);

            DOVirtual.DelayedCall(1f, () =>
            {
                spawnPoints.Add(t);
            });

            return t.position;
        }

        public Vector3 GetRandomSpawnPoint(TeamType typePlayer)
        {
            List<Transform> spawnPoints = typePlayer == TeamType.Red ? pointSpawnRed : pointSpawnBlue;
            int randomIndex = Random.Range(0, spawnPoints.Count);

            Transform t = spawnPoints[randomIndex];
            spawnPoints.Remove(t);

            DOVirtual.DelayedCall(1f, () =>
            {
                spawnPoints.Add(t);
            });

            return t.position;
        }

        public Vector3 GetRandomSpawnPointPlayer()
        {
            int randomIndex = Random.Range(0, pointSpawnPlayer.Count);

            Transform t = pointSpawnPlayer[randomIndex];
            pointSpawnPlayer.Remove(t);

            DOVirtual.DelayedCall(1f, () =>
            {
                pointSpawnPlayer.Add(t);
            });

            return t.position;
        }

        void OnDrawGizmos()
        {
            // Tính các điểm còn lại của hình chữ nhật
            Vector3 downRight = new Vector3(topRight.x, downLeft.y, downLeft.z); // Góc dưới-phải
            Vector3 topLeft = new Vector3(downLeft.x, topRight.y, downLeft.z);   // Góc trên-trái

            // Chọn màu cho hình chữ nhật
            Gizmos.color = Color.green;

            // Vẽ hình chữ nhật bằng cách nối các điểm
            Gizmos.DrawLine(downLeft, downRight);  // Cạnh dưới
            Gizmos.DrawLine(downRight, topRight);  // Cạnh phải
            Gizmos.DrawLine(topRight, topLeft);    // Cạnh trên
            Gizmos.DrawLine(topLeft, downLeft);    // Cạnh trái
        }
    }
}
