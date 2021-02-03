using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Managers
    private Spawn_Manager _spawnManager;    
    private UI_Manager _UI_Manager;
    //Associated objects
    private GameObject _shield, _thruster, _damage1, _damage2, _damage3;
    //Associated audio
    private AudioSource audio_source;
    [SerializeField]
    private AudioClip audio_fire, audio_powerup, audio_damage;
    //Prefabs
    [SerializeField]
    public GameObject _laser;
    [SerializeField]
    private GameObject _triple_laser;

    //Movement
    public float t_entrance = 5.0f; //time of reaching the starting position
    private float v_hor = 6f;
    private float v_up = 8f;
    private float v_down = 4.5f;
    private float v_speedup = 1.5f; //times of the normal speed
    private float speedup_time = 5f; //active time of speedup PU
    private enum Direction {Up, Down, Right, Left};
    Direction forward = Direction.Up; //Forward direction
    //Fire
    private float t_laser_cooldown = 0.1f;
    private float triple_laser_time = 5f;  //active time of triple laser PU
    //Damage
    private int Enemy_col_damage = 1;
    private int Obj_col_damage = 1;
    //Player stat
    private int Player_health = 4;
    private int Player_kills = 0;
    //Triggers
    private bool thrust_on = false;
    private bool thrust_current = false;
    private bool rotation_fix = true;
    private bool Fire_enabled = true;
    private bool PU_triple = false;
    private bool PU_speed = false;
    //Collision counters for PU and fire cooldown
    private int Fire_cooldown_count = 0;
    private int PU_Triple_count = 0;
    private int PU_speed_count = 0;

    void Start()
    {
        _spawnManager = GameObject.FindGameObjectWithTag("Spawn_Manager").GetComponent<Spawn_Manager>();
        _UI_Manager = _spawnManager._UI_manager;
        audio_source = transform.GetComponent<AudioSource>();
        //Identifying child objects
        for (int i=0; i < transform.childCount; i++)
        {
            switch (transform.GetChild(i).name)
            {
                case "Shield":
                    _shield = transform.GetChild(i).gameObject;
                    break;
                case "Fire_1":
                    _damage1 = transform.GetChild(i).gameObject;
                    break;
                case "Fire_2":
                    _damage2 = transform.GetChild(i).gameObject;
                    break;
                case "Fire_3":
                    _damage3 = transform.GetChild(i).gameObject;
                    break;
                case "Thruster":
                    _thruster = transform.GetChild(i).gameObject;
                    break;
                default:
                    Debug.LogWarning("There is an unrecognized child of Player.");
                    break;
            }
        }
        //Objects check
        if (_spawnManager == null)
        {
            Debug.LogError("Player could not locate Spawn Manager.");
        }        
        if (_UI_Manager == null) {
            Debug.LogWarning("Player could not obtain link to UI Canvas.");
        }
        if (audio_source == null)
        {
            Debug.LogWarning("Player could not locate its audio source.");
        }
        if (audio_fire == null)
        {
            Debug.LogWarning("Player could not locate fire audio.");
        }
        if (audio_powerup == null)
        {
            Debug.LogWarning("Player could not locate powerup audio.");
        }
        if (audio_damage == null)
        {
            Debug.LogWarning("Player could not locate damage audio.");
        }
        if (_laser == null)
        {
            Debug.LogError("Player could not locate PREFAB for Laser.");
        }
        if (_triple_laser == null)
        {
            Debug.LogError("Player could not locate PREFAB for Triple Laser.");
        }        
        if (_shield == null)
        {
            Debug.LogError("Player could not locate Shield.");
        }
        if (_thruster == null)
        {
            Debug.LogError("Player could not locate Fire3.");
        }
        if (_damage1 == null)
        {
            Debug.LogError("Player could not locate Fire1.");
        }
        if (_damage2 == null)
        {
            Debug.LogError("Player could not locate Fire2.");
        }
        if (_damage3 == null)
        {
            Debug.LogError("Player could not locate Fire3.");
        }
        //Parameters check
        if (v_hor <= 0)
        {
            Debug.LogWarning("Player horizontal speed is equal to or less than 0.");
        }
        if (v_up <= 0)
        {
            Debug.LogWarning("Player forward speed is equal to or less than 0.");
        }
        if (v_down <= 0)
        {
            Debug.LogWarning("Player backward speed is equal to or less than 0.");
        }        
        if (Player_health <= 0)
        {
            Debug.LogError("Player health is set below 1.");
        }
        //UI initial set
        _UI_Manager.Score_update(Player_kills);
        _UI_Manager.Lives_update(Player_health);
        //Player initial set
        _shield.gameObject.SetActive(false);
        _damage1.gameObject.SetActive(false);
        _damage2.gameObject.SetActive(false);
        _damage3.gameObject.SetActive(false);
        transform.position = new Vector3(0, -6f, 0);
        transform.rotation = Quaternion.Euler(0,0,0);
        StartCoroutine(Entrance());
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad <= t_entrance)
        {
            //Player entrance
            transform.Translate(Vector3.up * 5f / t_entrance * Time.deltaTime, Space.World);
        }
        else
        {            
            Movement();

            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && Fire_enabled)
            {
                Fire();
            }
        }
    }

    //Starting pause before activating UI
    IEnumerator Entrance()
    {
        yield return new WaitForSeconds(t_entrance);
        _UI_Manager.Trigger_UI();
    }

    void Movement()
    {
        float input_h = Input.GetAxis("Horizontal");
        float input_v = Input.GetAxis("Vertical");

        if (PU_speed)
        {
            input_h *= v_speedup;
            input_v *= v_speedup;
        }

        switch (forward)
        {
            case Direction.Up:
                if (input_v > 0)
                {
                    transform.Translate((Vector3.up * v_up * input_v + Vector3.right * v_hor * input_h) * Time.deltaTime, Space.World);
                    thrust_current = true;
                }
                else if (input_v <= 0)
                {
                    transform.Translate((Vector3.up * v_down * input_v + Vector3.right * v_hor * input_h) * Time.deltaTime, Space.World);
                    thrust_current = false;
                }
                break;
            case Direction.Down:
                if (input_v >= 0)
                {
                    transform.Translate((Vector3.up * v_down * input_v + Vector3.right * v_hor * input_h) * Time.deltaTime, Space.World);
                    thrust_current = false;
                }
                else if (input_v < 0)
                {
                    transform.Translate((Vector3.up * v_up * input_v + Vector3.right * v_hor * input_h) * Time.deltaTime, Space.World);
                    thrust_current = true;
                }
                break;
            case Direction.Right:
                if (input_h > 0)
                {
                    transform.Translate((Vector3.up * v_hor * input_v + Vector3.right * v_up * input_h) * Time.deltaTime, Space.World);
                    thrust_current = true;
                }
                else if (input_h <= 0)
                {
                    transform.Translate((Vector3.up * v_hor * input_v + Vector3.right * v_down * input_h) * Time.deltaTime, Space.World);
                    thrust_current = false;
                }
                break;
            case Direction.Left:
                if (input_h >= 0)
                {
                    transform.Translate((Vector3.up * v_hor * input_v + Vector3.right * v_down * input_h) * Time.deltaTime, Space.World);
                    thrust_current = false;
                }
                else if (input_h < 0)
                {
                    transform.Translate((Vector3.up * v_hor * input_v + Vector3.right * v_up * input_h) * Time.deltaTime, Space.World);
                    thrust_current = true;
                }
                break;
        }
        //Position limits
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.15f, 9.15f), Mathf.Clamp(transform.position.y, -3.50f, 5.67f), 0);
        //Thruster scale change
        if (thrust_on != thrust_current)
        {
            if (thrust_current)
            {
                _thruster.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            }
            else
            {
                _thruster.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            thrust_on = thrust_current;            
        }

        if (!rotation_fix)
        {
            Vector3 M = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            transform.rotation *= Quaternion.FromToRotation(transform.up, (M - transform.position));
            if (_damage1.activeSelf)
            {
                _damage1.transform.rotation *= Quaternion.FromToRotation(_damage1.transform.up, Vector3.up);
            }
            if (_damage2.activeSelf)
            {
                _damage2.transform.rotation *= Quaternion.FromToRotation(_damage2.transform.up, Vector3.up);
            }
            if (_damage3.activeSelf)
            {
                _damage3.transform.rotation *= Quaternion.FromToRotation(_damage3.transform.up, Vector3.up);
            }
        }
    }

    void Fire()
    {
        if (PU_triple == true)
        {
            Instantiate(_triple_laser, transform.position, transform.rotation, _spawnManager._laser_cont.transform);
            audio_source.PlayOneShot(audio_fire);
            audio_source.PlayOneShot(audio_fire);
            audio_source.PlayOneShot(audio_fire);
        }
        else
        {
            Instantiate(_laser, transform.position + transform.up * 0.8f, transform.rotation, _spawnManager._laser_cont.transform);
            audio_source.PlayOneShot(audio_fire);
        }

        StartCoroutine(Fire_cooldown());
    }
    
    IEnumerator Fire_cooldown()
    {
        Fire_cooldown_count++;
        Fire_enabled = false;
        yield return new WaitForSeconds(t_laser_cooldown);
        Fire_cooldown_count--;
        if (Fire_cooldown_count == 0)
        {
            Fire_enabled = true;
        }
    }

    //Activation of triple shot PU
    IEnumerator PU_triple_act()
    {
        PU_Triple_count++;
        PU_triple = true;
        yield return new WaitForSeconds(triple_laser_time);
        PU_Triple_count--;
        if (PU_Triple_count == 0) {
            PU_triple = false;
        }
    }

    //Activation of speed boost PU
    IEnumerator PU_speed_act()
    {
        PU_speed_count++;
        PU_speed = true;
        yield return new WaitForSeconds(speedup_time);
        PU_speed_count--;
        if (PU_speed_count == 0)
        {
            PU_speed = false;
        }
    }

    // PU collisions
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "PU_Triple":
                StartCoroutine(PU_triple_act());
                audio_source.PlayOneShot(audio_powerup);
                Destroy(other.gameObject);
                break;
            case "PU_player_speed":        
                StartCoroutine(PU_speed_act());
                audio_source.PlayOneShot(audio_powerup);
                Destroy(other.gameObject);
                break;
            case "PU_shield":
                _shield.gameObject.SetActive(true);
                audio_source.PlayOneShot(audio_powerup);
                Destroy(other.gameObject);
                break;
            case "Fire_enemy":
                Destroy(other.gameObject);
                Object_collide();
                break;
        }
    }

    //Enemy collision
    public void Enemy_collide()
    {
        Kill_count(1);
        if (_shield.gameObject.activeSelf)
        {
          //Shield works
          _shield.gameObject.SetActive(false);
        }
        else
        {
          Take_Damage(Enemy_col_damage);
          if (Player_health <= 0)
          {
             _UI_Manager.GAME_OVER();
             transform.gameObject.SetActive(false);
             return;
          }
          StartCoroutine(Fire_cooldown());
        }
    }

    //Object Collision
    public void Object_collide()
    {
        if (_shield.gameObject.activeSelf)
        {
            //Shield works
            _shield.gameObject.SetActive(false);
        }
        else
        {
            Take_Damage(Obj_col_damage);
            if (Player_health <= 0)
            {
                _UI_Manager.GAME_OVER();
                transform.gameObject.SetActive(false);
                return;
            }
            StartCoroutine(Fire_cooldown());
        }
    }

    //Adding to kill count
    public void Kill_count(int add)
    {
        Player_kills += add;
        _UI_Manager.Score_update(Player_kills);
    }

    //Player taking damage
    public void Take_Damage(int Damage)
    {
        audio_source.PlayOneShot(audio_damage);
        if (Damage > Player_health)
        {
            Player_health = 0;
        }
        else {
            Player_health -= Damage;
            switch (Player_health)
            {
                case 3:
                    _damage1.transform.rotation = transform.rotation;
                    _damage1.gameObject.SetActive(true);
                    break;
                case 2:
                    _damage2.transform.rotation = transform.rotation;
                    _damage2.gameObject.SetActive(true);
                    break;
                case 1:
                    _damage3.transform.rotation = transform.rotation;
                    _damage3.gameObject.SetActive(true);
                    break;
            }
        }
        
        _UI_Manager.Lives_update(Player_health);
    }

    //Returns Player's lives
    public int Lives()
    {
        return Player_health;
    }

    //Check if player's alive
    public bool Alive()
    {
        if (Player_health > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Rotation stop animation
    public void Stop_rot(Vector3 dir, float t_stop)
    {
        rotation_fix = true;
        StartCoroutine(Stop_rotation(dir, t_stop, t_stop + Time.timeSinceLevelLoad));
    }
    private IEnumerator Stop_rotation(Vector3 dir, float t, float t_end)
    {
        float w_stop; //angular velocity
        float Z = Quaternion.FromToRotation(transform.up, dir).eulerAngles.z; //angle of rotation
        if (Mathf.Abs(Z) <= 180) {
            w_stop = Z * 0.05f / t;
        }
        else
        {
            w_stop = (Z-360) * 0.05f / t;
        }
        while (Time.timeSinceLevelLoad < t_end)
        {
            transform.Rotate(0,0,w_stop);
            if (_damage1.activeSelf)
            {
                _damage1.transform.rotation *= Quaternion.FromToRotation(_damage1.transform.up, Vector3.up);
            }
            if (_damage2.activeSelf)
            {
                _damage2.transform.rotation *= Quaternion.FromToRotation(_damage2.transform.up, Vector3.up);
            }
            if (_damage3.activeSelf)
            {
                _damage3.transform.rotation *= Quaternion.FromToRotation(_damage3.transform.up, Vector3.up);
            }
            yield return new WaitForSeconds(0.05f);
        }
        transform.rotation *= Quaternion.FromToRotation(transform.up, dir);
        if (_damage1.activeSelf)
        {
            _damage1.transform.rotation *= Quaternion.FromToRotation(_damage1.transform.up, dir);
        }
        if (_damage2.activeSelf)
        {
            _damage2.transform.rotation *= Quaternion.FromToRotation(_damage2.transform.up, dir);
        }
        if (_damage3.activeSelf)
        {
            _damage3.transform.rotation *= Quaternion.FromToRotation(_damage3.transform.up, dir);
        }
        yield return null;
    }

    public void Start_Rot()
    {
        rotation_fix = false;
    }
}
