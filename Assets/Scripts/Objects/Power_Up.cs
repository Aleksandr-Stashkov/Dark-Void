using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_Up : Moving_Object
{
    protected override void Start()
    {
        v = 3f;
        if (v <= 0)
        {
            Debug.LogWarning("The powerup's speed is not positive.");
        }

        base.Start();

        if (transform.position.x < 11f && transform.position.x > -11f && transform.position.y > -5.95f && transform.position.y < 7.5f)
        {
            Debug.LogWarning("A powerup appeared out of nowhere!");
            dir = Vector3.down;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }    
}
