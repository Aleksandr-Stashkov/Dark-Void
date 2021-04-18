using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Managers
    protected Spawn_Manager _spawnManager;
    protected UI_Manager _UI_Manager;
    //Associated objects
    protected GameObject _shield, _thruster, _damage1, _damage2, _damage3;
    protected Animator _anim;
    protected int anim_Turn_Left_id, anim_Turn_Right_id; //ids for Animator variables
    //Associated audio
    protected AudioSource audio_source;
    [SerializeField]
    protected AudioClip audio_fire, audio_powerup, audio_damage;
    //Prefabs
    [SerializeField]
    protected GameObject _laser;
    [SerializeField]
    protected GameObject _triple_laser;

    //Movement
    protected bool User_Control = false;
    protected bool rotation_fix = true;
    [HideInInspector]
    public float t_entrance = 0f; //time for reaching the starting position
    protected float v_side = 6f;
    protected float v_for = 8f;
    protected float v_back = 4.5f;
    protected float v_speedup = 1.5f; //times of the normal speed
    protected float speedup_time = 5f; //active time of speedup PU
    protected enum Direction {Up, Down, Right, Left};
    Direction forward = Direction.Up; //Forward direction
    //Fire
    protected bool Fire_enabled = true;
    protected float t_laser_cooldown = 0.1f;
    protected float triple_laser_time = 5f;  //active time of triple laser PU
    //Damage
    protected int Enemy_col_damage = 1;
    protected int Obj_col_damage = 1;
    //Player stat
    protected int Player_health = 4;
    protected int Player_kills = 0;
    //Triggers    
    protected bool thrust_current = false; //current thrust state
    protected bool thrust_prev = false; //stores previous thrust state for comparing (for prevention of excess changes in transform)   
    protected bool turn_left_prev = false; //previous turn animation triggers (for prevention of excess changes in Animator)
    protected bool turn_right_prev = false;
    protected bool PU_triple = false;
    protected bool PU_speed = false;
    //Collision counters for PU and fire cooldown
    protected int Fire_cooldown_count = 0;
    protected int PU_Triple_count = 0;
    protected int PU_speed_count = 0;

    protected virtual void Start()
    {
        _spawnManager = GameObject.FindGameObjectWithTag("Spawn_Manager").GetComponent<Spawn_Manager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Player could not locate Spawn Manager.");
        }
        _UI_Manager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Player could not locate its animator.");
        }
        anim_Turn_Left_id = Animator.StringToHash("Turn_Left");
        anim_Turn_Right_id = Animator.StringToHash("Turn_Right");
        audio_source = GetComponent<AudioSource>();
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
        if (anim_Turn_Left_id == 0)
        {
            Debug.LogWarning("Player's animator could not find Turn_Left parameter.");
        }
        if (anim_Turn_Right_id == 0)
        {
            Debug.LogWarning("Player's animator could not find Turn_Right parameter.");
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
        if (v_side <= 0)
        {
            Debug.LogWarning("Player horizontal speed is equal to or less than 0.");
        }
        if (v_for <= 0)
        {
            Debug.LogWarning("Player forward speed is equal to or less than 0.");
        }
        if (v_back <= 0)
        {
            Debug.LogWarning("Player backward speed is equal to or less than 0.");
        }        
        if (Player_health <= 0)
        {
            Debug.LogError("Player health is set below 1.");
        }
        if (User_Control)
        {
            Debug.LogWarning("User will control the Player from the start.");
        }
       
        //Player initial set
        _shield.gameObject.SetActive(false);
        _damage1.gameObject.SetActive(false);
        _damage2.gameObject.SetActive(false);
        _damage3.gameObject.SetActive(false);
        //Animation triggers confirmation
        _anim.SetBool(anim_Turn_Left_id, turn_left_prev);
        _anim.SetBool(anim_Turn_Right_id, turn_right_prev);
        transform.rotation = Quaternion.Euler(0,0,0);
        StartCoroutine(Entrance_timer());
    }

    //Events related to entrance pause
    protected virtual IEnumerator Entrance_timer()
    {
        yield return new WaitForSeconds(t_entrance);
        User_Control = true;
    }

    protected virtual void Update()
    {
            if (Time.timeSinceLevelLoad <= t_entrance)
            {
                Entrance_Movement();
            }
    }



    protected virtual void Movement()
    {
        float input_h = GetHorizontal();
        float input_v = GetVertical();
        //speed modificator
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
                    transform.Translate((Vector3.up * v_for * input_v + Vector3.right * v_side * input_h) * Time.deltaTime, Space.World);
                    thrust_current = true;
                }
                else if (input_v <= 0)
                {
                    transform.Translate((Vector3.up * v_back * input_v + Vector3.right * v_side * input_h) * Time.deltaTime, Space.World);
                    thrust_current = false;
                }                
                Turn_Animation(input_h);
                break;
            case Direction.Down:
                if (input_v >= 0)
                {
                    transform.Translate((Vector3.up * v_back * input_v + Vector3.right * v_side * input_h) * Time.deltaTime, Space.World);
                    thrust_current = false;
                }
                else if (input_v < 0)
                {
                    transform.Translate((Vector3.up * v_for * input_v + Vector3.right * v_side * input_h) * Time.deltaTime, Space.World);
                    thrust_current = true;
                }
                Turn_Animation(input_h);
                break;
            case Direction.Right:
                if (input_h > 0)
                {
                    transform.Translate((Vector3.up * v_side * input_v + Vector3.right * v_for * input_h) * Time.deltaTime, Space.World);
                    thrust_current = true;
                }
                else if (input_h <= 0)
                {
                    transform.Translate((Vector3.up * v_side * input_v + Vector3.right * v_back * input_h) * Time.deltaTime, Space.World);
                    thrust_current = false;
                }
                Turn_Animation(input_v);
                break;
            case Direction.Left:
                if (input_h >= 0)
                {
                    transform.Translate((Vector3.up * v_side * input_v + Vector3.right * v_back * input_h) * Time.deltaTime, Space.World);
                    thrust_current = false;
                }
                else if (input_h < 0)
                {
                    transform.Translate((Vector3.up * v_side * input_v + Vector3.right * v_for * input_h) * Time.deltaTime, Space.World);
                    thrust_current = true;
                }
                Turn_Animation(input_v);
                break;
        }
        //Position limits
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.15f, 9.15f), Mathf.Clamp(transform.position.y, -3.50f, 5.67f), 0);        
        //Thruster scale change
        if (thrust_prev != thrust_current)
        {
            if (thrust_current)
            {
                _thruster.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            }
            else
            {
                _thruster.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            thrust_prev = thrust_current;            
        }       
    }

    protected virtual float GetHorizontal()
    {
        return 0f;
    }
    protected virtual float GetVertical()
    {
        return 0f;
    }

    //Managing Animator's parameters for turns
    protected void Turn_Animation(float right)
    {
        if (right == 0 && (turn_left_prev || turn_right_prev))
        {
            _anim.SetBool(anim_Turn_Right_id, false);
            _anim.SetBool(anim_Turn_Left_id, false);
            turn_left_prev = false;
            turn_right_prev = false;
        }
        else if (right > 0 && !turn_right_prev)
        {
            _anim.SetBool(anim_Turn_Right_id, true);
            turn_right_prev = true;
        }
        else if (right < 0 && !turn_left_prev)
        {
            _anim.SetBool(anim_Turn_Left_id, true);
            turn_left_prev = true;
        }
    }



    //Rotation towards mouse position
    protected void Mouse_Rotation()
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

    //Entrance Movement
    protected virtual void Entrance_Movement()
    {
        transform.Translate(Vector3.up * 5f / t_entrance * Time.deltaTime, Space.World);        
    }

    protected void Fire()
    {
        if (PU_triple == true)
        {
            GameObject _new_triple = Instantiate(_triple_laser, transform.position, transform.rotation, _spawnManager._laser_cont.transform);
            Laser[] lasers = _new_triple.GetComponentsInChildren<Laser>();
            foreach (Laser laser in lasers)
            {
                laser.SetPlayer(this);
            }
            audio_source.PlayOneShot(audio_fire);
            audio_source.PlayOneShot(audio_fire);
            audio_source.PlayOneShot(audio_fire);
        }
        else
        {
            GameObject _new_laser = Instantiate(_laser, transform.position + transform.up * 0.8f, transform.rotation, _spawnManager._laser_cont.transform);
            _new_laser.GetComponent<Laser>().SetPlayer(this);
            audio_source.PlayOneShot(audio_fire);
        }

        StartCoroutine(Fire_cooldown());
    }

    protected IEnumerator Fire_cooldown()
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
    protected IEnumerator PU_triple_act()
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
    protected IEnumerator PU_speed_act()
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
    protected void OnTriggerEnter2D(Collider2D other)
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
    public virtual void Kill_count(int add)
    {
        Player_kills += add;        
    }

    //Player taking damage
    public virtual void Take_Damage(int Damage)
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

    //Initial set of entrance time
    public void Set_t_entrance(float t)
    {
        t_entrance = t;
        if(t_entrance == 0)
        {
            User_Control = true;
        }
    }

    //Rotation stop animation
    public void Stop_rot(Vector3 dir, float t_stop)
    {
        rotation_fix = true;
        StartCoroutine(Stop_rotation(dir, t_stop, t_stop + Time.timeSinceLevelLoad));
    }
    protected IEnumerator Stop_rotation(Vector3 dir, float t, float t_end)
    {
        if (t == 0) {
            Debug.LogWarning("Coroutine Stop_rotation got 0 as the time of execution.");
            yield return null;
        }
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

    public virtual bool Is_Player1() { return true; }
}
