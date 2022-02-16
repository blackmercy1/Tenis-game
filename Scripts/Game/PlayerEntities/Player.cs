using System;

using UnityEngine;

public class Player : PlayerEntity
{
    public event Action Started;
    public event Action<Player> CollideWithWall;
    public event Action<BossBattleStart> CollideWithBossBattleStart;

    private bool _isTappedOnce;

    public void Move(Vector3 delta)
    {
        if (_isTappedOnce == false)
        {
            _isTappedOnce = true;
            Started?.Invoke();
        }

        if (CanMove == false) return;

        if (IsBossBattle)
            delta *= 2.5f;

        var horizontalDelta = Vector3.right * delta.x;
        var newPosition = transform.position + horizontalDelta;

        newPosition.x = Mathf.Clamp(newPosition.x, MinBoundX, MaxBoundX);

        var positionDelta = newPosition - transform.position;
        transform.position = newPosition;

        Velocity.x = Mathf.Clamp(Velocity.x + positionDelta.x, -1f, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CanMove == false) return;

        if (other.TryGetComponent(out Wall _))
            OnCollideWithWall();
        else if (other.TryGetComponent(out BossBattleStart bossBattleStart))
            OnCollideWithBossBattleStart(bossBattleStart);
    }
    private void OnCollideWithWall()
    {
        CollideWithWall?.Invoke(this);
    }
    private void OnCollideWithBossBattleStart(BossBattleStart bossBattleStart)
    {
        CollideWithBossBattleStart?.Invoke(bossBattleStart);
    }
}