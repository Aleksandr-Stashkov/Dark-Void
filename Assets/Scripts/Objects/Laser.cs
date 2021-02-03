using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float v = 12f;

    void Start()
    {
        //Parameter check
        if (v <= 0)
        {
            Debug.LogWarning("Laser speed is equal to or less than 0.");
        }
    }
        
    void Update()
    {
        transform.Translate(transform.up * v * Time.deltaTime, Space.World);

        if (transform.position.y > 8f || transform.position.y < -8f || transform.position.x > 10f || transform.position.x < -10f){
            if (transform.parent.tag == "Fire")
            {
                // Triple laser destruction
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(transform.gameObject);
            }
        }
    }
}
