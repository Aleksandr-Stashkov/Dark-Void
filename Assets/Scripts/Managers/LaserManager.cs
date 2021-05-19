using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _laser;
    [SerializeField]
    private GameObject _enemyLaser;
    [SerializeField]
    private GameObject _tripleLaser;
    private List<GameObject> _reservedLasers = new List<GameObject>();
    private List<GameObject> _reservedEnemyLasers = new List<GameObject>();
    
    void Start()
    {
        if (_laser == null)
        {
            Debug.LogError("Laser Manager could not locate PREFAB for Laser.");
        }
        if (_enemyLaser == null)
        {
            Debug.LogError("Laser Manager could not locate PREFAB for Enemy Laser.");
        }
        if (_tripleLaser == null)
        {
            Debug.LogError("Laser Manager could not locate PREFAB for Triple Laser.");
        }
    }

    public void CreateLaser(Player player)
    {
        GameObject newLaser;
        if (_reservedLasers.Count > 0)
        {
            newLaser = _reservedLasers[0];
            _reservedLasers.RemoveAt(0);
            newLaser.transform.position = player.transform.position + player.transform.up * 0.8f;
            newLaser.transform.rotation = player.transform.rotation;
            newLaser.gameObject.SetActive(true);
        }
        else
        {
            newLaser = Instantiate(_laser, player.transform.position + player.transform.up * 0.8f, player.transform.rotation, this.transform);
        }

        newLaser.GetComponent<Laser>().SetPlayer(player);
    }

    public void CreateTripleLaser(Player player)
    {
        GameObject newTripleFire = Instantiate(_tripleLaser, player.transform.position, player.transform.rotation, this.transform);
        Laser[] lasers = newTripleFire.GetComponentsInChildren<Laser>();
        foreach (Laser laser in lasers)
        {
            laser.SetPlayer(player);
            laser.transform.parent = this.transform;
            laser.DisableReserve();
        }
        Destroy(newTripleFire);

        if(_reservedLasers.Count > 3)
        {
            _reservedLasers.RemoveRange(0, 3);
        }
    }

    public void CreateEnemyLaser(Transform enemy)
    {
        if (Random.value < 0.5f)
        {
            if (_reservedEnemyLasers.Count > 0)
            {
                GameObject newLaser = _reservedEnemyLasers[0];
                _reservedEnemyLasers.RemoveAt(0);
                newLaser.transform.position = enemy.position - enemy.up * 0.706f + enemy.right * 0.09f;
                newLaser.transform.rotation = enemy.rotation * Quaternion.FromToRotation(enemy.up, -enemy.up);
                newLaser.SetActive(true);
            }
            else
            {
                Instantiate(_enemyLaser, enemy.position - enemy.up * 0.706f + enemy.right * 0.09f, enemy.rotation * Quaternion.FromToRotation(enemy.up, -enemy.up), this.transform);
            }
        }
        else
        {
            if (_reservedEnemyLasers.Count > 0)
            {
                GameObject newLaser = _reservedEnemyLasers[0];
                _reservedEnemyLasers.RemoveAt(0);
                newLaser.transform.position = enemy.position - enemy.up * 0.706f - enemy.right * 0.09f;
                newLaser.transform.rotation = enemy.rotation * Quaternion.FromToRotation(enemy.up, -enemy.up);
                newLaser.SetActive(true);
            }
            else
            {
                Instantiate(_enemyLaser, enemy.position - enemy.up * 0.706f - enemy.right * 0.09f, enemy.rotation * Quaternion.FromToRotation(enemy.up, -enemy.up), this.transform);
            }
        }
    }

    public void AddLaserToReserve(GameObject laser)
    {
        if (laser.CompareTag("Fire"))
        {
            _reservedLasers.Add(laser);
        }
        else if (laser.CompareTag("Fire_enemy"))
        {
            _reservedEnemyLasers.Add(laser);
        }
    }

    public void ClearReserves()
    {
        foreach (GameObject laser in _reservedLasers)
        {
            Destroy(laser);
        }
        _reservedLasers.Clear();

        foreach (GameObject laser in _reservedEnemyLasers)
        {
            Destroy(laser);
        }
        _reservedEnemyLasers.Clear();
    }
}
