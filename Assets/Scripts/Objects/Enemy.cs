using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Moving_Object
{
    [SerializeField]
    private GameObject _laser;
    private Transform _laserContainer;
    private Animator _anim_Destruction;
    private Collider2D _collider;
    private AudioSource _audio_Destruction;
    
    private float _averageFirePause = 3f;
    private float _firePause = 0f;
    private bool _isFireEnabled = false;
    private bool _isAlive = true;    

    protected override void Start()
    {
        FindObjects();
        if (_speed <= 0)
        {
            Debug.LogWarning("Enemy speed is less or equal to 0.");
        }
        if (_averageFirePause <= 0)
        {
            Debug.LogError("Enemy fire interval is invalid.");
        }

        base.Start();

        _firePause = Random.Range(0, _averageFirePause/2);
        StartCoroutine(FireCooldown());
    }

    private void FindObjects()
    {
        _laserContainer = transform.parent.parent.GetComponent<Spawn_Manager>().laserContainer.transform;
        _anim_Destruction = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _audio_Destruction = transform.parent.GetComponent<AudioSource>();

        if (_laser == null)
        {
            Debug.LogWarning("Enemy could not locate its laser.");
        }
        if (_anim_Destruction == null)
        {
            Debug.LogWarning("Enemy could not locate its animator.");
        }
        if (_collider == null)
        {
            Debug.LogWarning("Enemy could not locate its collider.");
        }
        if (_audio_Destruction == null)
        {
            Debug.LogError("Enemy could not locate destruction audio in Enemy Container.");
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
            _anim_Destruction.SetTrigger("Enemy_dead");           
            _audio_Destruction.PlayOneShot(_audio_Destruction.clip);
            Destroy(other.gameObject);
            Destroy(transform.gameObject, 2.37f);            
        }
        if (other.CompareTag("Player"))
        {
            _collider.enabled = false;
            _isAlive = false;
            other.GetComponent<Player>().EnemyCollision();
            _anim_Destruction.SetTrigger("Enemy_dead");
            _audio_Destruction.PlayOneShot(_audio_Destruction.clip);
            Destroy(transform.gameObject, 2.37f);
        }
        if (other.CompareTag("Asteroid"))
        {
            _collider.enabled = false;
            _isAlive = false;
            _anim_Destruction.SetTrigger("Enemy_dead");            
            _audio_Destruction.PlayOneShot(_audio_Destruction.clip);
            Destroy(transform.gameObject, 2.37f);
        }       
    }    
}
