using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_Object : MonoBehaviour
{
    protected float v = 2.5f;
    protected Vector3 dir = Vector3.down;    
    protected bool _isReturnable = false; //Return to the screen by a turnover

    protected virtual void Start()
    {
        if (transform.position.x > 11f)
        {
            dir = Vector3.left;
            transform.rotation = Quaternion.Euler(0, 0, -90f);
        }
        else if (transform.position.x < -11f)
        {
            dir = Vector3.right;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (transform.position.y > 7.5f)
        {
            dir = Vector3.down;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (transform.position.y < -5.95f)
        {
            dir = Vector3.up;
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    
    protected virtual void Update()
    {
        transform.Translate(Time.deltaTime * v * dir, Space.World);

        if ((dir == Vector3.left && transform.position.x < -22f) || (dir == Vector3.right && transform.position.x > 22f) ||
            (dir == Vector3.down && transform.position.y < -11.9f) || (dir == Vector3.up && transform.position.y > 15f))
        {
            if (_isReturnable)
            {
                Start();
            }
            else
            {
                Destroy(transform.gameObject);
            }
        }
    }

    //Trigger object return function
    public void SetReturn(bool ret)
    {
        _isReturnable = ret;
    }
}
