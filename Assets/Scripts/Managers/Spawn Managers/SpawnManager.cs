using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Wave dircetion
public enum WaveDirection { down, up, right, left };

public class SpawnManager : MonoBehaviour
{
    protected Player _player;
    protected AudioSource _audio_Background;
    //Containers' managers    
    protected Transform _laserContainer;
    private PU_Manager _powerUpManager;
    private EnemyManager _enemyManager;
    private ObjectManager _objectManager;    
    //Timeline
    protected float _playerEntranceDuration; //time for player to reach its starting position
    protected float _waveStartPause = 4f;
    protected float _waveEndPause = 3f;
    protected float _waveDuration = 30f;
    private bool _isAsteroidDestroyed = false;
    //Spawn periods
    protected float _enemySpawnPeriod = 2f;
    protected float _enemySpawnPeriodDeviation = 0.3f; //percentage
    private float _PU_TripleFireSpawnPeriod = 20f;
    private float _PU_TripleFireSpawnPeriodDeviation = 0.3f; //percentage
    private float _PU_SpeedUpSpawnPeriod = 20f;
    private float _PU_SpeedUpSpawnPeriodDeviation = 0.3f; //percentage
    private float _PU_ShieldSpawnPeriod = 20f;
    private float _PU_ShieldSpawnPeriodDeviation = 0.3f; //percentage                                          

    protected virtual void Start()
    {
        FindObjects();
    }
    
    private void FindObjects()
    {
        Transform child;
        for (int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);
            switch (child.name)
            {
                case "LaserContainer":
                    _laserContainer = child.transform;
                    break;
                case "EnemyContainer":
                    _enemyManager = child.gameObject.GetComponent<EnemyManager>();
                    break;
                case "PUContainer":
                    _powerUpManager = child.gameObject.GetComponent<PU_Manager>();
                    break;
                case "ObjectContainer":
                    _objectManager = child.gameObject.GetComponent<ObjectManager>();
                    break;
                default:
                    Debug.LogWarning("There is an unrecognized child of Spawn Manager.");
                    break;
            }
        }
        _audio_Background = GetComponent<AudioSource>();

        CheckObjects();
    }

    private void CheckObjects()
    {
        if (_laserContainer == null)
        {
            Debug.LogError("Spawn Manager could not locate Laser Container.");
        }
        if (_enemyManager == null)
        {
            Debug.LogError("Spawn Manager could not locate Enemy Manager.");
        }
        if (_powerUpManager == null)
        {
            Debug.LogError("Spawn Manager could not locate Power Up Manager.");
        }
        if (_objectManager == null)
        {
            Debug.LogError("Spawn Manager could not locate Object Manager.");
        }        
        if (_audio_Background == null)
        {
            Debug.LogError("Spawn Manager could not locate Audio Manager.");
        }        
    }
    
    protected IEnumerator AsteroidTriggerWave(float waveEndTime, float startPause, float endPause, float spawnPeriod, float spawnPeriodDeviation, WaveDirection direction)
    {
        if (waveEndTime <= Time.timeSinceLevelLoad)
        {
            Debug.LogAssertion("Asteroid Trigger Wave recieved invalid wave end time and will not start.");
        }
        if (spawnPeriod < 0)
        {
            Debug.LogAssertion("Asteroid Trigger Wave recieved negative spawn period.");
        }
        if (spawnPeriodDeviation == 0)
        {
            Debug.LogWarning("You can use a shortened version of Asteroid Trigger Wave call without Period Deviation.");
        }

        yield return new WaitForSeconds(startPause);        
        while (Time.timeSinceLevelLoad <= waveEndTime && !_isAsteroidDestroyed)
        {
            _objectManager.CreateAsteroidTrigger(direction);

            float spawnPause = Random.Range((1f - spawnPeriodDeviation) * spawnPeriod, (1f + spawnPeriodDeviation) * spawnPeriod);            
            if (Time.timeSinceLevelLoad + spawnPause > waveEndTime)
            {
                yield return new WaitForSeconds(waveEndTime - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(spawnPause);
            }
        }            
        yield return new WaitForSeconds(endPause);
    }
    //Period Deviation = 0 
    protected IEnumerator AsteroidTriggerWave(float waveEndTime, float startPause, float endPause, float spawnPeriod, WaveDirection direction)
    {
        if (waveEndTime <= Time.timeSinceLevelLoad)
        {
            Debug.LogAssertion("Asteroid Trigger Wave recieved invalid wave end time and will not start.");
        }
        if (spawnPeriod < 0)
        {
            Debug.LogAssertion("Asteroid Trigger Wave recieved negative spawn period.");
        }
        if (endPause == 0)
        {
            Debug.LogWarning("You can use a shortened version of Asteroid Trigger Wave call without End Pause.");
        }

        yield return new WaitForSeconds(startPause);
        while (Time.timeSinceLevelLoad <= waveEndTime && !_isAsteroidDestroyed)
        {
            _objectManager.CreateAsteroidTrigger(direction);

            if (Time.timeSinceLevelLoad + spawnPeriod > waveEndTime)
            {
                yield return new WaitForSeconds(waveEndTime - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(spawnPeriod);
            }
        }
        yield return new WaitForSeconds(endPause);
    }
    //Period Deviation = 0; End Pause = 0
    protected IEnumerator AsteroidTriggerWave(float waveEndTime, float startPause, float spawnPeriod, WaveDirection direction)
    {
        if (waveEndTime <= Time.timeSinceLevelLoad)
        {
            Debug.LogAssertion("Asteroid Trigger Wave recieved invalid wave end time and will not start.");
        }
        if (spawnPeriod < 0)
        {
            Debug.LogAssertion("Asteroid Trigger Wave recieved negative spawn period.");
        }

        yield return new WaitForSeconds(startPause);
        while (Time.timeSinceLevelLoad <= waveEndTime && !_isAsteroidDestroyed)
        {
            _objectManager.CreateAsteroidTrigger(direction);

            if (Time.timeSinceLevelLoad + spawnPeriod > waveEndTime)
            {
                yield return new WaitForSeconds(waveEndTime - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(spawnPeriod);
            }
        }
    }
    
    protected IEnumerator AsteroidWave(float waveEndTime, float startPause, float endPause, float spawnPeriod, float spawnPeriodDeviation, WaveDirection direction)
    {
        if (waveEndTime <= Time.timeSinceLevelLoad)
        {
            Debug.LogAssertion("Asteroid Wave recieved invalid wave end time and will not start.");
        }
        if (spawnPeriod < 0)
        {
            Debug.LogAssertion("Asteroid Wave recieved negative spawn period.");
        }
        if (spawnPeriodDeviation == 0)
        {
            Debug.LogWarning("You can use a shortened version of Asteroid Wave call without Period Deviation.");
        }

        yield return new WaitForSeconds(startPause);
        while (Time.timeSinceLevelLoad <= waveEndTime)
        {
            _objectManager.CreateAsteroid(direction);

            float spawnPause = Random.Range((1f - spawnPeriodDeviation) * spawnPeriod, (1f + spawnPeriodDeviation) * spawnPeriod);            
            if (Time.timeSinceLevelLoad + spawnPause > waveEndTime)
            {
                yield return new WaitForSeconds(waveEndTime - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(spawnPause);
            }
        }
        yield return new WaitForSeconds(endPause);
    }
    //Period Deviation = 0
    protected IEnumerator AsteroidWave(float waveEndTime, float startPause, float endPause, float spawnPeriod, WaveDirection direction)
    {
        if (waveEndTime <= Time.timeSinceLevelLoad)
        {
            Debug.LogAssertion("Asteroid Wave recieved invalid wave end time and will not start.");
        }
        if (spawnPeriod < 0)
        {
            Debug.LogAssertion("Asteroid Wave recieved negative spawn period.");
        }
        if (endPause == 0)
        {
            Debug.LogWarning("You can use a shortened version of Asteroid Wave call without End Pause.");
        }

        yield return new WaitForSeconds(startPause);
        while (Time.timeSinceLevelLoad <= waveEndTime)
        {
            _objectManager.CreateAsteroid(direction);

            if (Time.timeSinceLevelLoad + spawnPeriod > waveEndTime)
            {
                yield return new WaitForSeconds(waveEndTime - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(spawnPeriod);
            }
        }
        yield return new WaitForSeconds(endPause);
    }
    //Period Deviation = 0; End Pause = 0
    protected IEnumerator AsteroidWave(float waveEndTime, float startPause, float spawnPeriod, WaveDirection direction)
    {
        if (waveEndTime <= Time.timeSinceLevelLoad)
        {
            Debug.LogAssertion("Asteroid Wave recieved invalid wave end time and will not start.");
        }
        if (spawnPeriod < 0)
        {
            Debug.LogAssertion("Asteroid Wave recieved negative spawn period.");
        }

        yield return new WaitForSeconds(startPause);
        while (Time.timeSinceLevelLoad <= waveEndTime)
        {
            _objectManager.CreateAsteroid(direction);

            if (Time.timeSinceLevelLoad + spawnPeriod > waveEndTime)
            {
                yield return new WaitForSeconds(waveEndTime - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(spawnPeriod);
            }
        }
    }
        
    protected IEnumerator EnemyWave(float waveEndTime, float startPause, float endPause, float spawnPeriod, float spawnPeriodDeviation, WaveDirection direction)
    {
        if (waveEndTime <= Time.timeSinceLevelLoad)
        {
            Debug.LogAssertion("Enemy Wave recieved invalid wave end time and will not start.");
        }
        if (spawnPeriod < 0)
        {
            Debug.LogAssertion("Enemy Wave recieved negative spawn period.");
        }
        if (spawnPeriodDeviation == 0)
        {
            Debug.LogWarning("You can use a shortened version of Enemy Wave call without Period Deviation.");
        }

        yield return new WaitForSeconds(startPause);
        while (Time.timeSinceLevelLoad <= waveEndTime)
        {
            _enemyManager.CreateEnemy(direction);

            float spawnPause = Random.Range((1f - spawnPeriodDeviation) * spawnPeriod, (1f + spawnPeriodDeviation) * spawnPeriod);            
            if (Time.timeSinceLevelLoad + spawnPause > waveEndTime)
            {
                yield return new WaitForSeconds(waveEndTime - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(spawnPause);
            }
        }
        yield return new WaitForSeconds(endPause);
    }
    //Period Deviation = 0
    protected IEnumerator EnemyWave(float waveEndTime, float startPause, float endPause, float spawnPeriod, WaveDirection direction)
    {
        if (waveEndTime <= Time.timeSinceLevelLoad)
        {
            Debug.LogAssertion("Enemy Wave recieved invalid wave end time and will not start.");
        }
        if (spawnPeriod < 0)
        {
            Debug.LogAssertion("Enemy Wave recieved negative spawn period.");
        }
        if (endPause == 0)
        {
            Debug.LogWarning("You can use a shortened version of Enemy Wave call without EndPause.");
        }

        yield return new WaitForSeconds(startPause);
        while (Time.timeSinceLevelLoad <= waveEndTime)
        {
            _enemyManager.CreateEnemy(direction);

            if (Time.timeSinceLevelLoad + spawnPeriod > waveEndTime)
            {
                yield return new WaitForSeconds(waveEndTime - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(spawnPeriod);
            }
        }
        yield return new WaitForSeconds(endPause);
    }
    //Period Deviation = 0; End Pause = 0
    protected IEnumerator EnemyWave(float waveEndTime, float startPause, float spawnPeriod, WaveDirection direction)
    {
        if (waveEndTime <= Time.timeSinceLevelLoad)
        {
            Debug.LogAssertion("Enemy Wave recieved invalid wave end time and will not start.");
        }
        if (spawnPeriod < 0)
        {
            Debug.LogAssertion("Enemy Wave recieved negative spawn period.");
        }        

        yield return new WaitForSeconds(startPause);
        while (Time.timeSinceLevelLoad <= waveEndTime)
        {
            _enemyManager.CreateEnemy(direction);

            if (Time.timeSinceLevelLoad + spawnPeriod > waveEndTime)
            {
                yield return new WaitForSeconds(waveEndTime - Time.timeSinceLevelLoad);
            }
            else
            {
                yield return new WaitForSeconds(spawnPeriod);
            }
        }
    }

    protected IEnumerator PU_TripleFireWave(float waveEndTime)
    {
        while (Time.timeSinceLevelLoad < waveEndTime)
        {
            _powerUpManager.Create_PU_TripleFire();
            yield return new WaitForSeconds(Random.Range((1 - _PU_TripleFireSpawnPeriodDeviation) * _PU_TripleFireSpawnPeriod, (1 + _PU_TripleFireSpawnPeriodDeviation) * _PU_TripleFireSpawnPeriod));
        }
    }

    protected IEnumerator PU_SpeedUpWave(float waveEndTime)
    {
        while (Time.timeSinceLevelLoad < waveEndTime)
        {
            _powerUpManager.Create_PU_SpeedUp();
            yield return new WaitForSeconds(Random.Range((1 - _PU_SpeedUpSpawnPeriodDeviation) * _PU_SpeedUpSpawnPeriod, (1 + _PU_SpeedUpSpawnPeriodDeviation) * _PU_SpeedUpSpawnPeriod));
        }
    }

    protected IEnumerator PU_ShieldWave(float waveEndTime)
    {
        while (Time.timeSinceLevelLoad < waveEndTime)
        {
            _powerUpManager.Create_PU_Shield();
            yield return new WaitForSeconds(Random.Range((1 - _PU_ShieldSpawnPeriodDeviation) * _PU_ShieldSpawnPeriod, (1 + _PU_ShieldSpawnPeriodDeviation) * _PU_ShieldSpawnPeriod));
        }
    }

    protected virtual IEnumerator MainTimeline()
    {
        yield break;
    }
    //Asteroid destruction triggers the wave
    public void TriggerWave()
    {
        if (!_isAsteroidDestroyed)
        {
            _isAsteroidDestroyed = true;
            StartCoroutine(MainTimeline());
        }
    }

    public Transform LaserContainer() { return _laserContainer; }
}