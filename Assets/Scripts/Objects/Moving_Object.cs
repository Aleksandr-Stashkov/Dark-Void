using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_Object : MonoBehaviour
{
    protected float _speed = 2.5f;
    protected Vector3 _forwardDirection = Vector3.down;    
    private bool _isTurningBack = false;

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
            Debug.LogWarning("A " + this.gameObject.name + " appeared out of nowhere!");
            _forwardDirection = Vector3.down;
            transform.rotation = Quaternion.Euler(0, 0, 0);
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
                Destroy(transform.gameObject);
            }
        }
    }
    
    public void SetReturn(bool isTurningBack)
    {
        _isTurningBack = isTurningBack;
    }
}
