using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VFX_FBF
{
    public class CameraController : MonoBehaviour
    {
        public float panSpeed = 10f; // Tốc độ di chuyển camera
        public float zoomSpeedFactor = 0.1f;
        public float zoomSpeed = 5f; // Tốc độ zoom
        public float minZoom = 5f; // Giới hạn zoom gần nhất
        public float maxZoom = 20f; // Giới hạn zoom xa nhất

        private Vector3 lastMousePosition;

        void Update()
        {
            HandlePan();
            HandleZoom();
        }

        void HandlePan()
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                lastMousePosition = Input.mousePosition;
            }
        
            if (Input.GetMouseButton(0)) 
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                Vector3 move = new Vector3(-delta.x, -delta.y, 0) * panSpeed * Time.deltaTime;
                transform.Translate(move, Space.Self);
                lastMousePosition = Input.mousePosition;
            }
        }

        void HandleZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                Camera camera = GetComponent<Camera>();
                float zoomSpeed = camera.orthographicSize * zoomSpeedFactor; // Điều chỉnh tốc độ zoom theo kích thước camera
                float newSize = Mathf.Clamp(camera.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
                camera.orthographicSize = newSize;
            }
        }
    }
}