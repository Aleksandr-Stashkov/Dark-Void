using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MovingObject
{
    private PU_Manager _PU_Manager;
    private bool _isChildOfManager = true;

    protected override void Start()
    {
        Transform PU_Container = transform.parent;
        if (PU_Container == null)
        {
            Debug.LogError("Power Up could not locate its parent.");
            _isChildOfManager = false;
        }
        else
        {
            _PU_Manager = PU_Container.GetComponent<PU_Manager>();
            if (_PU_Manager == null)
            {
                Debug.LogError("Power Up could not locate its Manager on the parent.");
                _isChildOfManager = false;
            }
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

    public void DisposeOfSelf()
    {
        Dispose();
    }
}
