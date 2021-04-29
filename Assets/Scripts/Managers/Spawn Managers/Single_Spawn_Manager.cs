using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_Spawn_Manager : Spawn_Manager
{
    protected override void Start()
    {
        base.Start();

        FindPlayer();
        //Player entrance pause
        _playerEntranceDuration = 5f;
        _waveStartPause += _playerEntranceDuration;
        player.SetEntranceDuration(_playerEntranceDuration);        

        if (player.IsAlive())
        {
            StartCoroutine(AsteroidTriggerWave(_waveStartPause + _waveDuration, _waveStartPause, _waveStartPause, _enemySpawnPeriod, WaveDirection.down));
        }
    }

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Single_Player>();
        if (player == null)
        {
            Debug.LogError("Spawn Manager could not locate Player.");
        }
        else
        {
            player.SetSpawnManager(this);
        }
    }

    //Main wave sequaence
    protected override IEnumerator MainTimeline()
    {
        player.StopRotation(Vector3.up, _waveStartPause / 4);
        _audio_Background.PlayDelayed(_waveStartPause / 4);
        while (player.IsAlive())
        {
            yield return StartCoroutine(EnemyWave(_waveStartPause + _waveDuration, _waveStartPause, _waveEndPause, _enemySpawnPeriod, WaveDirection.down));
            if (!(player.IsAlive()))
            {
                yield break;
            }
            StartCoroutine(PU_TripleFireWave(_waveStartPause + _waveEndPause + 2 * _waveDuration));
            yield return StartCoroutine(EnemyWave(_waveStartPause + _waveEndPause + 2 * _waveDuration, 0, _waveEndPause, _enemySpawnPeriod, _enemySpawnPeriodDeviation, WaveDirection.down));
            if (!(player.IsAlive()))
            {
                yield break;
            }
            StartCoroutine(PU_SpeedUpWave(_waveStartPause + 2*_waveEndPause + 3 * _waveDuration));
            yield return StartCoroutine(EnemyWave(_waveStartPause + 2 * _waveEndPause + 3 * _waveDuration, 0, _waveEndPause, _enemySpawnPeriod, _enemySpawnPeriodDeviation, WaveDirection.down));
            if (!(player.IsAlive()))
            {
                yield break;
            }
            StartCoroutine(PU_ShieldWave(_waveStartPause + 3 * _waveEndPause + 4 * _waveDuration));
            yield return StartCoroutine(EnemyWave(_waveStartPause + 3 * _waveEndPause + 4 * _waveDuration, 0, _waveEndPause, _enemySpawnPeriod, _enemySpawnPeriodDeviation, WaveDirection.down));
            if (!(player.IsAlive()))
            {
                yield break;
            }
        }
    }
}
