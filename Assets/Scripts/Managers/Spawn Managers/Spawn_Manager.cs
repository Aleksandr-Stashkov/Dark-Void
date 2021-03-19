using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Manager : MonoBehaviour
{
    //Containers
    [HideInInspector]
    public GameObject _laser_cont, _enemy_cont, _PU_cont, _obj_cont;
    //Main objects
    [HideInInspector]
    public AudioSource audio_background;
    [HideInInspector]
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
    protected float t_player_entrance = 5f; //time for player to reach its starting position
    protected float t_start_0 = 5f;
    protected float t_wave_pause_0 = 3f; 
    protected float t_wave_0 = 30f;
    protected bool asteroid_trigger = false;
    //Time gaps
    protected float dt_enemy_0 = 2f;
    protected float dt_enemy_0_def = 0.3f;
    protected float dt_PU_triple = 20f;
    protected float dt_PU_triple_dev = 0.2f;
    protected float dt_PU_player_speed = 20f;
    protected float dt_PU_player_speed_dev = 0.3f;
    protected float dt_PU_shield = 20f;
    protected float dt_PU_shield_dev = 0.3f;
    //Object parameters
    protected float scale_top_ast = 0.65f;
    protected float scale_bot_ast = 0.3f;
    // Power Up switch container
    protected class PU_data
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
    protected PU_data PU_0 = new PU_data();
    protected PU_data PU_1 = new PU_data(false, false, true);
    protected PU_data PU_2 = new PU_data(false, true, false);
    protected PU_data PU_3 = new PU_data(false, true, true);
    protected PU_data PU_4 = new PU_data(true, false, false);
    protected PU_data PU_5 = new PU_data(true, false, true);
    protected PU_data PU_6 = new PU_data(true, true, false);
    protected PU_data PU_7 = new PU_data(true,true,true);

    //Wave dircetion
    protected enum Wave_dir { Down, Up, Right, Left };

    protected virtual void Start()
    {
        //Identifying child objects
        for (int i = 0; i < transform.childCount; i++)
        {
            switch (transform.GetChild(i).name)
            {
                case "LaserContainer":
                    _laser_cont = transform.GetChild(i).gameObject;
                    break;
                case "EnemyContainer":
                    _enemy_cont = transform.GetChild(i).gameObject;
                    break;
                case "PUContainer":
                    _PU_cont = transform.GetChild(i).gameObject;
                    break;
                case "ObjectContainer":
                    _obj_cont = transform.GetChild(i).gameObject;
                    break;                
                default:
                    Debug.LogWarning("There is an unrecognized child of Spawn Manager.");
                    break;
            }
        }       
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

        t_start_0 += t_player_entrance;
    }      

    //Asteroid spawn (trigger - asteroid's destruction launches Asteroid_destroyed)
    protected void Asteroid_create(Wave_dir dir, bool trigger)
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
    protected void Asteroid_create(Wave_dir dir)
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
    protected void Enemy_create(Wave_dir dir, bool returnable)
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
    protected void Enemy_create(Wave_dir dir)
    {
        switch (dir)
        {
            case Wave_dir.Down:
                Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), 7.8f), Quaternion.identity, _enemy_cont.transform);
                break;
            case Wave_dir.Up:
                Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), -6f), Quaternion.identity, _enemy_cont.transform);
                break;
            case Wave_dir.Right:
                Instantiate(_enemy, new Vector3(-11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, _enemy_cont.transform);
                break;
            case Wave_dir.Left:
                Instantiate(_enemy, new Vector3(11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, _enemy_cont.transform);
                break;
        }
    }

    //Asteroid wave (end time, pauses without spawn, interval of spawn, its deviation, direction, trigger option)
    protected IEnumerator Asteroid_field(float t_wave_end, float t_start_pause, float t_end_pause, float dt_ast, float dt_ast_dev, Wave_dir dir, bool trigger)
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
    protected IEnumerator Asteroid_field(float t_wave_end, float t_start_pause, float t_end_pause, float dt_ast, Wave_dir dir, bool trigger)
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
    protected IEnumerator Asteroid_field(float t_wave_end, float t_start_pause, float dt_ast, Wave_dir dir, bool trigger)
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
    protected IEnumerator Asteroid_field(float t_wave_end, float t_start_pause, float t_end_pause, float dt_ast, float dt_ast_dev, Wave_dir dir)
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
    protected IEnumerator Asteroid_field(float t_wave_end, float t_start_pause, float t_end_pause, float dt_ast, Wave_dir dir)
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
    protected IEnumerator Asteroid_field(float t_wave_end, float t_start_pause, float dt_ast, Wave_dir dir)
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
    protected IEnumerator Pos_range(float t_wave_end, float t_start_pause, float t_end_pause, float dt_enemy, float dt_enemy_dev, Wave_dir dir, PU_data PU)
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
    protected IEnumerator Pos_range(float t_wave_end, float t_start_pause, float t_end_pause, float dt_enemy, float dt_enemy_dev, Wave_dir dir)
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
    protected IEnumerator Pos_range(float t_wave_end, float t_start_pause, float t_end_pause, float dt_enemy, Wave_dir dir, PU_data PU)
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
    protected IEnumerator Pos_range(float t_wave_end, float t_start_pause, float t_end_pause, float dt_enemy, Wave_dir dir)
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
    protected IEnumerator Pos_range(float t_wave_end, float t_start_pause, float dt_enemy, Wave_dir dir, PU_data PU)
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
    protected IEnumerator Pos_range(float t_wave_end, float t_start_pause, float dt_enemy, Wave_dir dir)
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
    protected IEnumerator PU_triple(float t_wave_end)
    {
        while (Time.timeSinceLevelLoad < t_wave_end)
        {
            Instantiate(_PU_triple, new Vector3(Random.Range(-9.15f, 9.15f), 10f, 0), Quaternion.identity, _PU_cont.transform);
            yield return new WaitForSeconds(Random.Range((1 - dt_PU_triple_dev) * dt_PU_triple, (1 + dt_PU_triple_dev) * dt_PU_triple));
        }
    }
    protected IEnumerator PU_player_speed(float t_wave_end)
    {
        while (Time.timeSinceLevelLoad < t_wave_end)
        {
            Instantiate(_PU_player_speed, new Vector3(Random.Range(-9.15f, 9.15f), 10f, 0), Quaternion.identity, _PU_cont.transform);
            yield return new WaitForSeconds(Random.Range((1 - dt_PU_player_speed_dev) * dt_PU_player_speed, (1 + dt_PU_player_speed_dev) * dt_PU_player_speed));
        }
    }
    protected IEnumerator PU_shield(float t_wave_end)
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
    protected virtual IEnumerator Main_Timeline()
    {
        yield break;
    }    
}