using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Manager : MonoBehaviour
{
    //Containers
    public GameObject _laser_cont, _enemy_cont, _PU_cont, _obj_cont;
    //Main objects  
    public UI_Manager _UI_manager;
    public AudioSource audio_background;
    public Player _player;
    //Prefabs
    [SerializeField]
    protected GameObject _enemy;
    [SerializeField]
    protected GameObject _asteroid;
    [SerializeField]
    protected GameObject _PU_triple;
    [SerializeField]
    protected GameObject _PU_player_speed;
    [SerializeField]
    protected GameObject _PU_shield;
    //Timeline
    private float t_start_0 = 5f;
    private float t_wave_pause_0 = 3f; 
    private float t_wave_0 = 30f;
    protected bool asteroid_trigger = false;
    //Time gaps
    private float dt_enemy_0 = 2f;
    private float dt_enemy_0_def = 0.3f;
    private float dt_PU_triple = 20f;
    private float dt_PU_triple_dev = 0.2f;
    private float dt_PU_player_speed = 20f;
    private float dt_PU_player_speed_dev = 0.3f;
    private float dt_PU_shield = 20f;
    private float dt_PU_shield_dev = 0.3f;
    //Object parameters
    private float scale_top_ast = 0.65f;
    private float scale_bot_ast = 0.3f;
    // Power Up switch container
    class PU_data
    {
        public bool triple, speed, shield;
        
        public PU_data(bool sh, bool sp, bool tr)
        {
            triple = tr;
            speed = sp;
            shield = sh;
        }
        public PU_data()
        {
            triple = speed = shield = false;
        }
    }
    //PU sets
    PU_data PU_0 = new PU_data();
    PU_data PU_1 = new PU_data(false, false, true);
    PU_data PU_2 = new PU_data(false, true, false);
    PU_data PU_3 = new PU_data(false, true, true);
    PU_data PU_4 = new PU_data(true, false, false);
    PU_data PU_5 = new PU_data(true, false, true);
    PU_data PU_6 = new PU_data(true, true, false);
    PU_data PU_7 = new PU_data(true,true,true);

    //Wave dircetion
    private enum Wave_dir { Down, Up, Right, Left };

    void Start()
    {
        _laser_cont = GameObject.Find("LaserContainer");
        _enemy_cont = GameObject.Find("EnemyContainer");
        _PU_cont = GameObject.Find("PUContainer");
        _obj_cont = GameObject.Find("ObjectContainer");
        _UI_manager = GameObject.Find("Canvas").GetComponent<UI_Manager>();       
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        audio_background = GameObject.FindGameObjectWithTag("Audio_Manager").GetComponent<AudioSource>();
        //Objects check
        if (_laser_cont == null)
        {
            Debug.LogError("Spawn Manager could not locate Laser Container.");
        }       
        if (_enemy_cont == null)
        {
            Debug.LogError("Spawn Manager could not locate Enemy Container.");
        }        
        if (_PU_cont == null)
        {
            Debug.LogError("Spawn Manager could not locate Power Up Container.");
        }       
        if (_obj_cont == null)
        {
            Debug.LogError("Spawn Manager could not locate Object Container.");
        }        
        if (_UI_manager == null) {
            Debug.LogError("Spawn Manager could not locate UI Canvas.");
        }       
        if (_player == null)
        {
            Debug.LogError("Spawn Manager could not locate Player.");
        }
        //Sound check
        if (audio_background == null)
        {
            Debug.LogError("Spawn Manager could not locate Audio Manager.");
        }
        //Prefab check
        if (_enemy == null)
        {
            Debug.LogError("Spawn Manager could not locate PREFAB for Enemy.");
        }
        if (_PU_triple == null)
        {
            Debug.LogError("Spawn Manager could not locate PREFAB for Triple Laser Power Up.");
        }
        if (_PU_player_speed == null)
        {
            Debug.LogError("Spawn Manager could not locate PREFAB for Player speed Power Up.");
        }
        if (_PU_shield == null)
        {
            Debug.LogError("Spawn Manager could not locate PREFAB for Shield Power Up.");
        }
        //Player entrance pause
        t_start_0 += _player.t_entrance;
        
        if (_player.Alive())
        {
            StartCoroutine(Asteroid_field(t_start_0+t_wave_0, t_start_0, t_start_0, dt_enemy_0, Wave_dir.Down, true));
        }
    }      

    //Asteroid spawn (trigger - asteroid's destruction launches Asteroid_destroyed)
    void Asteroid_create(Wave_dir dir, bool trigger)
    {
        float scale = Random.Range(scale_bot_ast, scale_top_ast);
        switch (dir)
        {
            case Wave_dir.Down:
                GameObject _new_asteroid = Instantiate(_asteroid, new Vector3(Random.Range(-8.25f, 8.25f), 7.8f), Quaternion.identity, _obj_cont.transform);
                _new_asteroid.transform.localScale = new Vector3(scale, scale, scale);
                _new_asteroid.GetComponent<Asteroid>().SetTrigger(trigger);
                break;
            case Wave_dir.Up:
                _new_asteroid = Instantiate(_asteroid, new Vector3(Random.Range(-8.25f, 8.25f), -6f), Quaternion.identity, _obj_cont.transform);
                _new_asteroid.transform.localScale = new Vector3(scale, scale, scale);
                _new_asteroid.GetComponent<Asteroid>().SetTrigger(trigger);
                break;
            case Wave_dir.Right:
                _new_asteroid = Instantiate(_asteroid, new Vector3(-11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, _obj_cont.transform);
                _new_asteroid.transform.localScale = new Vector3(scale, scale, scale);
                _new_asteroid.GetComponent<Asteroid>().SetTrigger(trigger);
                break;
            case Wave_dir.Left:
                _new_asteroid = Instantiate(_asteroid, new Vector3(11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, _obj_cont.transform);
                _new_asteroid.transform.localScale = new Vector3(scale, scale, scale);
                _new_asteroid.GetComponent<Asteroid>().SetTrigger(trigger);
                break;
        }
    }
    //With trigger = false
    void Asteroid_create(Wave_dir dir)
    {
        float scale = Random.Range(scale_bot_ast, scale_top_ast);
        switch (dir)
        {
            case Wave_dir.Down:
                GameObject _new_asteroid = Instantiate(_asteroid, new Vector3(Random.Range(-8.25f, 8.25f), 7.8f), Quaternion.identity, _obj_cont.transform);
                _new_asteroid.transform.localScale = new Vector3(scale, scale, scale);
                break;
            case Wave_dir.Up:
                _new_asteroid = Instantiate(_asteroid, new Vector3(Random.Range(-8.25f, 8.25f), -6f), Quaternion.identity, _obj_cont.transform);
                _new_asteroid.transform.localScale = new Vector3(scale, scale, scale);
                break;
            case Wave_dir.Right:
                _new_asteroid = Instantiate(_asteroid, new Vector3(-11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, _obj_cont.transform);
                _new_asteroid.transform.localScale = new Vector3(scale, scale, scale);
                break;
            case Wave_dir.Left:
                _new_asteroid = Instantiate(_asteroid, new Vector3(11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, _obj_cont.transform);
                _new_asteroid.transform.localScale = new Vector3(scale, scale, scale);
                break;
        }
    }

    //Enemy spawn (returnable - enemy will come back)
    void Enemy_create(Wave_dir dir, bool returnable)
    {
        switch (dir)
        {
            case Wave_dir.Down:
                GameObject _new_enemy = Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), 7.8f), Quaternion.identity, _enemy_cont.transform);
                _new_enemy.GetComponent<Enemy>().SetReturn(returnable);
                break;
            case Wave_dir.Up:
                _new_enemy = Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), -6f), Quaternion.identity, _enemy_cont.transform);
                _new_enemy.GetComponent<Enemy>().SetReturn(returnable);
                break;
            case Wave_dir.Right:
                _new_enemy = Instantiate(_enemy, new Vector3(-11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, _enemy_cont.transform);
                _new_enemy.GetComponent<Enemy>().SetReturn(returnable);
                break;
            case Wave_dir.Left:
                _new_enemy = Instantiate(_enemy, new Vector3(11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, _enemy_cont.transform);
                _new_enemy.GetComponent<Enemy>().SetReturn(returnable);
                break;
        }
    }
    //With returnable = false
    void Enemy_create(Wave_dir dir)
    {
        switch (dir)
        {
            case Wave_dir.Down:
                GameObject _new_enemy = Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), 7.8f), Quaternion.identity, _enemy_cont.transform);
                break;
            case Wave_dir.Up:
                _new_enemy = Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), -6f), Quaternion.identity, _enemy_cont.transform);
                break;
            case Wave_dir.Right:
                _new_enemy = Instantiate(_enemy, new Vector3(-11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, _enemy_cont.transform);
                break;
            case Wave_dir.Left:
                _new_enemy = Instantiate(_enemy, new Vector3(11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, _enemy_cont.transform);
                break;
        }
    }

    //Asteroid wave (end time, pauses without spawn, interval of spawn, its deviation, direction, trigger option)
    private IEnumerator Asteroid_field(float t_wave_end, float t_start_pause, float t_end_pause, float dt_ast, float dt_ast_dev, Wave_dir dir, bool trigger)
    {
        yield return new WaitForSeconds(t_start_pause);
        if (trigger)
        {
            while (Time.timeSinceLevelLoad <= t_wave_end && !asteroid_trigger)
            {
                Asteroid_create(dir, trigger);

                float dt = 0;
                if (dt_ast_dev != 0)
                {
                    dt = Random.Range((1f - dt_ast_dev) * dt_ast, (1f + dt_ast_dev) * dt_ast);
                }
                if (Time.timeSinceLevelLoad + dt > t_wave_end)
                {
                    yield return new WaitForSeconds(t_wave_end - Time.timeSinceLevelLoad);
                }
                else
                {
                    yield return new WaitForSeconds(dt);
                }
            }
        }
        else
        {
            while (Time.timeSinceLevelLoad <= t_wave_end)
            {
                Asteroid_create(dir);

                float dt = 0;
                if (dt_ast_dev != 0)
                {
                    dt = Random.Range((1f - dt_ast_dev) * dt_ast, (1f + dt_ast_dev) * dt_ast);
                }
                if (Time.timeSinceLevelLoad + dt > t_wave_end)
                {
                    yield return new WaitForSeconds(t_wave_end - Time.timeSinceLevelLoad);
                }
                else
                {
                    yield return new WaitForSeconds(dt);
                }
            }
        }
        yield return new WaitForSeconds(t_end_pause);
    }
    //Set dt
    private IEnumerator Asteroid_field(float t_wave_end, float t_start_pause, float t_end_pause, float dt_ast, Wave_dir dir, bool trigger)
    {
        yield return new WaitForSeconds(t_start_pause);
        if (trigger)
        {
            while (Time.timeSinceLevelLoad <= t_wave_end && !asteroid_trigger)
            {
                Asteroid_create(dir, trigger);

                if (Time.timeSinceLevelLoad + dt_ast > t_wave_end)
                {
                    yield return new WaitForSeconds(t_wave_end - Time.timeSinceLevelLoad);
                }
                else
                {
                    yield return new WaitForSeconds(dt_ast);
                }
            }
        }
        else
        {
            while (Time.timeSinceLevelLoad <= t_wave_end)
            {
                Asteroid_create(dir);

                if (Time.timeSinceLevelLoad + dt_ast > t_wave_end)
                {
                    yield return new WaitForSeconds(t_wave_end - Time.timeSinceLevelLoad);
                }
                else
                {
                    yield return new WaitForSeconds(dt_ast);
                }
            }
        }
        yield return new WaitForSeconds(t_end_pause);
    }
    //Set dt & No end pause
    private IEnumerator Asteroid_field(float t_wave_end, float t_start_pause, float dt_ast, Wave_dir dir, bool trigger)
    {
        yield return new WaitForSeconds(t_start_pause);
        if (trigger)
        {
            while (Time.timeSinceLevelLoad <= t_wave_end && !asteroid_trigger)
            {
                Asteroid_create(dir, trigger);

                if (Time.timeSinceLevelLoad + dt_ast > t_wave_end)
                {
                    yield return new WaitForSeconds(t_wave_end - Time.timeSinceLevelLoad);
                }
                else
                {
                    yield return new WaitForSeconds(dt_ast);
                }
            }
        }
        else
        {
            while (Time.timeSinceLevelLoad <= t_wave_end)
            {
                Asteroid_create(dir);

                if (Time.timeSinceLevelLoad + dt_ast > t_wave_end)
                {
                    yield return new WaitForSeconds(t_wave_end - Time.timeSinceLevelLoad);
                }
                else
                {
                    yield return new WaitForSeconds(dt_ast);
                }
            }
        }
    }
    //No trigger
    private IEnumerator Asteroid_field(float t_wave_end, float t_start_pause, float t_end_pause, float dt_ast, float dt_ast_dev, Wave_dir dir)
    {
        yield return new WaitForSeconds(t_start_pause);
        while (Time.timeSinceLevelLoad <= t_wave_end)
        {
            Asteroid_create(dir);

            float dt = 0;
            if (dt_ast_dev != 0)
            {
                dt = Random.Range((1f - dt_ast_dev) * dt_ast, (1f + dt_ast_dev) * dt_ast);
            }
            if (Time.timeSinceLevelLoad + dt > t_wave_end)
            {
                yield return new WaitForSeconds(t_wave_end - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(dt);
            }
        }
        yield return new WaitForSeconds(t_end_pause);
    }
    //No trigger & Set dt
    private IEnumerator Asteroid_field(float t_wave_end, float t_start_pause, float t_end_pause, float dt_ast, Wave_dir dir)
    {
        yield return new WaitForSeconds(t_start_pause);
        while (Time.timeSinceLevelLoad <= t_wave_end)
        {
            Asteroid_create(dir);

            if (Time.timeSinceLevelLoad + dt_ast > t_wave_end)
            {
                yield return new WaitForSeconds(t_wave_end - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(dt_ast);
            }
        }
        yield return new WaitForSeconds(t_end_pause);
    }
    //No trigger & Set dt & No end pause
    private IEnumerator Asteroid_field(float t_wave_end, float t_start_pause, float dt_ast, Wave_dir dir)
    {
        yield return new WaitForSeconds(t_start_pause);
        while (Time.timeSinceLevelLoad <= t_wave_end)
        {
            Asteroid_create(dir);

            if (Time.timeSinceLevelLoad + dt_ast > t_wave_end)
            {
                yield return new WaitForSeconds(t_wave_end - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(dt_ast);
            }
        }
    }

    //Enemy wave randomly in the whole position range (end time, pauses without spawn, interval of spawn, its deviation, direction, set of PU)
    private IEnumerator Pos_range(float t_wave_end, float t_start_pause, float t_end_pause, float dt_enemy, float dt_enemy_dev, Wave_dir dir, PU_data PU)
    {
        yield return new WaitForSeconds(t_start_pause);
        if (PU.triple)
        {
            StartCoroutine(PU_triple(t_wave_end));
        }
        if (PU.speed)
        {
            StartCoroutine(PU_player_speed(t_wave_end));
        }
        if (PU.shield)
        {
            StartCoroutine(PU_shield(t_wave_end));
        }

        while (Time.timeSinceLevelLoad <= t_wave_end)
        {
            Enemy_create(dir);

            float dt = 0;
            if (dt_enemy_dev != 0)
            {
                dt = Random.Range((1f - dt_enemy_dev) * dt_enemy, (1f + dt_enemy_dev) * dt_enemy);
            }
            if (Time.timeSinceLevelLoad + dt > t_wave_end)
            {
                yield return new WaitForSeconds(t_wave_end - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(dt);
            }
        }
        yield return new WaitForSeconds(t_end_pause);
    }
    //Without Power Ups
    private IEnumerator Pos_range(float t_wave_end, float t_start_pause, float t_end_pause, float dt_enemy, float dt_enemy_dev, Wave_dir dir)
    {
        yield return new WaitForSeconds(t_start_pause);

        while (Time.timeSinceLevelLoad <= t_wave_end)
        {
            Enemy_create(dir);

            float dt = 0;
            if (dt_enemy_dev != 0)
            {
                dt = Random.Range((1f - dt_enemy_dev) * dt_enemy, (1f + dt_enemy_dev) * dt_enemy);
            }
            if (Time.timeSinceLevelLoad + dt > t_wave_end)
            {
                yield return new WaitForSeconds(t_wave_end - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(dt);
            }
        }
        yield return new WaitForSeconds(t_end_pause);
    }
    //Set dt_enemy
    private IEnumerator Pos_range(float t_wave_end, float t_start_pause, float t_end_pause, float dt_enemy, Wave_dir dir, PU_data PU)
    {
        yield return new WaitForSeconds(t_start_pause);
        if (PU.triple)
        {
            StartCoroutine(PU_triple(t_wave_end));
        }
        if (PU.speed)
        {
            StartCoroutine(PU_player_speed(t_wave_end));
        }
        if (PU.shield)
        {
            StartCoroutine(PU_shield(t_wave_end));
        }

        while (Time.timeSinceLevelLoad <= t_wave_end)
        {
            Enemy_create(dir);

            if (Time.timeSinceLevelLoad + dt_enemy > t_wave_end)
            {
                yield return new WaitForSeconds(t_wave_end - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(dt_enemy);
            }
        }
        yield return new WaitForSeconds(t_end_pause);
    }
    //Set dt_enemy & No Power Ups
    private IEnumerator Pos_range(float t_wave_end, float t_start_pause, float t_end_pause, float dt_enemy, Wave_dir dir)
    {
        yield return new WaitForSeconds(t_start_pause);

        while (Time.timeSinceLevelLoad <= t_wave_end)
        {
            Enemy_create(dir);

            if (Time.timeSinceLevelLoad + dt_enemy > t_wave_end)
            {
                yield return new WaitForSeconds(t_wave_end - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(dt_enemy);
            }
        }
        yield return new WaitForSeconds(t_end_pause);
    }
    //Set dt_enemy & No end pause
    private IEnumerator Pos_range(float t_wave_end, float t_start_pause, float dt_enemy, Wave_dir dir, PU_data PU)
    {
        yield return new WaitForSeconds(t_start_pause);
        if (PU.triple)
        {
            StartCoroutine(PU_triple(t_wave_end));
        }
        if (PU.speed)
        {
            StartCoroutine(PU_player_speed(t_wave_end));
        }
        if (PU.shield)
        {
            StartCoroutine(PU_shield(t_wave_end));
        }

        while (Time.timeSinceLevelLoad <= t_wave_end)
        {
            Enemy_create(dir);

            if (Time.timeSinceLevelLoad + dt_enemy > t_wave_end)
            {
                yield return new WaitForSeconds(t_wave_end - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(dt_enemy);
            }
        }
    }
    //Set dt_enemy & No end pause & No Power Ups
    private IEnumerator Pos_range(float t_wave_end, float t_start_pause, float dt_enemy, Wave_dir dir)
    {
        yield return new WaitForSeconds(t_start_pause);

        while (Time.timeSinceLevelLoad <= t_wave_end)
        {
            Enemy_create(dir);

            if (Time.timeSinceLevelLoad + dt_enemy > t_wave_end)
            {
                yield return new WaitForSeconds(t_wave_end - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(dt_enemy);
            }
        }
    }

    //Power Ups spawn methods
    IEnumerator PU_triple(float t_wave_end)
    {
        while (Time.timeSinceLevelLoad < t_wave_end)
        {
            Instantiate(_PU_triple, new Vector3(Random.Range(-9.15f, 9.15f), 10f, 0), Quaternion.identity, _PU_cont.transform);
            yield return new WaitForSeconds(Random.Range((1 - dt_PU_triple_dev) * dt_PU_triple, (1 + dt_PU_triple_dev) * dt_PU_triple));
        }
    }
    IEnumerator PU_player_speed(float t_wave_end)
    {
        while (Time.timeSinceLevelLoad < t_wave_end)
        {
            Instantiate(_PU_player_speed, new Vector3(Random.Range(-9.15f, 9.15f), 10f, 0), Quaternion.identity, _PU_cont.transform);
            yield return new WaitForSeconds(Random.Range((1 - dt_PU_player_speed_dev) * dt_PU_player_speed, (1 + dt_PU_player_speed_dev) * dt_PU_player_speed));
        }
    }
    IEnumerator PU_shield(float t_wave_end)
    {
        while (Time.timeSinceLevelLoad < t_wave_end)
        {
            Instantiate(_PU_shield, new Vector3(Random.Range(-9.15f, 9.15f), 10f, 0), Quaternion.identity, _PU_cont.transform);
            yield return new WaitForSeconds(Random.Range((1 - dt_PU_shield_dev) * dt_PU_shield, (1 + dt_PU_shield_dev) * dt_PU_shield));
        }
    }
    
    //Asteroid destruction as a start trigger
    public void Asteroid_destroyed()
    {
        if (!asteroid_trigger)
        {
            asteroid_trigger = true;
            StartCoroutine(Main_Timeline());
        }
    }    

    //Main wave sequaence
    IEnumerator Main_Timeline()
    {
        _player.Stop_rot(Vector3.up, t_start_0/4);
        audio_background.PlayDelayed(t_start_0/4);
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