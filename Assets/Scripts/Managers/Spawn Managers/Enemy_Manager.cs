using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
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
    
    public void CreateEnemy(WaveDirection direction, bool isTurningBack)
    {
        switch (direction)
        {
            case WaveDirection.down:
                GameObject newEnemy = Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), 7.8f), Quaternion.identity, this.transform);
                newEnemy.GetComponent<Enemy>().SetReturn(isTurningBack);
                break;
            case WaveDirection.up:
                newEnemy = Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), -6f), Quaternion.identity, this.transform);
                newEnemy.GetComponent<Enemy>().SetReturn(isTurningBack);
                break;
            case WaveDirection.right:
                newEnemy = Instantiate(_enemy, new Vector3(-11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                newEnemy.GetComponent<Enemy>().SetReturn(isTurningBack);
                break;
            case WaveDirection.left:
                newEnemy = Instantiate(_enemy, new Vector3(11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                newEnemy.GetComponent<Enemy>().SetReturn(isTurningBack);
                break;
        }
    }
    //with isTurningBack = false
    public void CreateEnemy(WaveDirection direction)
    {
        switch (direction)
        {
            case WaveDirection.down:
                Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), 7.8f), Quaternion.identity, this.transform);
                break;
            case WaveDirection.up:
                Instantiate(_enemy, new Vector3(Random.Range(-8.25f, 8.25f), -6f), Quaternion.identity, this.transform);
                break;
            case WaveDirection.right:
                Instantiate(_enemy, new Vector3(-11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                break;
            case WaveDirection.left:
                Instantiate(_enemy, new Vector3(11.5f, Random.Range(-3.9f, 5.75f)), Quaternion.identity, this.transform);
                break;

        }
    }
}
