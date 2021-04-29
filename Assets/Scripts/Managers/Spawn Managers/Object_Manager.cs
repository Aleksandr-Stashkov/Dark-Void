using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject _asteroid;
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
    public void CreateAsteroidTrigger(WaveDirection direction)
    {
        float scale = Random.Range(_asteroidScaleMin, _asteroidScaleMax);

        switch (direction)
        {
            case WaveDirection.down:
                GameObject newAsteroid = Instantiate(_asteroid, new Vector3(Random.Range(-8.25f, 8.25f), 7.8f), Quaternion.identity, this.transform);
                newAsteroid.transform.localScale = new Vector3(scale, scale, scale);
                newAsteroid.GetComponent<Asteroid>().SetAsWaveTrigger();
                break;
            case WaveDirection.up:
                newAsteroid = Instantiate(_asteroid, new Vector3(Random.Range(-8.25f, 8.25f), -6f), Quaternion.identity, this.transform);
                newAsteroid.transform.localScale = new Vector3(scale, scale, scale);
                newAsteroid.GetComponent<Asteroid>().SetAsWaveTrigger();
                break;
            case WaveDirection.right:
                newAsteroid = Instantiate(_asteroid, new Vector3(-11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                newAsteroid.transform.localScale = new Vector3(scale, scale, scale);
                newAsteroid.GetComponent<Asteroid>().SetAsWaveTrigger();
                break;
            case WaveDirection.left:
                newAsteroid = Instantiate(_asteroid, new Vector3(11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                newAsteroid.transform.localScale = new Vector3(scale, scale, scale);
                newAsteroid.GetComponent<Asteroid>().SetAsWaveTrigger();
                break;
        }
    }
    
    public void CreateAsteroid(WaveDirection direction)
    {
        float scale = Random.Range(_asteroidScaleMin, _asteroidScaleMax);

        switch (direction)
        {
            case WaveDirection.down:
                GameObject newAsteroid = Instantiate(_asteroid, new Vector3(Random.Range(-8.25f, 8.25f), 7.8f), Quaternion.identity, this.transform);
                newAsteroid.transform.localScale = new Vector3(scale, scale, scale);
                break;
            case WaveDirection.up:
                newAsteroid = Instantiate(_asteroid, new Vector3(Random.Range(-8.25f, 8.25f), -6f), Quaternion.identity, this.transform);
                newAsteroid.transform.localScale = new Vector3(scale, scale, scale);
                break;
            case WaveDirection.right:
                newAsteroid = Instantiate(_asteroid, new Vector3(-11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                newAsteroid.transform.localScale = new Vector3(scale, scale, scale);
                break;
            case WaveDirection.left:
                newAsteroid = Instantiate(_asteroid, new Vector3(11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                newAsteroid.transform.localScale = new Vector3(scale, scale, scale);
                break;
        }
    }
}
