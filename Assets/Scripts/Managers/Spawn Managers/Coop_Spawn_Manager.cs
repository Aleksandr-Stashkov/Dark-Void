using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coop_Spawn_Manager : Spawn_Manager
{
    [HideInInspector]
    public Player _player_2;

    protected override void Start()
    {
        base.Start();

        Find_Players();
        if (_player == null)
        {
            Debug.LogError("Spawn Manager could not locate Player 1.");
        }
        if (_player_2 == null)
        {
            Debug.LogError("Spawn Manager could not locate Player 2.");
        }

        //Players entrance pause
        _player.Set_t_entrance(t_player_entrance);
        _player_2.Set_t_entrance(t_player_entrance);

        if (_player.Alive() && _player_2.Alive())
        {
            StartCoroutine(Asteroid_field(t_start_0 + t_wave_0, t_start_0, t_start_0, dt_enemy_0, Wave_dir.Down, true));
        }
    }

    //Identifying players
    private void Find_Players()
    {
        GameObject[] _players;
        _players = GameObject.FindGameObjectsWithTag("Player");
        if(_players.Length == 2)
        {
            _player = _players[0].GetComponent<Player>();
            if (_player.Is_Player1())
            {
                _player_2 = _players[1].GetComponent<Player>();
            }
            else
            {
                _player_2 = _player;
                _player = _players[1].GetComponent<Player>();
            }
        }
        else
        {
            Debug.LogWarning("The number of players is different from two.");
        }
    }

    //Main wave sequaence
    protected override IEnumerator Main_Timeline()
    {       
        audio_background.PlayDelayed(t_start_0 / 4);
        while (_player.Alive() && _player_2.Alive())
        {
            yield return StartCoroutine(Pos_range(t_start_0 + t_wave_0, t_start_0, t_wave_pause_0, dt_enemy_0, Wave_dir.Down, PU_0));
            if (!(_player.Alive() && _player_2.Alive()))
            {
                yield break;
            }
            yield return StartCoroutine(Pos_range(t_start_0 + t_wave_pause_0 + 2 * t_wave_0, 0, t_wave_pause_0, dt_enemy_0, dt_enemy_0_def, Wave_dir.Down, PU_1));
            if (!(_player.Alive() && _player_2.Alive()))
            {
                yield break;
            }
            yield return StartCoroutine(Pos_range(t_start_0 + 2 * t_wave_pause_0 + 3 * t_wave_0, 0, t_wave_pause_0, dt_enemy_0, dt_enemy_0_def, Wave_dir.Down, PU_2));
            if (!(_player.Alive() && _player_2.Alive()))
            {
                yield break;
            }
            yield return StartCoroutine(Pos_range(t_start_0 + 3 * t_wave_pause_0 + 4 * t_wave_0, 0, t_wave_pause_0, dt_enemy_0, dt_enemy_0_def, Wave_dir.Down, PU_4));
            if (!(_player.Alive() && _player_2.Alive()))
            {
                yield break;
            }
        }
    }
}
