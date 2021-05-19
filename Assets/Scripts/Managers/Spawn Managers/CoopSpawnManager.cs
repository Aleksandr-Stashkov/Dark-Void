using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopSpawnManager : SpawnManager
{
    private Player _player2;    

    protected override void Start()
    {
        base.Start();

        FindPlayers();

        _playerEntranceDuration = 7f;
        _player.SetEntranceDuration(_playerEntranceDuration);
        _player2.SetEntranceDuration(_playerEntranceDuration);

        if (_player.IsAlive() && _player2.IsAlive())
        {
            StartCoroutine(AsteroidTriggerWave(_asteroidWaveDuration, _waveStartPause + _playerEntranceDuration, _waveStartPause, _enemySpawnPeriod, direction.down));
        }
    }

    private void FindPlayers()
    {
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length == 2)
        {
            _player = players[0].GetComponent<CoopPlayer>();
            if (_player.IsPlayer1())
            {
                _player2 = players[1].GetComponent<CoopPlayer>();
                if (_player2.IsPlayer1())
                {
                    Debug.LogWarning("Two players are set as Player 1.");
                }
            }
            else
            {
                _player2 = _player;
                _player = players[1].GetComponent<CoopPlayer>();
                if (!_player.IsPlayer1())
                {
                    Debug.LogWarning("Neither of players is set as Player 1.");
                }
            }
        }
        else
        {
            Debug.LogWarning("The number of players is other than two.");
        }

        if (_player == null)
        {
            Debug.LogError("Spawn Manager could not locate Player 1.");
        }
        else
        {
            _player.SetLaserContainer(_laserManager);
        }
        if (_player2 == null)
        {
            Debug.LogError("Spawn Manager could not locate Player 2.");
        }
        else
        {
            _player2.SetLaserContainer(_laserManager);
        }
    }

    protected override IEnumerator MainTimeline()
    {       
        _audio_Background.PlayDelayed(_waveStartPause / 4);
        while (_player.IsAlive() && _player2.IsAlive())
        {
            yield return StartCoroutine(EnemyWave(_waveDuration, _waveStartPause, _waveEndPause, _enemySpawnPeriod, direction.down));
            if (!(_player.IsAlive() && _player2.IsAlive()))
            {
                yield break;
            }
            StartCoroutine(PU_TripleFireWave(_waveDuration, direction.down));
            yield return StartCoroutine(EnemyWave(_waveDuration, 0, _waveEndPause, _enemySpawnPeriod, _enemySpawnPeriodDeviation, direction.down));
            if (!(_player.IsAlive() && _player2.IsAlive()))
            {
                yield break;
            }
            StartCoroutine(PU_SpeedUpWave(_waveDuration, direction.down));
            yield return StartCoroutine(EnemyWave(_waveDuration, 0, _waveEndPause, _enemySpawnPeriod, _enemySpawnPeriodDeviation, direction.down));
            if (!(_player.IsAlive() && _player2.IsAlive()))
            {
                yield break;
            }
            StartCoroutine(PU_ShieldWave(_waveDuration, direction.down));
            yield return StartCoroutine(EnemyWave(_waveDuration, 0, _waveEndPause, _enemySpawnPeriod, _enemySpawnPeriodDeviation, direction.down));
            if (!(_player.IsAlive() && _player2.IsAlive()))
            {
                yield break;
            }
        }
    }
}
