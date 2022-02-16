using UnityEngine;

using DG.Tweening;

[RequireComponent(typeof(MeshRenderer))]
public class Wall : MonoBehaviour
{
    public bool IsDestructed { get; private set; }

    [Header("References")]
    [SerializeField] private Transform _popupSpawnPoint;
    [SerializeField] private WallData _data;
    [Header("Parameters")]
    [SerializeField] private int _countHitsToDestruct;

    private int _health;
    private MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _health = _countHitsToDestruct;
    }
    private void Start()
    {
        ChangeColorByHealth();
    }

    public void Hit()
    {
        if (IsDestructed) return;

        _health--;

        if (_health <= 0)
        {
            Destruct();
        }
        else
        {
            ChangeColorByHealth();
            AddScore(_data.ScoreForHit);
        }
    }
    public void Destruct()
    {
        if (IsDestructed) return;

        AddScore(_data.ScoreForDestruct);

        var destructedWall = Instantiate(_data.DesturctedPrefab, transform.position, transform.rotation);
        var currentColor = _renderer.material.color;

        destructedWall.ChangeColor(currentColor);
        destructedWall.Explode(_data.ExplodeForce, _data.ExplodeUpwardsForceScale);

        Destroy(gameObject);

        IsDestructed = true;
    }

    private void ChangeColorByHealth()
    {
        var color = _data.ColorsData.Colors[_health - 1];
        ChangeColor(color);
    }
    private void ChangeColor(Color color)
    {
        _renderer.material.DOColor(color, _data.ColorChangeDuration);
    }

    private void AddScore(int score)
    {
        Score.Value += score;

        var popup = Instantiate(_data.ScorePopupPrefab, _popupSpawnPoint.position, Quaternion.identity);
        popup.Initialize(score);
    }
}