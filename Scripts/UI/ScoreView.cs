using UnityEngine;

using TMPro;
using DG.Tweening;

public class ScoreView : MonoBehaviour
{
    public static ScoreView Instance;

    [Header("References")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _textField;
    [Header("Parameters")]
    [SerializeField] private string _textFormat = "{0}";

    private RectTransform _transform;

    private int TextScore
    {
        get => _textScore;
        set
        {
            _textScore = value;
            UpdateScoreText(value);
        }
    }
    private int _textScore;

    private Tween _punchScaleTween;

    private void Awake()
    {
        Instance = this;

        _transform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        _punchScaleTween = _transform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 0, 1)
            .SetAutoKill(false)
            .Pause();
    }

    private void OnEnable()
    {
        Score.Changed += OnScoreChanged;
        OnScoreChanged(Score.Value);
    }
    private void OnDisable()
    {
        Score.Changed -= OnScoreChanged;
    }

    public void Show()
    {
        _canvas.enabled = true;

        _transform.anchoredPosition = new Vector2(_transform.anchoredPosition.x - 1000f, _transform.anchoredPosition.y);
        _transform.DOAnchorPosX(_transform.anchoredPosition.x + 1000f, 1f);
    }
    public void Hide()
    {
        _transform.DOAnchorPosX(_transform.anchoredPosition.x - 1000f, 1f)
            .onComplete = () => _canvas.enabled = false;
    }

    private void OnScoreChanged(int score)
    {
        DOTween.To(() => TextScore, value => TextScore = value, score, 0.5f);

        _punchScaleTween.Restart();
    }
    private void UpdateScoreText(int score)
    {
        _textField.text = string.Format(_textFormat, score);
    }
}