using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MovingObject
{
    private PU_Manager _PU_Manager;
    private bool _isChildOfManager = true;

    protected override void Start()
    {
        _PU_Manager = transform.parent.GetComponent<PU_Manager>();
        if (_PU_Manager == null)
        {
            Debug.LogError("Power Up could not locate its Manager as a parent.");
            _isChildOfManager = false;
        }

        base.Start();
    }

    protected override void Dispose()
    {
        if (_isChildOfManager)
        {
            gameObject.SetActive(false);
            _PU_Manager.AddToReserve(gameObject);
        }
        else
        {
            base.Dispose();
        }
    }

    public void DisposeOf_PU()
    {
        Dispose();
    }
}
