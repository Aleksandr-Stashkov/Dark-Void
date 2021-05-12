using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;

    void Start()
    {
        if (_enemy == null)
        {
            Debug.LogError("Enemy Manager could not locate PREFAB for Enemy.");
        }
    }
    
    public void CreateEnemy(direction waveDirection, bool isTurningBack)
    {
        switch (waveDirection)
        {
            case direction.down:
                GameObject newEnemy = Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), 7.8f), Quaternion.identity, this.transform);
                newEnemy.GetComponent<Enemy>().SetTurningBack(isTurningBack);
                break;
            case direction.up:
                newEnemy = Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), -6f), Quaternion.identity, this.transform);
                newEnemy.GetComponent<Enemy>().SetTurningBack(isTurningBack);
                break;
            case direction.right:
                newEnemy = Instantiate(_enemy, new Vector3(-11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                newEnemy.GetComponent<Enemy>().SetTurningBack(isTurningBack);
                break;
            case direction.left:
                newEnemy = Instantiate(_enemy, new Vector3(11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
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
                Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), 7.8f), Quaternion.identity, this.transform);
                break;
            case direction.up:
                Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), -6f), Quaternion.identity, this.transform);
                break;
            case direction.right:
                Instantiate(_enemy, new Vector3(-11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                break;
            case direction.left:
                Instantiate(_enemy, new Vector3(11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                break;

        }
    }
}
