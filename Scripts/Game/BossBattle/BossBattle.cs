using System;

using UnityEngine;

using Cinemachine;

public class BossBattle : MonoBehaviour
{
    public event Action Win;
    public event Action Lose;

    [Header("References")]
    [SerializeField] private EnergyBar _energyBar;
    [SerializeField] private CinemachineVirtualCamera _bossBattleCamera;

    private Vector3 _playerStartPlace;
    private Player _player;
    private Boss _boss;
    private Ball _ball;

    private bool _isStarting;

    public void StartBattle(Player player, BossBattleStart bossBattleStart)
    {
        _player = player;
        _boss = bossBattleStart.Boss;
        _playerStartPlace = bossBattleStart.PlayerPoint;

        var targetGroup = new GameObject().AddComponent<CinemachineTargetGroup>();
        targetGroup.AddMember(_player.transform, 1f, 0);
        targetGroup.AddMember(_boss.transform, 1f, 0);

        _bossBattleCamera.Follow = _player.transform;
        _bossBattleCamera.LookAt = targetGroup.transform;
        _bossBattleCamera.Priority += 10;

        _isStarting = true;
    }
    public void StopBattle()
    {
        _boss.Hit -= OnBossHit;

        _ball.PlayerOut -= OnBallPlayerOut;
        _ball.BossOut -= OnBallBossOut;
        _ball.Hide();

        _player.StopMove();
        _boss.StopMove();

        _bossBattleCamera.LookAt = _player.transform;

        HideEnergyBar();
    }

    private void FixedUpdate()
    {
        if (_isStarting == false) return;

        if (IsPlayerOnStartPlace())
            OnBossBattleStarted();
    }
    private bool IsPlayerOnStartPlace()
    {
        return _player.transform.position.z >= _playerStartPlace.z;
    }

    private void OnBossBattleStarted()
    {
        _isStarting = false;

        _boss.Hit += OnBossHit;

        ShowEnergyBar();

        _player.StopMove();
        _player.StartBossBattle();
        _boss.StartBossBattle();

        _ball = _player.SpawnBall();
        _ball.PlayerOut += OnBallPlayerOut;
        _ball.BossOut += OnBallBossOut;
        _boss.TrackBall(_ball);

        _bossBattleCamera.LookAt = _ball.transform;
    }
    private void OnBallPlayerOut()
    {
        OnLose();
    }
    private void OnBallBossOut()
    {
        OnWin();
    }
    private void OnBossHit()
    {
        UpdateEnergyBar();
    }

    private void OnWin()
    {
        _player.PlayWinAnimation();
        _boss.PlayLoseAnimation();

        StopBattle();

        Win?.Invoke();
    }
    private void OnLose()
    {
        _player.PlayLoseAnimation();
        _boss.PlayWinAnimation();

        StopBattle();

        Lose?.Invoke();
    }

    private void ShowEnergyBar()
    {
        _energyBar.Show();
        UpdateEnergyBar();
    }
    private void HideEnergyBar()
    {
        _energyBar.Hide();
    }
    private void UpdateEnergyBar()
    {
        var value = (float)_boss.Energy / _boss.MaxEnergy;
        _energyBar.ChangeValue(value);
    }
}