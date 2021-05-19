using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _velocity = 12f;    
    private Player _playerSource;
    private LaserManager _laserManager;
    private bool _isChildOfManager = true;    

    private void Start()
    {
        _laserManager = transform.parent.GetComponent<LaserManager>();
        if (_laserManager == null)
        {
            Debug.LogError("Laser could not locate its Manager as a parent.");
            _isChildOfManager = false;
        }

        if (_velocity <= 0)
        {
            Debug.LogAssertion("Laser speed is equal to or less than 0.");           
        }
    }
        
    private void Update()
    {
        transform.Translate(transform.up * _velocity * Time.deltaTime, Space.World);

        if (transform.position.y > 8f || transform.position.y < -8f || transform.position.x > 12f || transform.position.x < -12f)
        {
            Dispose();
        }
    }

    public void Dispose()
    {
        if (_isChildOfManager)
        {
            gameObject.SetActive(false);
            _laserManager.AddLaserToReserve(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
        Dispose();
    }

    public void DisableReserve()
    {
        _isChildOfManager = false;
    }
}
