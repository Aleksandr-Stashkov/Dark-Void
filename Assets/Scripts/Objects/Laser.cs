using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _velocity = 12f;    
    private Player _playerSource;

    private void Start()
    {
        if (_velocity <= 0)
        {
            Debug.LogAssertion("Laser speed is equal to or less than 0.");
        }
    }
        
    private void Update()
    {
        transform.Translate(transform.up * _velocity * Time.deltaTime, Space.World);

        if (transform.position.y > 8f || transform.position.y < -8f || transform.position.x > 10f || transform.position.x < -10f)
        {
            if (transform.parent.CompareTag("Fire"))
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(transform.gameObject);
            }
        }
    }

    public void SetPlayer(Player player)
    {
        if (player == null)
        {
            Debug.LogError("Laser was handled an empty player.");
        }
        else
        {
            _playerSource = player;
        }
    }

    public void AddScore(int score)
    {
        _playerSource.AddScore(score);
    }
}
