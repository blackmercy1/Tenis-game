using UnityEngine;

using DG.Tweening;

public class LoseScreen : MonoBehaviour
{
    public static LoseScreen Instance;

    [Header("References")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _textTransform;

    private void Awake()
    {
        Instance = this;
    }

    public void Show()
    {
        _canvas.enabled = true;

        _textTransform.DOScale(1.2f, 3f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
    }
    public void Hide()
    {
        _textTransform.DOKill();

        _canvas.enabled = false;
    }
}