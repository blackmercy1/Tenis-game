using System.Collections.Generic;

using UnityEngine;

using Cinemachine;
using DG.Tweening;

public class GameLevel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private BossBattle _bossBattle;
    [SerializeField] private CinemachineVirtualCamera _gameCamera;
    [SerializeField] private CinemachineVirtualCamera _bossCamera;
    [SerializeField] private LevelData _data;

    private Player Player => _players[0];

    private List<Player> _players = new List<Player>();
    private List<Ball> _balls = new List<Ball>();

    private bool _isNormalPlaying;
    private CinemachineTargetGroup _targetGroup;

    private void Awake()
    {
        Level.Load();
        Score.Load();

        _targetGroup = new GameObject().AddComponent<CinemachineTargetGroup>();
        _gameCamera.LookAt = _targetGroup.transform;
    }
    private void Start()
    {
        LevelGenerator.Instance.Generate();
        SpawnStartPlayer();

        ScoreView.Instance.Show();
        LevelView.Instance.Show();
        Tutorial.Instance.Show();
    }
    private void FixedUpdate()
    {
        if (_isNormalPlaying == false) return;

        CheckBallsOut();

        var countBalls = GetCountBalls();
        if (countBalls == 0)
            SpeedUpPlayers();
    }

    private void SpawnStartPlayer()
    {
        var player = SpawnPlayer(_playerSpawnPoint.position);

        player.Started += OnGameStarted;
        _gameCamera.Follow = player.transform;
    }
    private Player SpawnTwin()
    {
        var twin = SpawnPlayer(Player.transform.position);
        twin.transform.DOMoveX(Player.transform.position.x + 4f, 0.2f)
            .onComplete = () =>
            {
                twin.transform.position = new Vector3(
                    twin.transform.position.x,
                    twin.transform.position.y,
                    Player.transform.position.z
                );
                twin.StartMove();
            };

        Player.MaxBoundX /= 2f;
        twin.MinBoundX /= 2f;

        return twin;
    }
    private Player SpawnPlayer(Vector3 position)
    {
        var player = Instantiate(_playerPrefab, position, Quaternion.identity);
        AddPlayer(player);

        return player;
    }
    private void AddPlayer(Player player)
    {
        player.Hit += OnPlayerHitBall;

        _targetGroup.AddMember(player.transform, 1f, 0f);
        _players.Add(player);

        player.BallSpawned += OnBallSpawned;
        player.CollideWithWall += OnPlayerCollideWithWall;
        player.CollideWithBossBattleStart += OnPlayerCollideWithBossBattleStart;
    }
    private void RemovePlayer(Player player)
    {
        _players.Remove(player);

        if (_players.Count != 0)
        {
            _targetGroup.RemoveMember(player.transform);
            _gameCamera.Follow = Player.transform;

            Player.ResetBounds();
        }
    }
    private void MergePlayers()
    {
        for (var i = 1; i < _players.Count; i++)
        {
            var player = _players[i];
            MergePlayer(player);
        }
    }
    private void MergePlayer(Player player)
    {
        DOTween.Sequence()
                .Join(player.transform.DOMoveX(Player.transform.position.x, 0.1f))
                .AppendCallback(() =>
                {
                    player.StopMove();
                    RemovePlayer(player);

                    player.gameObject.SetActive(false);
                });
    }
    private void SpeedUpPlayers()
    {
        foreach (var player in _players)
            player.BoostMove();
    }

    private void CheckBallsOut()
    {
        var balls = _balls.ToArray();

        foreach (var ball in balls)
        {
            if (IsBallOut(ball))
                ball.Hide();
        }

        if (_balls.Count == 0)
            OnBallsOut();
    }
    private bool IsBallOut(Ball ball)
    {
        return ball.transform.position.z < Player.transform.position.z - 8f;
    }
    private int GetCountBalls()
    {
        var count = 0;

        foreach (var ball in _balls)
        {
            if (ball != null && ball.gameObject.activeInHierarchy)
                count++;
        }

        return count;
    }

    private void OnGameStarted()
    {
        StartPlay();
    }
    private void OnBallSpawned(Ball ball)
    {
        _balls.Add(ball);
        ball.HitWall += OnBallHitWall;
        ball.HitBonus += OnBallHitBonus;
        ball.Destroying += OnBallDestroying;

        _targetGroup.AddMember(ball.transform, 0.1f, 0f);
    }
    private void OnBallDestroying(Ball ball)
    {
        ball.HitWall -= OnBallHitWall;
        ball.HitBonus -= OnBallHitBonus;
        ball.Destroying -= OnBallDestroying;

        _balls.Remove(ball);

        _targetGroup.RemoveMember(ball.transform);
    }

    private void OnBallHitWall()
    {
        ShakeCamera(_data.ShakeCameraData.WallBallHitForce);
    }
    private void OnBallHitBonus(Ball ball, Bonus bonus)
    {
        Instantiate(_data.BonusEffector, bonus.transform.position, Quaternion.identity);

        if (bonus is AddBallBonus)
            AddBall();
        else if (bonus is FireBallBonus)
            ball.EnableFireballMode();
        else if (bonus is TwinBonus)
            ActivateTwin();

        Destroy(bonus.gameObject);

        ShakeCamera(_data.ShakeCameraData.BonusBallHitForce);
    }
    private void OnBallsOut()
    {
        foreach (var player in _players)
            player.PlayLoseAnimation();

        OnLose();
    }

    private void OnPlayerHitBall()
    {
        ShakeCamera(_data.ShakeCameraData.PlayerBallHitForce);
    }
    private void OnPlayerCollideWithWall(Player player)
    {
        player.StopMove();
        player.PlayDeathAnimation();

        RemovePlayer(player);

        if (_players.Count == 0)
            OnLose();
    }
    private void OnPlayerCollideWithBossBattleStart(BossBattleStart bossBattleStart)
    {
        StartBossBattle(bossBattleStart);
    }

    private void StartBossBattle(BossBattleStart bossBattleStart)
    {
        ScoreView.Instance.Hide();
        LevelView.Instance.Hide();

        if (_players.Count > 1)
            MergePlayers();

        StopPlay();

        _bossBattle.StartBattle(Player, bossBattleStart);
        _bossBattle.Lose += OnLose;
        _bossBattle.Win += OnWin;
    }

    private void OnLose()
    {
        StopMove();

        StopPlay();
        Invoke(nameof(Lose), _data.LoseDelay);
    }
    private void OnWin()
    {
        Instantiate(_data.WinEffector, Player.transform.position + Vector3.up * 5f, Quaternion.identity);

        StopMove();

        StopPlay();
        Invoke(nameof(Win), _data.WinDelay);
    }
    private void StopMove()
    {
        foreach (var player in _players)
            player.StopMove();
    }

    private void StartPlay()
    {
        Tutorial.Instance.Hide();

        Player.SpawnBall();
        Player.StartMove();

        _isNormalPlaying = true;
    }
    private void StopPlay()
    {
        HideBalls();

        if (_players.Count > 0)
        {
            Player.BallSpawned -= OnBallSpawned;
            Player.CollideWithWall -= OnPlayerCollideWithWall;
            Player.CollideWithBossBattleStart -= OnPlayerCollideWithBossBattleStart;
        }
        

        _isNormalPlaying = false;
    }

    private void Win()
    {
        WinScreen.Instance.Show();

        Level.Value++;
        Level.Save();
        Score.Save();
    }
    private void Lose()
    {
        LoseScreen.Instance.Show();
    }

    private void AddBall()
    {
        Player.SpawnBall();
    }
    private void ActivateTwin()
    {
        if (_players.Count != 1)
            CancelInvoke(nameof(MergePlayers));
        else SpawnTwin();

        Invoke(nameof(MergePlayers), _data.TwinDuration);
    }

    private void HideBalls()
    {
        var balls = _balls.ToArray();

        foreach (var ball in balls)
            ball.Hide();
    }
    private void ShakeCamera(float force)
    {
        if (_isNormalPlaying)
            _data.ShakeCameraData.Shake(_gameCamera, force);
        else _data.ShakeCameraData.Shake(_bossCamera, force);
    }
}