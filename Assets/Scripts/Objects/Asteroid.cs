using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Asteroid : MovingObject
{
    [SerializeField]
    private GameObject _explosion;
    private SpawnManager _spawnManager;
    private ObjectManager _objectManager;
    private AudioSource _audio_Destruction;

    private int _collisionDamege = 1;
    private float _rotationalSpeedMin = 1f;
    private float _rotationalSpeedMax = 11f;
    private bool _isWaveTrigger = false;
    private float _explosionAnimationLength = 0;
    private bool _isChildOfManager = true;

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
            Debug.LogError("Asteroid could not locate Explosion.");
        }
        else
        {
            Animator anim_Explosion = _explosion.GetComponent<Animator>();
            if (anim_Explosion == null)
            {
                Debug.LogError("Asteroid could not locate Animator component of Explosion.");
            }
            else
            {
                _explosionAnimationLength = anim_Explosion.runtimeAnimatorController.animationClips[0].length;
                if (_explosionAnimationLength == 0)
                {
                    Debug.LogAssertion("Asteroid could not determine the length of Explosion animation.");
                }
            }
        }

        Transform parent = transform.parent;
        if (parent == null)
        {
            Debug.LogError("Asteroid could not locate its parent.");
            _isChildOfManager = false;
        }
        else
        {
            _objectManager = parent.GetComponent<ObjectManager>();            
            _audio_Destruction = parent.GetComponent<AudioSource>();
            if (_objectManager == null)
            {
                Debug.LogError("Asteroid could not locate Object Manager on the parent.");
                _isChildOfManager = false;
            }            
            if (_audio_Destruction == null)
            {
                Debug.LogError("Asteroid could not locate Audio Source on the parent.");
            }
            parent = parent.parent;
            if (parent == null)
            {
                Debug.LogError("Asteroid could not locate its parent.parent.");
            }
            else
            {
                _spawnManager = parent.GetComponent<SpawnManager>();
                if (_spawnManager == null)
                {
                    Debug.LogError("Asteroid could not locate Spawn Manager on its parent.parent.");
                }
            }
        }
    }   

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject newExplosion = Instantiate(_explosion, transform.position, transform.rotation, transform.parent);
        newExplosion.transform.localScale = transform.localScale;
        newExplosion.GetComponent<MovingObject>().FullSetup(_speed, _rotationalSpeed, _forwardDirection, false);

        gameObject.SetActive(false);
        _audio_Destruction.PlayOneShot(_audio_Destruction.clip);

        if (other.CompareTag("Fire") || other.CompareTag("Fire_enemy"))
        {
            Laser laser = other.GetComponent<Laser>();
            if (laser == null)
            {
                Debug.LogError("Asteroid could not locate Laser component on Collider.");
                Destroy(other);
            }
            else
            {
                other.GetComponent<Laser>().Dispose();
            }
            if (_isWaveTrigger == true)
            {
                _spawnManager.TriggerWave();
            }         
        }
        else if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player == null)
            {
                Debug.LogError("Asteroid could not locate Player component on Collider.");
            }
            else
            {
                player.ObjectCollision(_collisionDamege);
            }
        }

        Destroy(newExplosion.gameObject, _explosionAnimationLength);
        Dispose();
    }

    protected override void Dispose()
    {
        if (_isChildOfManager)
        {
            gameObject.SetActive(false);
            _objectManager.AddToReserve(gameObject);
        }
        else
        {
            base.Dispose();
        }
    }    

    public void SetAsWaveTrigger()
    {
        _isWaveTrigger = true;
    }
}
