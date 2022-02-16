using UnityEngine;

public class BossBattleStart : MonoBehaviour
{
    public Boss Boss => _boss;
    public Vector3 PlayerPoint => _playerPoint.position;

    [Header("References")]
    [SerializeField] private Boss _boss;
    [SerializeField] private Transform _playerPoint;
}