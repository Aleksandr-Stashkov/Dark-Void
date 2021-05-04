using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Asteroid : MovingObject
{
    [SerializeField]
    private GameObject _explosion;
    private SpawnManager _spawnManager;
    private AudioSource _audio_Destruction;

    private int _collisionDamege = 1;
    private float _rotationalSpeedMin = 1f;
    private float _rotationalSpeedMax = 11f;
    private bool _isWaveTrigger = false;
    private float _explosionAnimationLength = 0;

    protected override void Start()
    {
        FindObjects();
        
        _speed = 1.5f;
        _rotationalSpeed = Random.Range(_rotationalSpeedMin, _rotationalSpeedMax) * (Random.Range(0, 2) * 2 - 1);
        _isRotating = true;
        if (Mathf.Abs(_rotationalSpeed) < _rotationalSpeedMin || Mathf.Abs(_rotationalSpeed) > _rotationalSpeedMax)
        {
            Debug.LogAssertion("The asteroid's rotational speed is out of the set range.");
        }
        
        base.Start();
    }

    private void FindObjects()
    {
        if (_explosion == null)
        {
            Debug.LogError("Asteroid could not locate Explosion animation.");
        }
        else
        {
            _explosionAnimationLength = _explosion.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
            if (_explosionAnimationLength == 0)
            {
                Debug.LogAssertion("Asteroid could not determine the length of Explosion animation.");
            }
        }

        _spawnManager = transform.parent.parent.GetComponent<SpawnManager>();
        _audio_Destruction = transform.parent.GetComponent<AudioSource>();        
        if (_spawnManager == null)
        {
            Debug.LogError("Asteroid could not locate Spawn Manager.");
        }
        if (_audio_Destruction == null)
        {
            Debug.LogError("Asteroid could not locate audio in the Object Container.");
        }
    }   

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject newExplosion = Instantiate(_explosion, transform.position, transform.rotation, transform.parent);
        newExplosion.transform.localScale = transform.localScale;
        newExplosion.GetComponent<MovingObject>().FullSetup(_speed, _rotationalSpeed, _forwardDirection, false);

        transform.gameObject.SetActive(false);
        _audio_Destruction.PlayOneShot(_audio_Destruction.clip);

        if (other.CompareTag("Fire") || other.CompareTag("Fire_enemy"))
        {
            Destroy(other.gameObject);            
            if (_isWaveTrigger == true)
            {
                _spawnManager.TriggerWave();
            }           
        }
        else if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().ObjectCollision(_collisionDamege);            
        }

        Destroy(newExplosion.gameObject, _explosionAnimationLength);
        Destroy(transform.gameObject);
    }
    
    public void SetAsWaveTrigger()
    {
        _isWaveTrigger = true;
    }
}
