using UnityEngine;

public class Boss : PlayerEntity
{
    public int MaxEnergy { get; private set; }
    public int Energy { get; private set; }

    [Header("References")]
    [SerializeField] private BossData _bossData;

    private Ball _ball;

    private void Start()
    {
        Energy = MaxEnergy = _bossData.DefaultEnergy + (Level.Value * _bossData.AddEnergyPerLevel);
    }
    private void FixedUpdate()
    {
        if (Energy == 0 || _ball == null) return;

        var delta = (transform.position.x - _ball.transform.position.x) * Time.fixedDeltaTime * 10f;
        var newPosition = transform.position + (Vector3.left * delta);

        newPosition.x = Mathf.Clamp(newPosition.x, MinBoundX, MaxBoundX);
        delta = transform.position.x - newPosition.x;

        transform.position = newPosition;

        Velocity.x = Mathf.Clamp(Velocity.x + delta, -1, 1);
    }

    public void TrackBall(Ball ball)
    {
        _ball = ball;
    }
    public override void HitBall(Ball ball)
    {
        if (IsBossBattle == false) return;
        if (Energy == 0) return;

        Energy--;

        base.HitBall(ball);

        if (Energy == 0)
            StopMove();
    }
}