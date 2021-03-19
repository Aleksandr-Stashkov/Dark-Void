using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Asteroid : Moving_Object
{
    private float w = 0f; //rotation speed in rpm
    private float w_min = 1f;  //rotation speed limits
    private float w_max = 11f;
    private bool trigger = false; //destruction serves as a trigger

    [SerializeField]
    private GameObject _Explosion;
    private Player _player;
    private Spawn_Manager _spawnManager;
    //Audio
    private AudioSource audio_destruction;

    protected override void Start()
    {
        _spawnManager = transform.parent.parent.GetComponent<Spawn_Manager>();
        _player = _spawnManager._player;
        audio_destruction = transform.parent.GetComponent<AudioSource>();
        //Objects check        
        if (_Explosion == null)
        {
            Debug.LogError("Asteroid could not locate Explosion animation.");
        }       
        if (_spawnManager == null)
        {
            Debug.LogError("Asteroid could not locate Spawn Manager.");
        }        
        if (_player == null)
        {
            Debug.LogError("Asteroid could not locate Player.");
        }
        if (audio_destruction == null)
        {
            Debug.LogError("Asteroid could not locate audio in the Object Container.");
        }

        v = 1.5f;
        w = Random.Range(w_min, w_max) * (Random.Range(0, 2) * 2 - 1);
        //Parameters check
        if (v <= 0)
        {
            Debug.LogWarning("The asteroid's speed is not positive.");
        }
        if (Mathf.Abs(w)<w_min || Mathf.Abs(w) > w_max)
        {
            Debug.LogWarning("The asteroid's rotational speed is out of the set range.");
        }
        
        base.Start();

        if (transform.position.x < 11f && transform.position.x > -11f && transform.position.y > -5.95f && transform.position.y < 7.5f)
        {
            Debug.LogWarning("An asteroid appeared out of nowhere!");
            dir = Vector3.down;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
        
    protected override void Update()
    {
        base.Update();
        transform.Rotate(0, 0, Time.deltaTime * w * 6);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject _new_explosion = Instantiate(_Explosion, transform.position, transform.rotation, transform.parent);
        _new_explosion.transform.localScale = transform.localScale;       
        transform.gameObject.SetActive(false);
        audio_destruction.PlayOneShot(audio_destruction.clip);

        if (other.CompareTag("Fire") || other.CompareTag("Fire_enemy"))
        {
            Destroy(other.gameObject);            
            if (trigger == true)
            {
                _spawnManager.Asteroid_destroyed();
            }           
        }
        if (other.CompareTag("Player"))
        {
            _player.Object_collide();            
        }

        Destroy(_new_explosion.gameObject, 2.37f);
        Destroy(transform.gameObject, 0.28f);
    }

    //Setting trigger option
    public void SetTrigger(bool set)
    {
        trigger = set;
    }
}
