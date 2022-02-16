using UnityEngine;

using DG.Tweening;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;

    [Header("References")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _handCursorTransform;
    [SerializeField] private RectTransform _handTransform;
    [SerializeField] private RectTransform _textTransform;

    private void Awake() => Instance = this;

    public void Show()
    {
        _canvas.enabled = true;

        _handCursorTransform.anchoredPosition = new Vector2(_handCursorTransform.anchoredPosition.x - 250f, _handCursorTransform.anchoredPosition.y);
        _handCursorTransform.DOAnchorPosX(_handCursorTransform.anchoredPosition.x + 500f, 2f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);

        _handTransform.localScale = Vector3.one * 0.8f;
        _handTransform.DOScale(1.2f, 2f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InQuad);

        _textTransform.DOScale(1.5f, 2f)
            .SetLoops(-1, LoopType.Yoyo);
    }
    public void Hide()
    {
        _canvas.enabled = false;

        _handCursorTransform.DOKill();
        _handTransform.DOKill();
        _textTransform.DOKill();
    }
}