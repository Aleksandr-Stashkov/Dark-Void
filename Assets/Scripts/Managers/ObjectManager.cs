using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _asteroid;
    private List<GameObject> _reservedAsteroids = new List<GameObject>();
    private bool _isReserveActive = true;
    //Asteroid parameters    
    private float _asteroidScaleMin = 0.3f;
    private float _asteroidScaleMax = 0.65f;

    private void Start()
    {
        if (_asteroid == null)
        {
            Debug.LogError("Object Manager could not locate PREFAB for Asteroid.");
        }
    }
    //Asteroid serves as a trigger for the wave start
    public void CreateAsteroidTrigger(direction waveDirection)
    {
        float scale = Random.Range(_asteroidScaleMin, _asteroidScaleMax);
        GameObject newAsteroid;

        switch (waveDirection)
        {
            case direction.down:
                if (_reservedAsteroids.Count > 0)
                {
                    newAsteroid = _reservedAsteroids[0];
                    _reservedAsteroids.RemoveAt(0);
                    newAsteroid.transform.position = new Vector3(Random.Range(-8.25f, 8.25f), 7.8f);
                    newAsteroid.GetComponent<Asteroid>().Activate();
                }
                else
                {
                    newAsteroid = Instantiate(_asteroid, new Vector3(Random.Range(-8.25f, 8.25f), 7.8f), Quaternion.identity, this.transform);
                }
                newAsteroid.transform.localScale = new Vector3(scale, scale, scale);
                newAsteroid.GetComponent<Asteroid>().SetAsWaveTrigger();
                break;
            case direction.up:
                if (_reservedAsteroids.Count > 0)
                {
                    newAsteroid = _reservedAsteroids[0];
                    _reservedAsteroids.RemoveAt(0);
                    newAsteroid.transform.position = new Vector3(Random.Range(-8.25f, 8.25f), -6f);
                    newAsteroid.GetComponent<Asteroid>().Activate();
                }
                else
                {
                    newAsteroid = Instantiate(_asteroid, new Vector3(Random.Range(-8.25f, 8.25f), -6f), Quaternion.identity, this.transform);
                }
                newAsteroid.transform.localScale = new Vector3(scale, scale, scale);
                newAsteroid.GetComponent<Asteroid>().SetAsWaveTrigger();
                break;
            case direction.right:
                if (_reservedAsteroids.Count > 0)
                {
                    newAsteroid = _reservedAsteroids[0];
                    _reservedAsteroids.RemoveAt(0);
                    newAsteroid.transform.position = new Vector3(-11.5f, Random.Range(-3.9f, 5.75f));
                    newAsteroid.GetComponent<Asteroid>().Activate();
                }
                else
                {
                    newAsteroid = Instantiate(_asteroid, new Vector3(-11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                }
                newAsteroid.transform.localScale = new Vector3(scale, scale, scale);
                newAsteroid.GetComponent<Asteroid>().SetAsWaveTrigger();
                break;
            case direction.left:
                if (_reservedAsteroids.Count > 0)
                {
                    newAsteroid = _reservedAsteroids[0];
                    _reservedAsteroids.RemoveAt(0);
                    newAsteroid.transform.position = new Vector3(11.5f, Random.Range(-3.9f, 5.75f));
                    newAsteroid.GetComponent<Asteroid>().Activate();
                }
                else
                {
                    newAsteroid = Instantiate(_asteroid, new Vector3(11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                }
                newAsteroid.transform.localScale = new Vector3(scale, scale, scale);
                newAsteroid.GetComponent<Asteroid>().SetAsWaveTrigger();
                break;
        }
    }
    
    public void CreateAsteroid(direction waveDirection)
    {
        float scale = Random.Range(_asteroidScaleMin, _asteroidScaleMax);
        GameObject newAsteroid;

        switch (waveDirection)
        {
            case direction.down:                
                if (_reservedAsteroids.Count > 0)
                {
                    newAsteroid = _reservedAsteroids[0];
                    _reservedAsteroids.RemoveAt(0);
                    newAsteroid.transform.position = new Vector3(Random.Range(-8.25f, 8.25f), 7.8f);
                    newAsteroid.GetComponent<Asteroid>().Activate();
                }
                else
                {
                   newAsteroid = Instantiate(_asteroid, new Vector3(Random.Range(-8.25f, 8.25f), 7.8f), Quaternion.identity, this.transform);
                }
                newAsteroid.transform.localScale = new Vector3(scale, scale, scale);
                break;
            case direction.up:
                if (_reservedAsteroids.Count > 0)
                {
                    newAsteroid = _reservedAsteroids[0];
                    _reservedAsteroids.RemoveAt(0);
                    newAsteroid.transform.position = new Vector3(Random.Range(-8.25f, 8.25f), -6f);
                    newAsteroid.GetComponent<Asteroid>().Activate();
                }
                else
                {
                    newAsteroid = Instantiate(_asteroid, new Vector3(Random.Range(-8.25f, 8.25f), -6f), Quaternion.identity, this.transform);
                }
                newAsteroid.transform.localScale = new Vector3(scale, scale, scale);
                break;
            case direction.right:
                if (_reservedAsteroids.Count > 0)
                {
                    newAsteroid = _reservedAsteroids[0];
                    _reservedAsteroids.RemoveAt(0);
                    newAsteroid.transform.position = new Vector3(-11.5f, Random.Range(-3.9f, 5.75f));
                    newAsteroid.GetComponent<Asteroid>().Activate();
                }
                else
                {
                    newAsteroid = Instantiate(_asteroid, new Vector3(-11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                }
                newAsteroid.transform.localScale = new Vector3(scale, scale, scale);
                break;
            case direction.left:
                if (_reservedAsteroids.Count > 0)
                {
                    newAsteroid = _reservedAsteroids[0];
                    _reservedAsteroids.RemoveAt(0);
                    newAsteroid.transform.position = new Vector3(11.5f, Random.Range(-3.9f, 5.75f));
                    newAsteroid.GetComponent<Asteroid>().Activate();
                }
                else
                {
                    newAsteroid = Instantiate(_asteroid, new Vector3(11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                }
                newAsteroid.transform.localScale = new Vector3(scale, scale, scale);
                break;
        }
    }

    public void AddToReserve(GameObject asteroid)
    {
        if (asteroid == null)
        {
            Debug.LogAssertion("Object Manager was handled an empty Asteroid reference.");
        }
        else
        {
            if (_isReserveActive)
            {
                _reservedAsteroids.Add(asteroid);
            }
            else
            {
                Destroy(asteroid);
            }
        }
    }

    public void TrimReserve()
    {
        _reservedAsteroids.TrimExcess();
    }

    public void StartReserve()
    {
        _isReserveActive = true;
    }

    public void StopReserve()
    {
        _isReserveActive = false;
    }

    public void ClearReserve()
    {
        foreach (GameObject reservedAsteroid in _reservedAsteroids)
        {
            Destroy(reservedAsteroid);
        }
        _reservedAsteroids.Clear();
    }
}