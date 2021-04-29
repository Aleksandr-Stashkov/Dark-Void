using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coop_Spawn_Manager : Spawn_Manager
{
    [HideInInspector]
    public Player player2;
    

    protected override void Start()
    {
        base.Start();

        FindPlayers();
        //Players entrance pause
        _playerEntranceDuration = 7f;
        _waveStartPause += _playerEntranceDuration;
        player.SetEntranceDuration(_playerEntranceDuration);
        player2.SetEntranceDuration(_playerEntranceDuration);

        if (player.IsAlive() && player2.IsAlive())
        {
            StartCoroutine(AsteroidTriggerWave(_waveStartPause + _waveDuration, _waveStartPause, _waveStartPause, _enemySpawnPeriod,WaveDirection.down));
        }
    }

    //Identifying players
    private void FindPlayers()
    {
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length == 2)
        {
            player = players[0].GetComponent<Coop_Player>();
            if (player.IsPlayer1())
            {
                player2 = players[1].GetComponent<Coop_Player>();
            }
            else
            {
                player2 = player;
                player = players[1].GetComponent<Coop_Player>();
            }
        }
        else
        {
            Debug.LogError("The number of players is other than two.");
        }

        if (player == null)
        {
            Debug.LogError("Spawn Manager could not locate Player 1.");
        }
        else
        {
            player.SetSpawnManager(this);
        }
        if (player2 == null)
        {
            Debug.LogError("Spawn Manager could not locate Player 2.");
        }
        else
        {
            player2.SetSpawnManager(this);
        }
    }

    //Main wave sequaence
    protected override IEnumerator MainTimeline()
    {       
        _audio_Background.PlayDelayed(_waveStartPause / 4);
        while (player.IsAlive() && player2.IsAlive())
        {
            yield return StartCoroutine(EnemyWave(_waveStartPause + _waveDuration, _waveStartPause, _waveEndPause, _enemySpawnPeriod, WaveDirection.down));
            if (!(player.IsAlive() && player2.IsAlive()))
            {
                yield break;
            }
            StartCoroutine(PU_TripleFireWave(_waveStartPause + _waveEndPause + 2 * _waveDuration));
            yield return StartCoroutine(EnemyWave(_waveStartPause + _waveEndPause + 2 * _waveDuration, 0, _waveEndPause, _enemySpawnPeriod, _enemySpawnPeriodDeviation, WaveDirection.down));
            if (!(player.IsAlive() && player2.IsAlive()))
            {
                yield break;
            }
            StartCoroutine(PU_SpeedUpWave(_waveStartPause + 2 * _waveEndPause + 3 * _waveDuration));
            yield return StartCoroutine(EnemyWave(_waveStartPause + 2 * _waveEndPause + 3 * _waveDuration, 0, _waveEndPause, _enemySpawnPeriod, _enemySpawnPeriodDeviation, WaveDirection.down));
            if (!(player.IsAlive() && player2.IsAlive()))
            {
                yield break;
            }
            StartCoroutine(PU_ShieldWave(_waveStartPause + 3 * _waveEndPause + 4 * _waveDuration));
            yield return StartCoroutine(EnemyWave(_waveStartPause + 3 * _waveEndPause + 4 * _waveDuration, 0, _waveEndPause, _enemySpawnPeriod, _enemySpawnPeriodDeviation, WaveDirection.down));
            if (!(player.IsAlive() && player2.IsAlive()))
            {
                yield break;
            }
        }
    }
}
