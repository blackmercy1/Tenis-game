using System;

using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    public event Action<Ball> BallSpawned;
    public event Action Hit;

    public float Speed =>
        _data.DefaultSpeed + (Level.Value * _data.AddSpeedPerLevel) +
        (_isBoosted ? _data.AddBoostSpeed : 0);

    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private Transform _ballSpawnPoint;
    [SerializeField] private PlayerEntityData _data;
    [Header("Visual")]
    [SerializeField] private ModelsData _modelsData;

    [HideInInspector] public float MinBoundX;
    [HideInInspector] public float MaxBoundX;

    protected bool CanMove;
    protected bool CanMoveForward;
    protected Vector3 Velocity;
    protected bool IsBossBattle;

    private bool _isBoosted;
    private float _defaultRotation;

    protected virtual void Awake()
    {
        _defaultRotation = transform.rotation.eulerAngles.y;

        if (_modelsData.Models.Length != 0)
            SpawnRandomModel();

        ResetBounds();
    }

    private void SpawnRandomModel()
    {
        var randomIndex = UnityEngine.Random.Range(0, _modelsData.Models.Length);
        var randomModel = _modelsData.Models[randomIndex];

        SpawnModel(randomModel);
    }
    private void SpawnModel(PlayerModel model)
    {
        var modelObject = Instantiate(model.Model, transform);
        modelObject.transform.localPosition = Vector3.zero;

        var animator = modelObject.GetComponentInChildren<Animator>(true);
        Destroy(animator);

        _animator.avatar = model.Avatar;
        _animator.Rebind();
    }

    public void PlayWinAnimation()
    {
        _animator.SetTrigger("Win");
    }
    public void PlayLoseAnimation()
    {
        _animator.SetTrigger("Lose");
    }
    public void PlayDeathAnimation()
    {
        _animator.SetTrigger("CollideWithWall");
    }

    public virtual void StartBossBattle()
    {
        StartMove();
        StopMoveForward();

        MinBoundX *= 2;
        MaxBoundX *= 2;

        IsBossBattle = true;
    }
    public void ResetBounds()
    {
        MinBoundX = _data.MinBoundX;
        MaxBoundX = _data.MaxBoundX;
    }

    public void StartMove()
    {
        CanMove = true;
        CanMoveForward = true;
    }
    public void StopMove()
    {
        CanMove = false;
        StopMoveForward();

        Velocity = Vector3.zero;

        _animator.SetFloat("VelocityX", Velocity.x);
        _animator.SetFloat("VelocityZ", Velocity.z);
    }
    public void StopMoveForward()
    {
        _isBoosted = false;
        CanMoveForward = false;
    }
    public void BoostMove()
    {
        _isBoosted = true;
    }
    public Ball SpawnBall()
    {
        var ball = Instantiate(_ballPrefab, _ballSpawnPoint.position, Quaternion.identity);
        var ballCollider = ball.GetComponent<SphereCollider>();

        BallSpawned?.Invoke(ball);

        HitBall(ball);

        return ball;
    }
    public virtual void HitBall(Ball ball)
    {
        if (IsBossBattle)
            ball.ChangeDirectionWithRandom(transform.forward);
        else ball.ChangeDirection(Vector3.forward);

        _animator.SetTrigger("HitBall");

        ball.SpawnHitEffector();
        Hit?.Invoke();
    }

    protected virtual void Update()
    {
        if (CanMove == false) return;

        _animator.SetFloat("VelocityX", Velocity.x);

        if (CanMoveForward)
        {
            _animator.SetFloat("VelocityZ", Velocity.z);

            var forwardVelocity = Vector3.forward * Velocity.z * Time.deltaTime;
            transform.position += forwardVelocity;
        }

        var rotationAngle = Velocity.x * _data.RotationAngleRange;
        transform.rotation = Quaternion.Euler(0, rotationAngle + _defaultRotation, 0);

        // var acceleration = _acceleration * Time.deltaTime;
        // Velocity.z = Mathf.Clamp(Velocity.z + acceleration, 0, _maxSpeed);
        Velocity.z = Speed;
        Velocity.x = Mathf.Lerp(Velocity.x, 0, Time.deltaTime / _data.TimeToStopHorizontalAnimation);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_modelsData.Models.Length == 0) return;

        var randomIndex = UnityEngine.Random.Range(0, _modelsData.Models.Length);
        var randomModel = _modelsData.Models[randomIndex];

        var mesh = randomModel.Model.GetComponentInChildren<SkinnedMeshRenderer>(true).sharedMesh;
        Gizmos.DrawMesh(mesh, transform.position, transform.rotation, transform.lossyScale);
    }
#endif
}