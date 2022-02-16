using UnityEngine;

using TMPro;
using DG.Tweening;

public class LevelView : MonoBehaviour
{
    public static LevelView Instance;

    [Header("References")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _textField;
    [Header("Parameters")]
    [SerializeField] private string _textFormat = "Level {0}";

    private RectTransform _transform;

    private void Awake()
    {
        Instance = this;

        _transform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        _textField.text = string.Format(_textFormat, Level.Value + 1);
    }

    public void Show()
    {
        _canvas.enabled = true;

        _transform.anchoredPosition = new Vector2(_transform.anchoredPosition.x + 1000f, _transform.anchoredPosition.y);
        _transform.DOAnchorPosX(_transform.anchoredPosition.x - 1000f, 1f);
    }
    public void Hide()
    {
        _transform.DOAnchorPosX(_transform.anchoredPosition.x + 1000f, 1f)
            .onComplete = () => _canvas.enabled = false;
    }
}