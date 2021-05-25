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
    private List<GameObject> _reservedTripleFire = new List<GameObject>();
    private List<GameObject> _reservedSpeedUp = new List<GameObject>();
    private List<GameObject> _reservedShield = new List<GameObject>();

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

    public void Create_PU_TripleFire(direction waveDirection)
    {
        GameObject new_PU;

        switch (waveDirection)
        {
            case direction.down:
                if (_reservedTripleFire.Count > 0)
                {
                    new_PU = _reservedTripleFire[0];
                    _reservedTripleFire.RemoveAt(0);
                    new_PU.transform.position = new Vector3(Random.Range(-9.15f, 9.15f), 9f, 0);
                    new_PU.GetComponent<PowerUp>().Activate();
                }
                else
                {
                    new_PU = Instantiate(_PU_TripleFire, new Vector3(Random.Range(-9.15f, 9.15f), 9f, 0), Quaternion.identity, this.transform);
                }
                new_PU.GetComponent<MovingObject>().SetSpeed(3f);
                break;
            case direction.up:
                if (_reservedTripleFire.Count > 0)
                {
                    new_PU = _reservedTripleFire[0];
                    _reservedTripleFire.RemoveAt(0);
                    new_PU.transform.position = new Vector3(Random.Range(-9.15f, 9.15f), -6f, 0);
                    new_PU.GetComponent<PowerUp>().Activate();
                }
                else
                {
                    new_PU = Instantiate(_PU_TripleFire, new Vector3(Random.Range(-9.15f, 9.15f), -6f, 0), Quaternion.identity, this.transform);
                }
                new_PU.GetComponent<MovingObject>().SetSpeed(3f);
                break;
            case direction.right:
                if (_reservedTripleFire.Count > 0)
                {
                    new_PU = _reservedTripleFire[0];
                    _reservedTripleFire.RemoveAt(0);
                    new_PU.transform.position = new Vector3(-12f, Random.Range(-3.85f, 5.90f), 0);
                    new_PU.GetComponent<PowerUp>().Activate();
                }
                else
                {
                    new_PU = Instantiate(_PU_TripleFire, new Vector3(-12f, Random.Range(-3.85f, 5.90f), 0), Quaternion.identity, this.transform);
                }
                new_PU.GetComponent<MovingObject>().SetSpeed(3f);
                break;
            case direction.left:
                if (_reservedTripleFire.Count > 0)
                {
                    new_PU = _reservedTripleFire[0];
                    _reservedTripleFire.RemoveAt(0);
                    new_PU.transform.position = new Vector3(12f, Random.Range(-3.85f, 5.90f), 0);
                    new_PU.GetComponent<PowerUp>().Activate();
                }
                else
                {
                    new_PU = Instantiate(_PU_TripleFire, new Vector3(12f, Random.Range(-3.85f, 5.90f), 0), Quaternion.identity, this.transform);
                }
                new_PU.GetComponent<MovingObject>().SetSpeed(3f);
                break;
        }        
    }

    public void Create_PU_SpeedUp(direction waveDirection)
    {
        GameObject new_PU;

        switch (waveDirection)
        {
            case direction.down:
                if (_reservedSpeedUp.Count > 0)
                {
                    new_PU = _reservedSpeedUp[0];
                    _reservedSpeedUp.RemoveAt(0);
                    new_PU.transform.position = new Vector3(Random.Range(-9.15f, 9.15f), 9f, 0);
                    new_PU.GetComponent<PowerUp>().Activate();
                }
                else
                {
                    new_PU = Instantiate(_PU_SpeedUp, new Vector3(Random.Range(-9.15f, 9.15f), 9f, 0), Quaternion.identity, this.transform);
                }
                new_PU.GetComponent<MovingObject>().SetSpeed(3f);
                break;
            case direction.up:
                if (_reservedSpeedUp.Count > 0)
                {
                    new_PU = _reservedSpeedUp[0];
                    _reservedSpeedUp.RemoveAt(0);
                    new_PU.transform.position = new Vector3(Random.Range(-9.15f, 9.15f), -6f, 0);
                    new_PU.GetComponent<PowerUp>().Activate();
                }
                else
                {
                    new_PU = Instantiate(_PU_SpeedUp, new Vector3(Random.Range(-9.15f, 9.15f), -6f, 0), Quaternion.identity, this.transform);
                }
                new_PU.GetComponent<MovingObject>().SetSpeed(3f);
                break;
            case direction.right:
                if (_reservedSpeedUp.Count > 0)
                {
                    new_PU = _reservedSpeedUp[0];
                    _reservedSpeedUp.RemoveAt(0);
                    new_PU.transform.position = new Vector3(-12f, Random.Range(-3.85f, 5.90f), 0);
                    new_PU.GetComponent<PowerUp>().Activate();
                }
                else
                {
                    new_PU = Instantiate(_PU_SpeedUp, new Vector3(-12f, Random.Range(-3.85f, 5.90f), 0), Quaternion.identity, this.transform);
                }
                new_PU.GetComponent<MovingObject>().SetSpeed(3f);
                break;
            case direction.left:
                if (_reservedSpeedUp.Count > 0)
                {
                    new_PU = _reservedSpeedUp[0];
                    _reservedSpeedUp.RemoveAt(0);
                    new_PU.transform.position = new Vector3(12f, Random.Range(-3.85f, 5.90f), 0);
                    new_PU.GetComponent<PowerUp>().Activate();
                }
                else
                {
                    new_PU = Instantiate(_PU_SpeedUp, new Vector3(12f, Random.Range(-3.85f, 5.90f), 0), Quaternion.identity, this.transform);
                }
                new_PU.GetComponent<MovingObject>().SetSpeed(3f);
                break;
        }
    }

    public void Create_PU_Shield(direction waveDirection)
    {
        GameObject new_PU;

        switch (waveDirection)
        {
            case direction.down:
                if (_reservedShield.Count > 0)
                {
                    new_PU = _reservedShield[0];
                    _reservedShield.RemoveAt(0);
                    new_PU.transform.position = new Vector3(Random.Range(-9.15f, 9.15f), 9f, 0);
                    new_PU.transform.rotation = Quaternion.identity;
                    new_PU.GetComponent<PowerUp>().Activate();
                }
                else
                {
                    new_PU = Instantiate(_PU_Shield, new Vector3(Random.Range(-9.15f, 9.15f), 9f, 0), Quaternion.identity, this.transform);
                }
                new_PU.GetComponent<MovingObject>().SetSpeed(3f);
                break;
            case direction.up:
                if (_reservedShield.Count > 0)
                {
                    new_PU = _reservedShield[0];
                    _reservedShield.RemoveAt(0);
                    new_PU.transform.position = new Vector3(Random.Range(-9.15f, 9.15f), -6f, 0);
                    new_PU.transform.rotation = Quaternion.identity;
                    new_PU.GetComponent<PowerUp>().Activate();
                }
                else
                {
                    new_PU = Instantiate(_PU_Shield, new Vector3(Random.Range(-9.15f, 9.15f), -6f, 0), Quaternion.identity, this.transform);
                }
                new_PU.GetComponent<MovingObject>().SetSpeed(3f);
                break;
            case direction.right:
                if (_reservedShield.Count > 0)
                {
                    new_PU = _reservedShield[0];
                    _reservedShield.RemoveAt(0);
                    new_PU.transform.position = new Vector3(-12f, Random.Range(-3.85f, 5.90f), 0);
                    new_PU.transform.rotation = Quaternion.identity;
                    new_PU.GetComponent<PowerUp>().Activate();
                }
                else
                {
                    new_PU = Instantiate(_PU_Shield, new Vector3(-12f, Random.Range(-3.85f, 5.90f), 0), Quaternion.identity, this.transform);
                }
                new_PU.GetComponent<MovingObject>().SetSpeed(3f);
                break;
            case direction.left:
                if (_reservedShield.Count > 0)
                {
                    new_PU = _reservedShield[0];
                    _reservedShield.RemoveAt(0);
                    new_PU.transform.position = new Vector3(12f, Random.Range(-3.85f, 5.90f), 0);
                    new_PU.transform.rotation = Quaternion.identity;
                    new_PU.GetComponent<PowerUp>().Activate();
                }
                else
                {
                    new_PU = Instantiate(_PU_Shield, new Vector3(12f, Random.Range(-3.85f, 5.90f), 0), Quaternion.identity, this.transform);
                }
                new_PU.GetComponent<MovingObject>().SetSpeed(3f);
                break;
        }
    }

    public void AddToReserve(GameObject PowerUp)
    {
        if (PowerUp == null)
        {
            Debug.LogAssertion("Power Up Manager was handled an empty Power Up reference.");
        }
        else
        {
            if (PowerUp.CompareTag("PU_Shield"))
            {
                _reservedShield.Add(PowerUp);
            }
            else if (PowerUp.CompareTag("PU_SpeedUp"))
            {
                _reservedSpeedUp.Add(PowerUp);
            }
            else if (PowerUp.CompareTag("PU_TripleFire"))
            {
                _reservedTripleFire.Add(PowerUp);
            }
            else
            {
                Debug.LogAssertion("Name of a PU added to reserve is unrecognized.");
            }
        }
    }

    public void TrimReserve()
    {
        _reservedTripleFire.TrimExcess();
        _reservedSpeedUp.TrimExcess();
        _reservedShield.TrimExcess();
    }

    public void ClearReserve()
    {
        foreach (GameObject PU_Shield in _reservedShield)
        {
            Destroy(PU_Shield);
        }
        _reservedShield.Clear();

        foreach (GameObject PU_SpeedUp in _reservedSpeedUp)
        {
            Destroy(PU_SpeedUp);
        }
        _reservedSpeedUp.Clear();

        foreach (GameObject PU_TripleFire in _reservedTripleFire)
        {
            Destroy(PU_TripleFire);
        }
        _reservedTripleFire.Clear();
    }
}
