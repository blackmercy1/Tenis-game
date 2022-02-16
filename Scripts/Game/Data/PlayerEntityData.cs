using UnityEngine;

[CreateAssetMenu(menuName = "Game/PlayerEntity")]
public class PlayerEntityData : ScriptableObject
{
    public float DefaultSpeed => _defaultSpeed;
    public float AddSpeedPerLevel => _addSpeedPerLevel;
    public float AddBoostSpeed => _addBoostSpeed;

    public float MinBoundX => _minBoundX;
    public float MaxBoundX => _maxBoundX;

    public float RotationAngleRange => _rotationAngleRange;
    public float TimeToStopHorizontalAnimation => _timeToStopHorizontalAnimation;

    [Header("Physics")]
    [Tooltip("Скорость персонажа для первого уровня")]
    [SerializeField] private float _defaultSpeed;
    [Tooltip("Количество добавляемой скорости для персонажа за каждый уровень")]
    [SerializeField] private float _addSpeedPerLevel;
    [Tooltip("Добавляемая скорость для ускорения, когда все мячи дошли до босса")]
    [SerializeField] private float _addBoostSpeed;
    [Header("Bounds")]
    [Tooltip("Левая граница уровня")]
    [SerializeField] private float _minBoundX;
    [Tooltip("Правая граница уровня")]
    [SerializeField] private float _maxBoundX;
    [Header("Animation")]
    [Tooltip("Максимальный угол визуального поворота персонажа, задаётся от 0 до 90, используется от -90 до 90")]
    [SerializeField, Range(0, 90)] private float _rotationAngleRange;
    [Tooltip("Время за которое персонаж должен сбросить до 0 горизонтальную анимацию ходьбы (актуально в битве с боссом)")]
    [SerializeField] private float _timeToStopHorizontalAnimation;
}