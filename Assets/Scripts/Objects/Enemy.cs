using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    private LaserManager _laserManager;
    private EnemyManager _enemyManager;
    private Collider2D _collider;
    private AudioSource _audio_Destruction;

    private Animator _anim_Destruction;
    private int _anim_ID_IsEnemyDead = 0;
    private float _anim_Length;

    private int _collisionDamage = 1;
    private float _averageFirePause = 3f;
    private float _firePause = 0f;
    private bool _isFireEnabled = false;
    private bool _isAlive = true;
    private bool _isChildOfManager = true;

    protected override void Start()
    {
        FindObjects();
       
        if (_averageFirePause <= 0)
        {
            Debug.LogAssertion("Enemy fire interval is invalid.");
        }
        if (_collisionDamage < 0)
        {
            Debug.LogAssertion("Enemy collision damage is negative.");
        }

        base.Start();

        _collider.enabled = true;
        _isAlive = true;

        _firePause = Random.Range(0, _averageFirePause/2);
        StartCoroutine(FireCooldown());
    }

    private void FindObjects()
    {
        _collider = GetComponent<Collider2D>();
        if (_collider == null)
        {
            Debug.LogError("Enemy could not locate its collider.");
        }
        _anim_Destruction = GetComponent<Animator>();
        if (_anim_Destruction == null)
        {
            Debug.LogError("Enemy could not locate its animator.");
        }
        else
        {
            _anim_ID_IsEnemyDead = Animator.StringToHash("isEnemyDead");
            _anim_Length = _anim_Destruction.runtimeAnimatorController.animationClips[0].length / 1.75f;
            if (_anim_ID_IsEnemyDead == 0)
            {
                Debug.LogError("Enemy could not locate isEnemyDead parameter of the Animator.");
            }
        }

        Transform parent = transform.parent;
        if (parent == null)
        {
            Debug.LogError("Enemy could not locate its parent.");
            _isChildOfManager = false;
        }
        else
        {
            _enemyManager = parent.GetComponent<EnemyManager>();
            _audio_Destruction = parent.GetComponent<AudioSource>();
            if (_enemyManager == null)
            {
                Debug.LogError("Enemy could not locate its Manager on the parent.");
                _isChildOfManager = false;
            }
            if (_audio_Destruction == null)
            {
                Debug.LogError("Enemy could not locate destruction audio in Enemy Container.");
            }
        }

        parent = parent.parent;
        if (parent == null)
        {
            Debug.LogError("Enemy could not locate its paret.parent.");
        }
        else
        {
            SpawnManager spawnManager = parent.GetComponent<SpawnManager>();
            if (spawnManager == null)
            {
                Debug.LogError("Enemy could not locate Spawn Manager on its parent.parent.");
            }
            else
            {
                _laserManager = spawnManager.LaserContainer();
                if (_laserManager == null)
                {
                    Debug.LogAssertion("Enemy received invalid Laser Manager reference from Spawn Manager.");
                }
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        
        if (_isFireEnabled && _isAlive)
        {
            _laserManager.CreateEnemyLaser(transform);
            
            StartCoroutine(FireCooldown());
        }
    }

    IEnumerator FireCooldown()
    {
        _isFireEnabled = false;
        _firePause = Random.Range(0.8f * _averageFirePause, 1.2f * _averageFirePause);
        yield return new WaitForSeconds(_firePause);
        _isFireEnabled = true;
    }

    protected override void Dispose()
    {
        if (_isChildOfManager)
        {
            gameObject.SetActive(false);
            _enemyManager.AddToReserve(gameObject);
        }
        else
        {
            base.Dispose();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fire"))
        {                     
            _collider.enabled = false;
            other.gameObject.SetActive(false);
            Laser laser = other.transform.GetComponent<Laser>();
            if (laser == null)
            {
                Debug.LogError("Enemy could not locate Laser component on Collider.");
            }
            else
            {
                laser.AddScore(1);
            }
            _isAlive = false;
            _anim_Destruction.SetTrigger(_anim_ID_IsEnemyDead);           
            _audio_Destruction.PlayOneShot(_audio_Destruction.clip);
            StartCoroutine(Destruction());            
        }
        if (other.CompareTag("Player"))
        {
            _collider.enabled = false;
            _isAlive = false;
            Player player = other.GetComponent<Player>();
            if (player == null)
            {
                Debug.LogError("Enemy could not locate Player component on Collider.");
            }
            else
            {
                player.EnemyCollision(_collisionDamage);
            }
            _anim_Destruction.SetTrigger(_anim_ID_IsEnemyDead);
            _audio_Destruction.PlayOneShot(_audio_Destruction.clip);
            StartCoroutine(Destruction());
        }
        if (other.CompareTag("Asteroid"))
        {
            _collider.enabled = false;
            _isAlive = false;
            _anim_Destruction.SetTrigger(_anim_ID_IsEnemyDead);            
            _audio_Destruction.PlayOneShot(_audio_Destruction.clip);
            StartCoroutine(Destruction());
        }       
    }

    private IEnumerator Destruction()
    {
        yield return new WaitForSeconds(_anim_Length);
        Dispose();
    }

    public override void Activate()
    {
        base.Activate();
    }
}
