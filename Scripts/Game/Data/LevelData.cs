using UnityEngine;

[CreateAssetMenu(menuName = "Game/Level")]
public class LevelData : ScriptableObject
{
    public ShakeCameraData ShakeCameraData => _shakeCameraData;

    public float LoseDelay => _loseDelay;
    public float WinDelay => _winDelay;
    public float TwinDuration => _twinDuration;

    public ParticleSystem WinEffector => _winEffector;
    public ParticleSystem BonusEffector => _bonusEffector;

    [Header("References")]
    [SerializeField] private ShakeCameraData _shakeCameraData;

    [Header("Time")]
    [Tooltip("Время ожидания после поражения")]
    [SerializeField] private float _loseDelay;
    [Tooltip("Время ожидания после победы")]
    [SerializeField] private float _winDelay;
    [Tooltip("Время действия бонуса двойник")]
    [SerializeField] private float _twinDuration;

    [Header("Effects")]
    [Tooltip("Эффект победы")]
    [SerializeField] private ParticleSystem _winEffector;
    [Tooltip("Эффект подбора бонуса")]
    [SerializeField] private ParticleSystem _bonusEffector;
}