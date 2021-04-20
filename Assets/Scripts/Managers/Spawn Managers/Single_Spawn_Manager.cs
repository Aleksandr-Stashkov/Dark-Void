using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_Spawn_Manager : Spawn_Manager
{
    protected override void Start()
    {
        base.Start();

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Single_Player>();
        if (_player == null)
        {
            Debug.LogError("Spawn Manager could not locate Player.");
        }

        //Player entrance pause
        t_start_0 += t_player_entrance;
        _player.Set_t_entrance(t_player_entrance);        

        if (_player.Alive())
        {
            StartCoroutine(Asteroid_field(t_start_0 + t_wave_0, t_start_0, t_start_0, dt_enemy_0, Wave_dir.Down, true));
        }
    }

    //Main wave sequaence
    protected override IEnumerator Main_Timeline()
    {
        _player.Stop_rot(Vector3.up, t_start_0 / 4);
        audio_background.PlayDelayed(t_start_0 / 4);
        while (_player.Alive())
        {
            yield return StartCoroutine(Pos_range(t_start_0 + t_wave_0, t_start_0, t_wave_pause_0, dt_enemy_0, Wave_dir.Down, PU_0));
            if (!(_player.Alive()))
            {
                yield break;
            }
            yield return StartCoroutine(Pos_range(t_start_0 + t_wave_pause_0 + 2 * t_wave_0, 0, t_wave_pause_0, dt_enemy_0, dt_enemy_0_def, Wave_dir.Down, PU_1));
            if (!(_player.Alive()))
            {
                yield break;
            }
            yield return StartCoroutine(Pos_range(t_start_0 + 2 * t_wave_pause_0 + 3 * t_wave_0, 0, t_wave_pause_0, dt_enemy_0, dt_enemy_0_def, Wave_dir.Down, PU_2));
            if (!(_player.Alive()))
            {
                yield break;
            }
            yield return StartCoroutine(Pos_range(t_start_0 + 3 * t_wave_pause_0 + 4 * t_wave_0, 0, t_wave_pause_0, dt_enemy_0, dt_enemy_0_def, Wave_dir.Down, PU_4));
            if (!(_player.Alive()))
            {
                yield break;
            }
        }
    }
}
