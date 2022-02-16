using UnityEngine;

using Cinemachine;

[CreateAssetMenu(menuName = "Game/ShakeCamera")]
public class ShakeCameraData : ScriptableObject
{
    public float Intencity => _intencity;
    public float Duration => _duration;

    public float PlayerBallHitForce => _playerBallHitForce;
    public float WallBallHitForce => _wallBallHitForce;
    public float BonusBallHitForce => _bonusBallHitForce;

    [Header("General")]
    [Tooltip("Частота/интенсивность у тряски камеры")]
    [SerializeField] private float _intencity;
    [Tooltip("Время тряски")]
    [SerializeField] private float _duration;
    [Header("Custom")]
    [Tooltip("Сила тряски, при ударе мяча игроком")]
    [SerializeField] private float _playerBallHitForce;
    [Tooltip("Сила тряски, при ударе мяча об стену")]
    [SerializeField] private float _wallBallHitForce;
    [Tooltip("Сила тряски, при взятии бонуса")]
    [SerializeField] private float _bonusBallHitForce;

    public void Shake(CinemachineVirtualCamera camera, float force)
    {
        camera.DOShake(force, Intencity, Duration);
    }
}