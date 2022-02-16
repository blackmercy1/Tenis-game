using UnityEngine;

[CreateAssetMenu(menuName = "Game/Wall")]
public class WallData : ScriptableObject
{
    public DestructedWall DesturctedPrefab => _destructedPrefab;
    public float ExplodeForce => _explodeForce;
    public float ExplodeUpwardsForceScale => _explodeUpwardsForceScale;

    public ScorePopup ScorePopupPrefab => _scorePopupPrefab;
    public int ScoreForHit => _scoreForHit;
    public int ScoreForDestruct => _scoreForDestruct;

    public float ColorChangeDuration => _colorChangeDuration;
    public ColorsData ColorsData => _colorsData;

    [Header("Explosion")]
    [Tooltip("Префаб с разрушеной стеной")]
    [SerializeField] private DestructedWall _destructedPrefab;
    [Tooltip("Сила взрыва")]
    [SerializeField] private float _explodeForce;
    [Tooltip("Множитель подъёмной силы взрыва")]
    [SerializeField] private float _explodeUpwardsForceScale;
    [Header("Score")]
    [Tooltip("Префаб popup-а для очков")]
    [SerializeField] private ScorePopup _scorePopupPrefab;
    [Tooltip("Количество добавляемых очков за хит по стене")]
    [SerializeField] private int _scoreForHit;
    [Tooltip("Количество добавляемых очков за уничтожение стены")]
    [SerializeField] private int _scoreForDestruct;
    [Header("Colors")]
    [Tooltip("Длительность смены цвета стены")]
    [SerializeField] private float _colorChangeDuration;
    [Tooltip("Цвета стен в зависимости от прочности стены (порядок важен)")]
    [SerializeField] private ColorsData _colorsData;
}