using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    [Header("Cấu hình hiệu ứng")]
    public float scaleDelta = 0.1f;    // thay đổi tỉ lệ ±0.1 so với scale gốc
    public float duration = 0.5f;      // thời gian 1 chu kỳ
    public Ease easeType = Ease.InOutSine; // easing mượt

    private Vector3 originalScale;
    private Tween pulseTween;

    void Start()
    {
        originalScale = transform.localScale;
        StartPulse();
    }

    public void StartPulse()
    {
        pulseTween?.Kill();

        // Scale lên & xuống so với scale gốc
        Vector3 targetScale = originalScale * (1 + scaleDelta);

        pulseTween = transform.DOScale(targetScale, duration)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void StopPulse()
    {
        pulseTween?.Kill();
        transform.localScale = originalScale;
    }

    private void OnDisable()
    {
        pulseTween?.Kill();
        transform.localScale = originalScale;
    }
}
