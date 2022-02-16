using UnityEngine;

using TMPro;
using DG.Tweening;

[RequireComponent(typeof(TextMeshPro))]
public class ScorePopup : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private string _format = "+{0}";

    private Transform _transform;
    private TextMeshPro _textField;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _textField = GetComponent<TextMeshPro>();
    }

    public void Initialize(int score)
    {
        _textField.text = string.Format(_format, score);

        _transform.DOJump(_transform.position + (Vector3.up * 5f), 1, 1, 1f);
        _transform.DOPunchScale(Vector3.one, 1f, 0, 1);
        _textField.DOFade(0, 1f)
            .onComplete = () => Destroy(gameObject);
    }
}