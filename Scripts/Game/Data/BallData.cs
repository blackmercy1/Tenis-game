using UnityEngine;

[CreateAssetMenu(menuName = "Game/Ball")]
public class BallData : ScriptableObject
{
    public float DefaultSpeed => _defaultSpeed;
    public float AddSpeedPerLevel => _addSpeedPerLevel;
    public float RandomBounceAngleRange => _randomBounceAngleRange;

    public float CountWallToDisableFireball => _countWallToDisableFireball;
    public float FireballAddSpeed => _fireballAddSpeed;

    public float HideDuration => _hideDuration;
    public ParticleSystem HitEffector => _hitEffector;

    [Header("Physics")]
    [Tooltip("Скорость для первого уровня")]
    [SerializeField] private float _defaultSpeed;
    [Tooltip("Добавление скорости за каждый уровень")]
    [SerializeField] private float _addSpeedPerLevel;
    [Tooltip("Рандомный угол отскока. Указывается от 0 до 90, но рандомиться будет от -90 до 90")]
    [SerializeField, Range(0, 90)] private float _randomBounceAngleRange;
    [Header("Fireball")]
    [Tooltip("Количество стен, которое может уничтожить шарик в fireball режиме")]
    [SerializeField] private float _countWallToDisableFireball;
    [Tooltip("Добавление скорости, если мяч в fireball режиме")]
    [SerializeField] private float _fireballAddSpeed;
    [Header("Animation")]
    [Tooltip("Время за которое происходит анимация скрытия мячика")]
    [SerializeField] private float _hideDuration;
    [Header("Effects")]
    [Tooltip("Эффект хита для мячика (удар игрока, отскок от стен)")]
    [SerializeField] private ParticleSystem _hitEffector;
}