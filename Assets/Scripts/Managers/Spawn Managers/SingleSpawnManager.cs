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
        _player.SetEntranceDuration(_playerEntranceDuration);        

        if (_player.IsAlive())
        {
            _objectManager.StartReserve();
            StartCoroutine(AsteroidTriggerWave(_asteroidWaveDuration, _waveStartPause + _playerEntranceDuration, _waveStartPause, _enemySpawnPeriod, direction.down));
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
            _player.SetLaserContainer(_laserManager);
        }
    }

    protected override IEnumerator MainTimeline()
    {
        _player.StopRotation(Vector3.up, _waveStartPause / 4);
        _audio_Background.PlayDelayed(_waveStartPause / 4);
        while (_player.IsAlive())
        {
            yield return StartCoroutine(EnemyWave(_waveDuration, _waveStartPause, _waveEndPause, _enemySpawnPeriod, direction.down));
            if (!(_player.IsAlive()))
            {
                yield break;
            }
            StartCoroutine(PU_TripleFireWave(_waveDuration, direction.down));
            yield return StartCoroutine(EnemyWave(_waveDuration, 0, _waveEndPause, _enemySpawnPeriod, _enemySpawnPeriodDeviation, direction.down));
            if (!(_player.IsAlive()))
            {
                yield break;
            }
            StartCoroutine(PU_SpeedUpWave(_waveDuration, direction.down));
            yield return StartCoroutine(EnemyWave(_waveDuration, 0, _waveEndPause, _enemySpawnPeriod, _enemySpawnPeriodDeviation, direction.down));
            if (!(_player.IsAlive()))
            {
                yield break;
            }
            StartCoroutine(PU_ShieldWave(_waveDuration, direction.down));
            yield return StartCoroutine(EnemyWave(_waveDuration, 0, _waveEndPause, _enemySpawnPeriod, _enemySpawnPeriodDeviation, direction.down));
            if (!(_player.IsAlive()))
            {
                yield break;
            }
        }
    }
}
