using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class EnergyBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _slider;

    private RectTransform _transform;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
    }

    public void Show()
    {
        _transform.anchoredPosition = new Vector2(_transform.anchoredPosition.x, _transform.anchoredPosition.y + 500f);
        _transform.DOAnchorPosY(_transform.anchoredPosition.y - 500f, 1f);

        _canvas.enabled = true;
    }
    public void Hide()
    {
        _transform.DOAnchorPosY(_transform.anchoredPosition.y + 500f, 1f)
            .onComplete = () => _canvas.enabled = true;
    }

    public void ChangeValue(float value)
    {
        _slider.DOFillAmount(value, 1f);
        _transform.DOPunchScale(Vector3.one * 0.3f, 0.5f, 0, 1);

        if (value == 0)
            Hide();
    }
}