using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSpawnManager : SpawnManager
{
    protected override void Start()
    {
        base.Start();

        FindPlayer();

        _playerEntranceDuration = 5f;
        _waveStartPause += _playerEntranceDuration;
        _player.SetEntranceDuration(_playerEntranceDuration);        

        if (_player.IsAlive())
        {
            StartCoroutine(AsteroidTriggerWave(_waveStartPause + _waveDuration, _waveStartPause, _waveStartPause, _enemySpawnPeriod, WaveDirection.down));
        }
    }

    private void FindPlayer()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<SinglePlayer>();
        if (_player == null)
        {
            Debug.LogError("Spawn Manager could not locate Player.");
        }
        else
        {
            _player.SetLaserContainer(_laserContainer);
        }
    }

    protected override IEnumerator MainTimeline()
    {
        _player.StopRotation(Vector3.up, _waveStartPause / 4);
        _audio_Background.PlayDelayed(_waveStartPause / 4);
        while (_player.IsAlive())
        {
            yield return StartCoroutine(EnemyWave(_waveStartPause + _waveDuration, _waveStartPause, _waveEndPause, _enemySpawnPeriod, WaveDirection.down));
            if (!(_player.IsAlive()))
            {
                yield break;
            }
            StartCoroutine(PU_TripleFireWave(_waveStartPause + _waveEndPause + 2 * _waveDuration));
            yield return StartCoroutine(EnemyWave(_waveStartPause + _waveEndPause + 2 * _waveDuration, 0, _waveEndPause, _enemySpawnPeriod, _enemySpawnPeriodDeviation, WaveDirection.down));
            if (!(_player.IsAlive()))
            {
                yield break;
            }
            StartCoroutine(PU_SpeedUpWave(_waveStartPause + 2*_waveEndPause + 3 * _waveDuration));
            yield return StartCoroutine(EnemyWave(_waveStartPause + 2 * _waveEndPause + 3 * _waveDuration, 0, _waveEndPause, _enemySpawnPeriod, _enemySpawnPeriodDeviation, WaveDirection.down));
            if (!(_player.IsAlive()))
            {
                yield break;
            }
            StartCoroutine(PU_ShieldWave(_waveStartPause + 3 * _waveEndPause + 4 * _waveDuration));
            yield return StartCoroutine(EnemyWave(_waveStartPause + 3 * _waveEndPause + 4 * _waveDuration, 0, _waveEndPause, _enemySpawnPeriod, _enemySpawnPeriodDeviation, WaveDirection.down));
            if (!(_player.IsAlive()))
            {
                yield break;
            }
        }
    }
}
