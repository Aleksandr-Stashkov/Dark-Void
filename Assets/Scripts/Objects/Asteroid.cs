using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Asteroid : Moving_Object
{
    private float _rotationalSpeed = 0f; //in rpm
    private float _rotationalSpeedMin = 1f;
    private float _rotationalSpeedMax = 11f;
    private bool _isWaveTrigger = false;

    [SerializeField]
    private GameObject _explosion;
    private Spawn_Manager _spawnManager;    
    private AudioSource _audio_Destruction;

    protected override void Start()
    {
        FindObjects();
        
        _speed = 1.5f;
        _rotationalSpeed = Random.Range(_rotationalSpeedMin, _rotationalSpeedMax) * (Random.Range(0, 2) * 2 - 1);               
        if (Mathf.Abs(_rotationalSpeed) < _rotationalSpeedMin || Mathf.Abs(_rotationalSpeed) > _rotationalSpeedMax)
        {
            Debug.LogWarning("The asteroid's rotational speed is out of the set range.");
        }
        
        base.Start();
    }

    private void FindObjects()
    {
        _spawnManager = transform.parent.parent.GetComponent<Spawn_Manager>();
        _audio_Destruction = transform.parent.GetComponent<AudioSource>();
        if (_explosion == null)
        {
            Debug.LogError("Asteroid could not locate Explosion animation.");
        }
        if (_spawnManager == null)
        {
            Debug.LogError("Asteroid could not locate Spawn Manager.");
        }
        if (_audio_Destruction == null)
        {
            Debug.LogError("Asteroid could not locate audio in the Object Container.");
        }
    }
        
    protected override void Update()
    {
        base.Update();
        transform.Rotate(0, 0, Time.deltaTime * _rotationalSpeed * 6);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject _newExplosion = Instantiate(_explosion, transform.position, transform.rotation, transform.parent);
        _newExplosion.transform.localScale = transform.localScale;    
        transform.gameObject.SetActive(false);
        _audio_Destruction.PlayOneShot(_audio_Destruction.clip);

        if (other.CompareTag("Fire") || other.CompareTag("Fire_enemy"))
        {
            Destroy(other.gameObject);            
            if (_isWaveTrigger == true)
            {
                _spawnManager.DestroyedAsteroid();
            }           
        }
        else if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().ObjectCollision();            
        }

        Destroy(_newExplosion.gameObject, 2.37f);
        Destroy(transform.gameObject, 0.28f);
    }
    
    public void SetAsWaveTrigger()
    {
        _isWaveTrigger = true;
    }
}
