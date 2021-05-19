using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    private List<GameObject> _reservedEnemies = new List<GameObject>();

    void Start()
    {
        if (_enemy == null)
        {
            Debug.LogError("Enemy Manager could not locate PREFAB for Enemy.");
        }
    }
    
    public void CreateEnemy(direction waveDirection, bool isTurningBack)
    {
        GameObject newEnemy;

        switch (waveDirection)
        {
            case direction.down:
                if (_reservedEnemies.Count > 0) 
                {
                    newEnemy = _reservedEnemies[0];
                    _reservedEnemies.RemoveAt(0);
                    newEnemy.transform.position = new Vector3(Random.Range(-8.25f, 8.25f), 7.8f);
                    newEnemy.transform.rotation = Quaternion.identity;
                    newEnemy.GetComponent<Enemy>().Activate();
                }
                else
                {
                    newEnemy = Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), 7.8f), Quaternion.identity, this.transform);
                }
                newEnemy.GetComponent<Enemy>().SetTurningBack(isTurningBack);
                break;
            case direction.up:
                if (_reservedEnemies.Count > 0)
                {
                    newEnemy = _reservedEnemies[0];
                    _reservedEnemies.RemoveAt(0);
                    newEnemy.transform.position = new Vector3(Random.Range(-8.25f, 8.25f), -6f);
                    newEnemy.transform.rotation = Quaternion.identity;
                    newEnemy.GetComponent<Enemy>().Activate();
                }
                else
                {
                    newEnemy = Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), -6f), Quaternion.identity, this.transform);
                }
                newEnemy.GetComponent<Enemy>().SetTurningBack(isTurningBack);
                break;
            case direction.right:
                if (_reservedEnemies.Count > 0)
                {
                    newEnemy = _reservedEnemies[0];
                    _reservedEnemies.RemoveAt(0);
                    newEnemy.transform.position = new Vector3(-11.5f, Random.Range(-3.9f, 5.75f));
                    newEnemy.transform.rotation = Quaternion.identity;
                    newEnemy.GetComponent<Enemy>().Activate();
                }
                else
                {
                    newEnemy = Instantiate(_enemy, new Vector3(-11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                }
                newEnemy.GetComponent<Enemy>().SetTurningBack(isTurningBack);
                break;
            case direction.left:
                if (_reservedEnemies.Count > 0)
                {
                    newEnemy = _reservedEnemies[0];
                    _reservedEnemies.RemoveAt(0);
                    newEnemy.transform.position = new Vector3(11.5f, Random.Range(-3.9f, 5.75f));
                    newEnemy.transform.rotation = Quaternion.identity;
                    newEnemy.GetComponent<Enemy>().Activate();
                }
                else
                {
                    newEnemy = Instantiate(_enemy, new Vector3(11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                }
                newEnemy.GetComponent<Enemy>().SetTurningBack(isTurningBack);
                break;
        }
    }
    //with isTurningBack = false
    public void CreateEnemy(direction waveDirection)
    {
        switch (waveDirection)
        {
            case direction.down:
                if (_reservedEnemies.Count > 0)
                {
                    GameObject newEnemy = _reservedEnemies[0];
                    _reservedEnemies.RemoveAt(0);
                    newEnemy.transform.position = new Vector3(Random.Range(-8.25f, 8.25f), 7.8f);
                    newEnemy.transform.rotation = Quaternion.identity;
                    newEnemy.GetComponent<Enemy>().Activate();
                }
                else
                {
                    Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), 7.8f), Quaternion.identity, this.transform);
                }
                break;
            case direction.up:
                if (_reservedEnemies.Count > 0)
                {
                    GameObject newEnemy = _reservedEnemies[0];
                    _reservedEnemies.RemoveAt(0);
                    newEnemy.transform.position = new Vector3(Random.Range(-8.25f, 8.25f), -6f);
                    newEnemy.transform.rotation = Quaternion.identity;
                    newEnemy.GetComponent<Enemy>().Activate();
                }
                else
                {
                    Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), -6f), Quaternion.identity, this.transform);
                }
                break;
            case direction.right:
                if (_reservedEnemies.Count > 0)
                {
                    GameObject newEnemy = _reservedEnemies[0];
                    _reservedEnemies.RemoveAt(0);
                    newEnemy.transform.position = new Vector3(-11.5f, Random.Range(-3.9f, 5.75f));
                    newEnemy.transform.rotation = Quaternion.identity;
                    newEnemy.GetComponent<Enemy>().Activate();
                }
                else
                {
                    Instantiate(_enemy, new Vector3(-11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                }
                break;
            case direction.left:
                if (_reservedEnemies.Count > 0)
                {
                    GameObject newEnemy = _reservedEnemies[0];
                    _reservedEnemies.RemoveAt(0);
                    newEnemy.transform.position = new Vector3(11.5f, Random.Range(-3.9f, 5.75f));
                    newEnemy.transform.rotation = Quaternion.identity;
                    newEnemy.GetComponent<Enemy>().Activate();
                }
                else
                {
                    Instantiate(_enemy, new Vector3(11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                }
                break;
        }
    }

    public void AddToReserve(GameObject enemy)
    {
        _reservedEnemies.Add(enemy);        
    }

    public void TrimReserve()
    {
        _reservedEnemies.TrimExcess();
    }

    public void ClearReserve()
    {
        foreach (GameObject enemy in _reservedEnemies)
        {
            Destroy(enemy);
        }
        _reservedEnemies.Clear();
    }
}
