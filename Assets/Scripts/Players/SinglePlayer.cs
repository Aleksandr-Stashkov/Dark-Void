using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayer : Player
{
    protected override void InitialSetting()
    {
        base.InitialSetting();

        _UI_Manager.UpdateScore1(_score);
        _UI_Manager.UpdateLives1(_lives);
        
        transform.position = new Vector3(0, -6f, 0);
    }

    protected override void Update()
    {
        if (_isUserControlled && !GameManager.isPaused)
        {
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && _isFireEnabled)
            {
                Fire();
            }

            Movement();
        }
        else
        {
            base.Update();
        }
    }

    protected override void Movement()
    {

        base.Movement();

        if (!_isRotationOff)
        {
            RotationToMouse();
        }
    }

    protected override float GetHorizontal()
    {
        return Input.GetAxis("Horizontal_Single");
    }
    protected override float GetVertical()
    {
        return Input.GetAxis("Vertical_Single");
    }

    protected override IEnumerator EntranceTimer()
    {
        yield return StartCoroutine(base.EntranceTimer());
        _UI_Manager.Trigger_UI();
    }

    protected override void EntranceMovement()
    {
        if (_playerEntranceDuration != 0)
        {
            base.EntranceMovement();
        }
        else
        {
            transform.position = new Vector3(0, -1f, 0);
        }
    }

    public override void AddScore(int add)
    {
        base.AddScore(add);
        _UI_Manager.UpdateScore1(_score);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        _UI_Manager.UpdateLives1(_lives);
    }
}
