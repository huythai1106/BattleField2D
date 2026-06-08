using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkipController : MonoBehaviour
{
    public UnityEvent eventSkip;
    public Image progressBarUI;
    public GameObject progressUI;

    public void OnHold()
    {
        DOTween.Kill(progressBarUI);

        progressUI.SetActive(true);
        // progressBarUI.fillAmount = 0;

        progressBarUI.DOFillAmount(1, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            OnSuccess();
        }).SetUpdate(true);
    }

    public void OnExit()
    {
        DOTween.Kill(progressBarUI);

        progressBarUI.DOFillAmount(0, 0.3f).OnComplete(() =>
        {
            progressUI.SetActive(false);
        }).SetUpdate(true);
    }

    private void OnSuccess()
    {
        eventSkip?.Invoke();
    }
}
