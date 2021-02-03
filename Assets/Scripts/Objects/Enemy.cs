using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Moving_Object
{
    private Player _player;
    [SerializeField]
    private GameObject _laser;
    //Components
    private Animator _Anim;
    private Collider2D _Collider;
    //Audio
    private AudioSource audio_destruction;
    //Parameters
    private float t_fire_avg = 3f;  //average time between shots
    private float t_cooldown = 0f;  //varying cooldown of the fire
    private bool fire_enabled = false;
    private bool alive = true;    

    protected override void Start()
    {
        _player = transform.parent.parent.GetComponent<Spawn_Manager>()._player;        
        _Anim = GetComponent<Animator>();
        _Collider = GetComponent<Collider2D>();
        //Audio components
        audio_destruction = transform.parent.GetComponent<AudioSource>();

        //Objects check
        if (_player == null) {
            Debug.LogWarning("Enemy could not obtain link to Player.");
        }
        if (_laser == null)
        {
            Debug.LogWarning("Enemy could not locate its laser.");
        }
        if (_Anim == null)
        {
            Debug.LogWarning("Enemy could not locate its animator.");
        }        
        if (_Collider == null)
        {
            Debug.LogWarning("Enemy could not locate its collider.");
        }       
        if (audio_destruction == null)
        {
            Debug.LogError("Enemy could not locate destruction audio in Enemy Container.");
        }
        //Parameter check
        if (v <= 0)
        {
            Debug.LogWarning("Enemy speed is less or equal to 0.");
        }
        if (t_fire_avg <= 0) {
            Debug.LogWarning("Enemy fire interval is invalid.");
        }

        base.Start();

        if(transform.position.x < 11f && transform.position.x > -11f && transform.position.y > -5.95f && transform.position.y < 7.5f)
        {
            Debug.LogWarning("An enemy appeared out of nowhere!");
            dir = Vector3.down;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        t_cooldown = Random.Range(0, t_fire_avg/2);
        StartCoroutine(Fire_cooldown());
    }

    protected override void Update()
    {
        base.Update();
        
        if (fire_enabled && alive)
        {
            if (Random.value < 0.5f)
            {
                Instantiate(_laser, transform.position - transform.up * 0.706f + transform.right * 0.09f, transform.rotation * Quaternion.FromToRotation(transform.up, -transform.up), transform.parent.parent.GetComponent<Spawn_Manager>()._laser_cont.transform);
            }
            else {
                Instantiate(_laser, transform.position - transform.up * 0.706f - transform.right * 0.09f, transform.rotation * Quaternion.FromToRotation(transform.up, -transform.up), transform.parent.parent.GetComponent<Spawn_Manager>()._laser_cont.transform);
            }
            t_cooldown = Random.Range(0.8f * t_fire_avg, 1.2f * t_fire_avg);
            StartCoroutine(Fire_cooldown());
        }
    }

    IEnumerator Fire_cooldown()
    {
        fire_enabled = false;
        yield return new WaitForSeconds(t_cooldown);
        fire_enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Fire")
        {
            Destroy(other.gameObject);
            _Collider.enabled = false;
            alive = false;
            _Anim.SetTrigger("Enemy_dead");
            _player.Kill_count(1);
            audio_destruction.PlayOneShot(audio_destruction.clip);
            Destroy(transform.gameObject, 2.37f);
        }
        if (other.tag == "Player")
        {
            _Collider.enabled = false;
            alive = false;
            _player.Enemy_collide();
            _Anim.SetTrigger("Enemy_dead");
            audio_destruction.PlayOneShot(audio_destruction.clip);
            Destroy(transform.gameObject, 2.37f);
        }
        if (other.tag == "Asteroid")
        {
            _Collider.enabled = false;
            alive = false;
            _Anim.SetTrigger("Enemy_dead");
            _player.Kill_count(1);
            audio_destruction.PlayOneShot(audio_destruction.clip);
            Destroy(transform.gameObject, 2.37f);
        }       
    }    
}
