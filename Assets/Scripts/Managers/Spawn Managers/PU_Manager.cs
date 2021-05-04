using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject _PU_TripleFire;
    [SerializeField]
    private GameObject _PU_SpeedUp;
    [SerializeField]
    private GameObject _PU_Shield;    

    private void Start()
    {
        if (_PU_TripleFire == null)
        {
            Debug.LogError("PU Manager could not locate PREFAB for Triple Fire Power Up.");
        }
        if (_PU_SpeedUp == null)
        {
            Debug.LogError("PU Manager could not locate PREFAB for Speed Up Power Up.");
        }
        if (_PU_Shield == null)
        {
            Debug.LogError("PU Manager could not locate PREFAB for Shield Power Up.");
        }
    }

    public void Create_PU_TripleFire()
    {
        GameObject new_PU = Instantiate(_PU_TripleFire, new Vector3(Random.Range(-9.15f, 9.15f), 10f, 0), Quaternion.identity, this.transform);
        new_PU.GetComponent<MovingObject>().SetSpeed(3f);
    }

    public void Create_PU_SpeedUp()
    {
        GameObject new_PU = Instantiate(_PU_SpeedUp, new Vector3(Random.Range(-9.15f, 9.15f), 10f, 0), Quaternion.identity, this.transform);
        new_PU.GetComponent<MovingObject>().SetSpeed(3f);
    }

    public void Create_PU_Shield()
    {
        GameObject new_PU = Instantiate(_PU_Shield, new Vector3(Random.Range(-9.15f, 9.15f), 10f, 0), Quaternion.identity, this.transform);
        new_PU.GetComponent<MovingObject>().SetSpeed(3f);
    }
}
