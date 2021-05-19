using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private LaserManager _laserManager;
    protected UI_Manager _UI_Manager;
    //Associated objects
    private GameObject _shield, _thruster, _damage1, _damage2, _damage3;    
    private AudioSource _audio_Source;
    [SerializeField]
    private AudioClip _audio_Fire, _audio_PowerUp/*, _audio_Damage*/;
    //Movement
    protected bool _isUserControlled = false;
    protected bool _isRotationOff = true;
    protected float _playerEntranceDuration = 0f;
    private float _sidewardSpeed = 6f;
    private float _forwardSpeed = 8f;
    private float _backwardSpeed = 4.5f;
    private bool _isSpedUp = false;
    private float _speedUpFactor = 1.5f;
    private float _speedUpDuration = 5f;    
    private direction _forwardDirection = direction.up;
    //Fire
    protected bool _isFireEnabled = true;
    private float _laserCooldownDuration = 0.1f;
    private bool _isTripleFire = false;
    private float _tripleFireDuration = 5f;
    //Player stat
    protected int _lives = 4;
    protected int _score = 0;
    //Animation    
    private Animator _anim_Turning;
    private int _anim_ID_TurnLeft, _anim_ID_TurnRight; //ids for Animator variables
    private bool _isThrustOn = false;
    private bool _wasThrustOn = false; //for prevention of excess changes in components  
    private bool _wasTurningLeft = false;
    private bool _wasTurningRight = false;
    //Collision counters
    private int _fireCooldownCount = 0;
    private int _tripleFireCount = 0;
    private int _speedUpCount = 0;

    private void Start()
    {
        FindObjects();
        CheckParameters();

        InitialSetting();
        
        StartCoroutine(EntranceTimer());
    }

    private void FindObjects()
    {
        _UI_Manager = GameObject.Find("Canvas").GetComponent<UI_Manager>();        

        _anim_Turning = GetComponent<Animator>();
        if (_anim_Turning == null)
        {
            Debug.LogError("Player could not locate its animator.");
        }
        _anim_ID_TurnLeft = Animator.StringToHash("Turn_Left");
        _anim_ID_TurnRight = Animator.StringToHash("Turn_Right");        
        _audio_Source = GetComponent<AudioSource>();

        Transform child;
        for (int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);
            switch (child.name)
            {
                case "Shield":
                    _shield = child.gameObject;
                    break;
                case "Fire_1":
                    _damage1 = child.gameObject;
                    break;
                case "Fire_2":
                    _damage2 = child.gameObject;
                    break;
                case "Fire_3":
                    _damage3 = child.gameObject;
                    break;
                case "Thruster":
                    _thruster = child.gameObject;
                    break;
                default:
                    Debug.LogWarning("There is an unrecognized child of Player.");
                    break;
            }
        }

        CheckObjects();
    }

    private void CheckObjects()
    {
        if (_UI_Manager == null)
        {
            Debug.LogError("Player could not obtain link to UI Canvas.");
        }
        if (_audio_Source == null)
        {
            Debug.LogError("Player could not locate its audio source.");
        }
        if (_audio_Fire == null)
        {
            Debug.LogError("Player could not locate fire audio.");
        }
        if (_audio_PowerUp == null)
        {
            Debug.LogError("Player could not locate powerup audio.");
        }
        /*if (_audio_Damage == null)
        {
            Debug.LogError("Player could not locate damage audio.");
        }*/
        if (_anim_ID_TurnLeft == 0)
        {
            Debug.LogError("Player's animator could not find Turn_Left parameter.");
        }
        if (_anim_ID_TurnRight == 0)
        {
            Debug.LogError("Player's animator could not find Turn_Right parameter.");
        }        
        if (_shield == null)
        {
            Debug.LogError("Player could not locate Shield.");
        }
        if (_thruster == null)
        {
            Debug.LogError("Player could not locate thruster.");
        }
        if (_damage1 == null)
        {
            Debug.LogError("Player could not locate Damage Fire 1.");
        }
        if (_damage2 == null)
        {
            Debug.LogError("Player could not locate Damage Fire 2.");
        }
        if (_damage3 == null)
        {
            Debug.LogError("Player could not locate Damage Fire 3.");
        }
    }

    private void CheckParameters()
    {
        if (_sidewardSpeed <= 0)
        {
            Debug.LogAssertion("Player horizontal speed is equal to or less than 0.");
        }
        if (_forwardSpeed <= 0)
        {
            Debug.LogAssertion("Player forward speed is equal to or less than 0.");
        }
        if (_backwardSpeed <= 0)
        {
            Debug.LogAssertion("Player backward speed is equal to or less than 0.");
        }
        if (_lives <= 0)
        {
            Debug.LogAssertion("Player health is set below 1.");
        }
        if (_isUserControlled)
        {
            Debug.LogWarning("User will control the Player from the start.");
        }       
    }

    protected virtual void InitialSetting()
    {
        _shield.SetActive(false);
        _damage1.SetActive(false);
        _damage2.SetActive(false);
        _damage3.SetActive(false);

        _anim_Turning.SetBool(_anim_ID_TurnLeft, _wasTurningLeft);
        _anim_Turning.SetBool(_anim_ID_TurnRight, _wasTurningRight);

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    
    protected virtual IEnumerator EntranceTimer()
    {
        yield return new WaitForSeconds(_playerEntranceDuration);
        _isUserControlled = true;
    }

    protected virtual void Update()
    {
            if (Time.timeSinceLevelLoad <= _playerEntranceDuration)
            {
                EntranceMovement();
            }
    }

    protected virtual void Movement()
    {
        float horizontalInput = GetHorizontal();
        float verticalInput = GetVertical();
        
        if (_isSpedUp)
        {
            horizontalInput *= _speedUpFactor;
            verticalInput *= _speedUpFactor;
        }

        switch (_forwardDirection)
        {
            case direction.up:
                if (verticalInput > 0)
                {
                    transform.Translate((Vector3.up * _forwardSpeed * verticalInput + Vector3.right * _sidewardSpeed * horizontalInput) * Time.deltaTime, Space.World);
                    _isThrustOn = true;
                }
                else if (verticalInput <= 0)
                {
                    transform.Translate((Vector3.up * _backwardSpeed * verticalInput + Vector3.right * _sidewardSpeed * horizontalInput) * Time.deltaTime, Space.World);
                    _isThrustOn = false;
                }                
                TurningAnimation(horizontalInput);
                break;
            case direction.down:
                if (verticalInput >= 0)
                {
                    transform.Translate((Vector3.up * _backwardSpeed * verticalInput + Vector3.right * _sidewardSpeed * horizontalInput) * Time.deltaTime, Space.World);
                    _isThrustOn = false;
                }
                else if (verticalInput < 0)
                {
                    transform.Translate((Vector3.up * _forwardSpeed * verticalInput + Vector3.right * _sidewardSpeed * horizontalInput) * Time.deltaTime, Space.World);
                    _isThrustOn = true;
                }
                TurningAnimation(horizontalInput);
                break;
            case direction.right:
                if (horizontalInput > 0)
                {
                    transform.Translate((Vector3.up * _sidewardSpeed * verticalInput + Vector3.right * _forwardSpeed * horizontalInput) * Time.deltaTime, Space.World);
                    _isThrustOn = true;
                }
                else if (horizontalInput <= 0)
                {
                    transform.Translate((Vector3.up * _sidewardSpeed * verticalInput + Vector3.right * _backwardSpeed * horizontalInput) * Time.deltaTime, Space.World);
                    _isThrustOn = false;
                }
                TurningAnimation(verticalInput);
                break;
            case direction.left:
                if (horizontalInput >= 0)
                {
                    transform.Translate((Vector3.up * _sidewardSpeed * verticalInput + Vector3.right * _backwardSpeed * horizontalInput) * Time.deltaTime, Space.World);
                    _isThrustOn = false;
                }
                else if (horizontalInput < 0)
                {
                    transform.Translate((Vector3.up * _sidewardSpeed * verticalInput + Vector3.right * _forwardSpeed * horizontalInput) * Time.deltaTime, Space.World);
                    _isThrustOn = true;
                }
                TurningAnimation(verticalInput);
                break;
        }
        //Position limits
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.15f, 9.15f), Mathf.Clamp(transform.position.y, -3.50f, 5.67f), 0);

        ScaleThruster();          
    }

    protected virtual float GetHorizontal()
    {
        return 0f;
    }

    protected virtual float GetVertical()
    {
        return 0f;
    }
    
    private void TurningAnimation(float rightwardInput)
    {
        if (rightwardInput == 0 && (_wasTurningLeft || _wasTurningRight))
        {
            _anim_Turning.SetBool(_anim_ID_TurnRight, false);
            _anim_Turning.SetBool(_anim_ID_TurnLeft, false);
            _wasTurningLeft = false;
            _wasTurningRight = false;
        }
        else if (rightwardInput > 0 && !_wasTurningRight)
        {
            _anim_Turning.SetBool(_anim_ID_TurnRight, true);
            _wasTurningRight = true;
        }
        else if (rightwardInput < 0 && !_wasTurningLeft)
        {
            _anim_Turning.SetBool(_anim_ID_TurnLeft, true);
            _wasTurningLeft = true;
        }
    }

    private void ScaleThruster()
    {
        if (_wasThrustOn != _isThrustOn)
        {
            if (_isThrustOn)
            {
                _thruster.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            }
            else
            {
                _thruster.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            _wasThrustOn = _isThrustOn;
        }
    }
    
    protected void RotationToMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        transform.rotation *= Quaternion.FromToRotation(transform.up, (mousePosition - transform.position));
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
    
    protected virtual void EntranceMovement()
    {
        transform.Translate(Vector3.up * 5f / _playerEntranceDuration * Time.deltaTime, Space.World);        
    }

    protected void Fire()
    {
        if (_isTripleFire == true)
        {
            _laserManager.CreateTripleLaser(this);
            _audio_Source.PlayOneShot(_audio_Fire);
            _audio_Source.PlayOneShot(_audio_Fire);
            _audio_Source.PlayOneShot(_audio_Fire);
        }
        else
        {
            _laserManager.CreateLaser(this);
        }

        StartCoroutine(FireCooldown());
    }

    private IEnumerator FireCooldown()
    {
        _fireCooldownCount++;
        _isFireEnabled = false;
        yield return new WaitForSeconds(_laserCooldownDuration);

        _fireCooldownCount--;
        if (_fireCooldownCount == 0)
        {
            _isFireEnabled = true;
        }
    }
    
    private IEnumerator ActivateTripleFire()
    {
        _tripleFireCount++;
        _isTripleFire = true;
        yield return new WaitForSeconds(_tripleFireDuration);

        _tripleFireCount--;
        if (_tripleFireCount == 0) {
            _isTripleFire = false;
        }
    }
    
    private IEnumerator ActivateSpeedUp()
    {
        _speedUpCount++;
        _isSpedUp = true;
        yield return new WaitForSeconds(_speedUpDuration);

        _speedUpCount--;
        if (_speedUpCount == 0)
        {
            _isSpedUp = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PU_TripleFire"))
        {
            StartCoroutine(ActivateTripleFire());
            _audio_Source.PlayOneShot(_audio_PowerUp);
            other.GetComponent<PowerUp>().DisposeOf_PU();
        }
        if (other.CompareTag("PU_SpeedUp"))
        {
            StartCoroutine(ActivateSpeedUp());
            _audio_Source.PlayOneShot(_audio_PowerUp);
            other.GetComponent<PowerUp>().DisposeOf_PU();
        }
        if (other.CompareTag("PU_Shield"))
        {
            _shield.gameObject.SetActive(true);
            _audio_Source.PlayOneShot(_audio_PowerUp);
            other.GetComponent<PowerUp>().DisposeOf_PU();
        }
        if (other.CompareTag("Fire_enemy"))
        {
            other.GetComponent<Laser>().Dispose();
            ObjectCollision(1);
        }
    }
    
    public void EnemyCollision(int damage)
    {
        AddScore(1);
        if (_shield.activeSelf)
        {          
          _shield.SetActive(false);
        }
        else
        {
          TakeDamage(damage);         
        }
    }
    
    public void ObjectCollision(int damage)
    {
        if (_shield.activeSelf)
        {
            _shield.SetActive(false);
        }
        else
        {
            TakeDamage(damage);       
        }
    }
    
    public virtual void AddScore(int add)
    {
        _score += add;        
    }
    
    public virtual void TakeDamage(int damage)
    {
        //_audio_Source.PlayOneShot(_audio_Damage);

        if (damage > _lives)
        {
            _lives = 0;
        }
        else {
            _lives -= damage;
            switch (_lives)
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

        if (_lives <= 0)
        {
            _UI_Manager.GameOver();
            transform.gameObject.SetActive(false);
            return;
        }
        else
        {
            StartCoroutine(FireCooldown());
        }
    }
        
    public bool IsAlive()
    {
        if (_lives > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void SetEntranceDuration(float duration)
    {
        _playerEntranceDuration = duration;
        if(_playerEntranceDuration == 0)
        {
            _isUserControlled = true;
        }
    }
    
    public void StopRotation(Vector3 fixedForwardDirection, float stopDuration)
    {
        _isRotationOff = true;
        StartCoroutine(Stop_rotation(fixedForwardDirection, stopDuration, stopDuration + Time.timeSinceLevelLoad));
    }

    protected IEnumerator Stop_rotation(Vector3 forwardDirection, float stopDuration, float endTime)
    {
        if (stopDuration == 0) {
            Debug.LogAssertion("Coroutine Stop_rotation got 0 as the time of execution.");
            yield return null;
        }

        float angularSpeed;
        float angleOfRotation = Quaternion.FromToRotation(transform.up, forwardDirection).eulerAngles.z;
        if (Mathf.Abs(angleOfRotation) <= 180) {
            angularSpeed = angleOfRotation * 0.05f / stopDuration;
        }
        else
        {
            angularSpeed = (angleOfRotation-360) * 0.05f / stopDuration;
        }

        while (Time.timeSinceLevelLoad < endTime)
        {
            transform.Rotate(0,0,angularSpeed);
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
        transform.rotation *= Quaternion.FromToRotation(transform.up, forwardDirection);
        if (_damage1.activeSelf)
        {
            _damage1.transform.rotation *= Quaternion.FromToRotation(_damage1.transform.up, forwardDirection);
        }
        if (_damage2.activeSelf)
        {
            _damage2.transform.rotation *= Quaternion.FromToRotation(_damage2.transform.up, forwardDirection);
        }
        if (_damage3.activeSelf)
        {
            _damage3.transform.rotation *= Quaternion.FromToRotation(_damage3.transform.up, forwardDirection);
        }        
    }   

    public virtual bool IsPlayer1() { return true; }

    public void SetLaserContainer(LaserManager laserManager) 
    {
        if (laserManager == null)
        {
            Debug.LogAssertion("Player was handled an empty Laser Manager.");
        }
        else
        {
            _laserManager = laserManager;
        }
    }
}
