using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    [SerializeField]
    private GameObject _laser;
    private Transform _laserContainer;    
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

        _firePause = Random.Range(0, _averageFirePause/2);
        StartCoroutine(FireCooldown());
    }

    private void FindObjects()
    {
        _anim_Destruction = GetComponent<Animator>();
        if (_anim_Destruction == null)
        {
            Debug.LogError("Enemy could not locate its animator.");
        }
        _anim_ID_IsEnemyDead = Animator.StringToHash("isEnemyDead");
        _anim_Length = _anim_Destruction.runtimeAnimatorController.animationClips[0].length;

        _laserContainer = transform.parent.parent.GetComponent<SpawnManager>().LaserContainer();        
        _collider = GetComponent<Collider2D>();
        _audio_Destruction = transform.parent.GetComponent<AudioSource>();

        if (_laser == null)
        {
            Debug.LogError("Enemy could not locate its laser.");
        }        
        if (_collider == null)
        {
            Debug.LogError("Enemy could not locate its collider.");
        }
        if (_audio_Destruction == null)
        {
            Debug.LogError("Enemy could not locate destruction audio in Enemy Container.");
        }
        if (_anim_ID_IsEnemyDead == 0)
        {
            Debug.LogError("Enemy could not locate isEnemyDead parameter of the Animator.");
        }
    }

    protected override void Update()
    {
        base.Update();
        
        if (_isFireEnabled && _isAlive)
        {
            if (Random.value < 0.5f)
            {
                Instantiate(_laser, transform.position - transform.up * 0.706f + transform.right * 0.09f, transform.rotation * Quaternion.FromToRotation(transform.up, -transform.up), _laserContainer);
            }
            else
            {
                Instantiate(_laser, transform.position - transform.up * 0.706f - transform.right * 0.09f, transform.rotation * Quaternion.FromToRotation(transform.up, -transform.up), _laserContainer);
            }

            _firePause = Random.Range(0.8f * _averageFirePause, 1.2f * _averageFirePause);
            StartCoroutine(FireCooldown());
        }
    }

    IEnumerator FireCooldown()
    {
        _isFireEnabled = false;
        yield return new WaitForSeconds(_firePause);
        _isFireEnabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fire"))
        {                     
            _collider.enabled = false;
            other.gameObject.SetActive(false);
            other.transform.GetComponent<Laser>().AddScore(1);
            _isAlive = false;
            _anim_Destruction.SetTrigger(_anim_ID_IsEnemyDead);           
            _audio_Destruction.PlayOneShot(_audio_Destruction.clip);
            Destroy(other.gameObject);
            Destroy(transform.gameObject, _anim_Length);            
        }
        if (other.CompareTag("Player"))
        {
            _collider.enabled = false;
            _isAlive = false;
            other.GetComponent<Player>().EnemyCollision(_collisionDamage);
            _anim_Destruction.SetTrigger(_anim_ID_IsEnemyDead);
            _audio_Destruction.PlayOneShot(_audio_Destruction.clip);
            Destroy(transform.gameObject, _anim_Length);
        }
        if (other.CompareTag("Asteroid"))
        {
            _collider.enabled = false;
            _isAlive = false;
            _anim_Destruction.SetTrigger(_anim_ID_IsEnemyDead);            
            _audio_Destruction.PlayOneShot(_audio_Destruction.clip);
            Destroy(transform.gameObject, 2.37f);
        }       
    }    
}
