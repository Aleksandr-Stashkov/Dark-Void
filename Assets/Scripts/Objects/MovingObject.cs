using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    protected float _speed = 2.5f;
    protected Vector3 _forwardDirection = Vector3.down;

    protected float _rotationalSpeed = 0f; //in rpm
    protected bool _isRotating = false;

    protected bool _isTurningBack = false;

    protected virtual void Start()
    {
        if (transform.position.x > 11f)
        {
            _forwardDirection = Vector3.left;
            transform.rotation = Quaternion.Euler(0, 0, -90f);
        }
        else if (transform.position.x < -11f)
        {
            _forwardDirection = Vector3.right;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (transform.position.y > 7.5f)
        {
            _forwardDirection = Vector3.down;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (transform.position.y < -5.95f)
        {
            _forwardDirection = Vector3.up;
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        if (transform.position.x < 11f && transform.position.x > -11f && transform.position.y > -5.95f && transform.position.y < 7.5f)
        {
            Debug.LogWarning("A " + this.gameObject.name + " appeared within the view.");
            _forwardDirection = Vector3.down;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (_speed <= 0)
        {
            Debug.LogAssertion(this.name + " speed is less or equal to 0.");
        }
    }
    
    protected virtual void Update()
    {
        transform.Translate(Time.deltaTime * _speed * _forwardDirection, Space.World);

        if ((_forwardDirection == Vector3.left && transform.position.x < -22f) || (_forwardDirection == Vector3.right && transform.position.x > 22f) ||
            (_forwardDirection == Vector3.down && transform.position.y < -11.9f) || (_forwardDirection == Vector3.up && transform.position.y > 15f))
        {
            if (_isTurningBack)
            {
                Start();
            }
            else
            {
                Dispose();
            }
        }

        if (_isRotating)
        {
            transform.Rotate(0, 0, Time.deltaTime * _rotationalSpeed * 6);
        }
    }

    protected virtual void Dispose()
    {
        Destroy(gameObject);
    }
        
    public virtual void Activate()
    {
        gameObject.SetActive(true);
        Start();
    }

    public void SetForwardDirection(Vector3 forawdDirection)
    {
        _forwardDirection = forawdDirection;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetRotationalSpeed(float rotationalSpeed)
    {
        _rotationalSpeed = rotationalSpeed;
    }

    public void SetRotating(bool isRotating)
    {
        _isRotating = isRotating;
    }
    
    public void SetTurningBack(bool isTurningBack)
    {
        _isTurningBack = isTurningBack;
    }

    public void FullSetup(float speed, float rotationalSpeed, Vector3 forwardDirection, bool isTurningBack)
    {
        _speed = speed;
        if (rotationalSpeed == 0)
        {
            _isRotating = false;
        }
        else
        {
            _rotationalSpeed = rotationalSpeed;
        }
        _forwardDirection = forwardDirection;
        _isTurningBack = isTurningBack;
    }
}
