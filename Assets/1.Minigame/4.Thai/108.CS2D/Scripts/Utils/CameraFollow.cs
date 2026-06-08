using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Minigame.CS2D
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public bool isFixedX = false;

        public bool isLimitCamera = false;
        public Vector3 downLeft;
        public Vector3 topRight;

        float camHeight, camWidth;
        private Camera cam;

        private void Start()
        {
            cam = GetComponent<Camera>();
        }

        public Vector3 SetLimitCam()
        {
            camHeight = cam.orthographicSize;
            camWidth = cam.orthographicSize * cam.aspect;

            if (camHeight * 2 > topRight.y - downLeft.y)
            {
                camHeight = (topRight.y - downLeft.y) / 2f;
            }
            if (camWidth * 2 > topRight.x - downLeft.x)
            {
                camWidth = (topRight.x - downLeft.x) / 2f;
            }

            var targetPos = target.position;

            targetPos.z = transform.position.z;

            targetPos.x = Mathf.Clamp(targetPos.x, downLeft.x + camWidth, topRight.x - camWidth);
            targetPos.y = Mathf.Clamp(targetPos.y, downLeft.y + camHeight, topRight.y - camHeight);

            return targetPos;
        }

        public Vector3 SetLimitCamSelf()
        {
            camHeight = cam.orthographicSize;
            camWidth = cam.orthographicSize * cam.aspect;

            if (camHeight * 2 > topRight.y - downLeft.y)
            {
                camHeight = (topRight.y - downLeft.y) / 2f;
            }
            if (camWidth * 2 > topRight.x - downLeft.x)
            {
                camWidth = (topRight.x - downLeft.x) / 2f;
            }

            var targetPos = transform.position;

            targetPos.z = transform.position.z;

            targetPos.x = Mathf.Clamp(targetPos.x, downLeft.x + camWidth, topRight.x - camWidth);
            targetPos.y = Mathf.Clamp(targetPos.y, downLeft.y + camHeight, topRight.y - camHeight);

            return targetPos;
        }

        public void SetupBoudary(Vector3 topRight, Vector3 downLeft)
        {
            this.topRight = topRight;
            this.downLeft = downLeft;
        }

        public void ChangeTarget(Transform newTarget, float time)
        {
            target = null;

            transform.DOMove(new Vector3(newTarget.transform.position.x, newTarget.transform.position.y, transform.position.z), time).SetEase(Ease.Linear).OnComplete(() =>
            {
                target = newTarget;
            }).SetUpdate(true);
        }

        private void LateUpdate()
        {
            if (target)
            {
                transform.position = new Vector3(isFixedX ? transform.position.x : target.position.x, target.position.y, -10);
            }

            if (isLimitCamera)
            {
                transform.position = SetLimitCamSelf();
            }
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
