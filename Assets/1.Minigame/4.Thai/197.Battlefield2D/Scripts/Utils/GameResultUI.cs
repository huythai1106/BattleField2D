using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameResultUI : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup backgroundOverlay; // Khung nền đen mờ phía sau (Tùy chọn)
    public RectTransform winIcon;
    public RectTransform loseIcon;
    public Button replayButton;

    [Header("Animation Settings")]
    public float animDuration = 0.5f;

    private void Awake()
    {
        // Đăng ký sự kiện click
        replayButton.onClick.AddListener(OnReplayClicked);

        // Đảm bảo UI ẩn khi vừa vào game
        HideAll();
    }

    private void HideAll()
    {
        winIcon.gameObject.SetActive(false);
        winIcon.localScale = Vector3.zero;

        loseIcon.gameObject.SetActive(false);
        loseIcon.localScale = Vector3.zero;

        replayButton.gameObject.SetActive(false);
        replayButton.transform.localScale = Vector3.zero;

        if (backgroundOverlay != null)
        {
            backgroundOverlay.alpha = 0f;
            backgroundOverlay.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Gọi hàm này khi end game. Truyền true nếu thắng, false nếu thua.
    /// </summary>
    public void ShowResult(bool isWin)
    {
        HideAll();

        if (isWin)
        {
            SoundManager.instance.PlaySoundEffect("win");
        }
        else
        {
            SoundManager.instance.PlaySoundEffect("lose");
        }
        SoundManager.instance.bgSound.Stop();

        // 1. Fade in Background mờ
        if (backgroundOverlay != null)
        {
            backgroundOverlay.gameObject.SetActive(true);
            backgroundOverlay.DOFade(1, animDuration);
        }

        // 2. Chọn Icon tương ứng
        RectTransform targetIcon = isWin ? winIcon : loseIcon;
        targetIcon.gameObject.SetActive(true);

        // 3. Animation cho Icon (Scale up với hiệu ứng nảy)
        targetIcon.DOScale(Vector3.one, animDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                // Khi Icon chạy xong animation thì mới gọi animation cho nút Replay
                ShowReplayAnimation();
            });
    }

    private void ShowReplayAnimation()
    {
        replayButton.gameObject.SetActive(true);

        // Lưu vị trí ban đầu và đẩy nút xuống dưới 100px để làm hiệu ứng trượt lên
        Vector3 originalPos = replayButton.transform.localPosition;
        replayButton.transform.localPosition = originalPos + new Vector3(0, -100f, 0);

        // Kết hợp 2 hiệu ứng: Trượt lên và Phóng to
        Sequence btnSeq = DOTween.Sequence();

        btnSeq.Append(replayButton.transform.DOLocalMoveY(originalPos.y, animDuration)
              .SetEase(Ease.OutCubic)); // Trượt mượt mà

        btnSeq.Join(replayButton.transform.DOScale(Vector3.one, animDuration)
              .SetEase(Ease.OutBounce)); // Scale nảy nhẹ
    }

    private void OnReplayClicked()
    {
        // Quan trọng: Dọn dẹp tất cả các tween đang chạy trước khi chuyển scene để tránh Memory Leak / Missing Reference Exception
        DOTween.KillAll();

        // Reload lại scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        // Gỡ event khi object bị hủy
        replayButton.onClick.RemoveListener(OnReplayClicked);
    }
}