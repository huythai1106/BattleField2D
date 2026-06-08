using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestCam : MonoBehaviour
{
    private int indexSize = 0;
    private float[] sizeMulti = new float[] { 1, 1.2f, 1.35f, 1.5f };
    public float originSize = 9;
    public Camera cam;

    private void Start()
    {
        originSize = cam.orthographicSize;
    }

    public void TestSizeCam()
    {
        indexSize = (indexSize + 1) % sizeMulti.Length;
        cam.orthographicSize = originSize * sizeMulti[indexSize];
    }
}
