using System;

using UnityEngine;

using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    public event Action PlayerOut;
    public event Action BossOut;
    public event Action HitWall;
    public event Action<Ball> Destroying;
    public event Action<Ball, Bonus> HitBonus;

    public bool IsHiding => _isHiding;

    [Header("References")]
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private BallData _data;

    private float Speed =>
        _data.DefaultSpeed + (_data.AddSpeedPerLevel * Level.Value) +
        (_isFireball ? _data.FireballAddSpeed : 0);
    private Vector3 Direction
        => _velocity.normalized;

    private Rigidbody _rigidbody;
    private Vector3 _velocity;

    private int _countWallsDestructedWithFireball;
    private float _defaultTrailTime;
    private Color _defaultColor;
    private Color _defaultTrailColor;

    private bool _isHiding;
    private bool _isFireball;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _defaultTrailTime = _trailRenderer.time;
        _defaultColor = _renderer.material.color;
        _defaultTrailColor = _trailRenderer.material.color;
    }
    private void FixedUpdate()
    {
        _rigidbody.velocity = _velocity;
    }
    private void OnDestroy()
    {
        Destroying?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isHiding) return;

        var gameObject = collision.gameObject;

        if (gameObject.TryGetComponent(out Wall wall))
            OnCollideWithWall(collision, wall);
        else if (gameObject.TryGetComponent(out PlayerEntity _))
            return;
        else Bounce(collision);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_isHiding) return;

        var playerEntity = other.GetComponentInParent<PlayerEntity>();
        if (playerEntity != null)
            OnCollideWithPlayer(playerEntity);
        else if (other.TryGetComponent(out BossBattleStart _))
            OnCollideWithBossBattleStart();
        else if (other.TryGetComponent(out PlayerOutZone _))
            OnCollideWithPlayerOutZone();
        else if (other.TryGetComponent(out BossOutZone _))
            OnCollideWithBossOutZone();
        else if (other.TryGetComponent(out Bonus bonus))
            OnCollideWithBonus(bonus);
    }
    private void OnCollideWithWall(Collision collision, Wall wall)
    {
        if (_isFireball == false)
            Bounce(collision);

        if (_isFireball)
        {
            wall.Destruct();
            _countWallsDestructedWithFireball++;

            if (_countWallsDestructedWithFireball == _data.CountWallToDisableFireball)
                DisableFireballMode();
        }
        else wall.Hit();

        HitWall?.Invoke();
    }
    private void OnCollideWithPlayer(PlayerEntity player)
    {
        player.HitBall(this);
    }
    private void OnCollideWithBossBattleStart()
    {
        Hide(false);
    }
    private void OnCollideWithPlayerOutZone()
    {
        PlayerOut?.Invoke();
    }
    private void OnCollideWithBossOutZone()
    {
        BossOut?.Invoke();
    }
    private void OnCollideWithBonus(Bonus bonus)
    {
        HitBonus?.Invoke(this, bonus);
    }

    private void Bounce(Collision collision)
    {
        var firstContact = collision.GetContact(0);
        var normal = firstContact.normal;

        foreach (var contact in collision.contacts)
            normal += contact.normal;

        normal = normal.normalized;

        var reflectedDirection = ReflectDirection(normal);
        var newDirection = RandomRotateDirection(reflectedDirection.normalized);

        _lastDirection = Direction;

        ChangeDirection(newDirection);
        SpawnHitEffector();

        _lastReflection = reflectedDirection;
        _lastNewDirection = newDirection;
        _lastPoint = firstContact.point;

    }

    public void Hide(bool destroy = true)
    {
        if (_isHiding) return;
        else _isHiding = true;

        _velocity = Vector3.zero;
        _renderer.material.DOFade(0, _data.HideDuration)
            .onComplete = () => gameObject.SetActive(false);

        if (destroy)
            Destroying?.Invoke(this);
    }
    public void ChangeDirectionWithRandom(Vector3 direction)
    {
        var newDirection = RandomRotateDirection(direction.normalized);
        ChangeDirection(newDirection);
    }
    public void ChangeDirection(Vector3 direction)
    {
        _velocity = direction.normalized * Speed;
    }
    public void UpdateVelocity()
    {
        _velocity = Direction * Speed;
    }
    public void SpawnHitEffector()
    {
        Instantiate(_data.HitEffector, transform.position, Quaternion.identity);
    }

    public void EnableFireballMode()
    {
        _isFireball = true;

        _countWallsDestructedWithFireball = 0;

        var colorOffset = new Color(1.8f, 0.3f, 0.3f, 1f);
        var fireballColor = _renderer.material.color * colorOffset;
        var fireballTrailColor = _trailRenderer.material.color * colorOffset;

        _particleSystem.Play();

        _renderer.material.DOColor(fireballColor, 3f);
        _trailRenderer.material.DOColor(fireballTrailColor, 3f);
        _trailRenderer.DOTime(0.6f, 3f);

        UpdateVelocity();
    }
    public void DisableFireballMode()
    {
        _isFireball = false;

        _particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);

        _renderer.material.DOColor(_defaultColor, 3f);
        _trailRenderer.material.DOColor(_defaultTrailColor, 3f);
        _trailRenderer.DOTime(_defaultTrailTime, 3f);

        UpdateVelocity();
    }

    private Vector3 ReflectDirection(Vector3 normal)
    {
        return Vector3.Reflect(Direction, normal);
    }
    private Vector3 RandomRotateDirection(Vector3 direction)
    {
        var randomAngle = UnityEngine.Random.Range(-_data.RandomBounceAngleRange, _data.RandomBounceAngleRange);
        var rotatedDirection = Quaternion.Euler(0, randomAngle, 0) * direction;

        return rotatedDirection;
    }

    private Vector3 _lastReflection;
    private Vector3 _lastDirection;
    private Vector3 _lastNewDirection;
    private Vector3 _lastPoint;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_lastPoint, _lastReflection);
        Gizmos.DrawRay(_lastPoint, -_lastDirection);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_lastPoint, _lastNewDirection);
    }
}