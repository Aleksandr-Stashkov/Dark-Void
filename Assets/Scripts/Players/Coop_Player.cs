using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coop_Player : Player
{
    [SerializeField]
    private bool _isPlayer1 = false;
    
    protected override void InitialSetting()
    {
        base.InitialSetting();

        _isRotationOff = true;

        if (_isPlayer1)
        {
            _UI_Manager.UpdateScore1(_score);
            _UI_Manager.UpdateLives1(_lives);
            
            transform.position = new Vector3(-5f, -6f, 0);
        }
        else
        {
            _UI_Manager.UpdateScore2(_score);
            _UI_Manager.UpdateLives2(_lives);
           
            transform.position = new Vector3(5f, -6f, 0);
        }
    }

    protected override void Update()
    {
        if (_isUserControlled && !Game_Manager.isPaused)
        {
            if ((Input.GetKeyDown(KeyCode.Space) && _isPlayer1 && _isFireEnabled) || (Input.GetMouseButtonDown(0) && !_isPlayer1 && _isFireEnabled))
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

    protected override float GetHorizontal()
    {
        if (_isPlayer1)
        {
            return Input.GetAxis("Horizontal_Player1");
        }
        else
        {
            return Input.GetAxis("Horizontal_Player2");
        }
    }
    protected override float GetVertical()
    {
        if (_isPlayer1)
        {
            return Input.GetAxis("Vertical_Player1");
        }
        else
        {
            return Input.GetAxis("Vertical_Player2");
        }
    }

    protected override IEnumerator EntranceTimer()
    {
        yield return StartCoroutine(base.EntranceTimer());
        if (_isPlayer1)
        {
            _UI_Manager.Trigger_UI();
        }
    }

    protected override void EntranceMovement()
    {
        if (_playerEntranceDuration != 0)
        {
            base.EntranceMovement();
        }
        else
        {
            if (_isPlayer1)
            {
                transform.position = new Vector3(-5f, -1f, 0);
            }
            else
            {
                transform.position = new Vector3(5f, -1f, 0);
            }
        }
    }    

    public override void AddScore(int add)
    {
        base.AddScore(add);
        if (_isPlayer1)
        {
            _UI_Manager.UpdateScore1(_score);
        }
        else
        {
            _UI_Manager.UpdateScore2(_score);
        }
    }

    public override void TakeDamage(int Damage)
    {
        base.TakeDamage(Damage);

        if (_isPlayer1)
        {
            _UI_Manager.UpdateLives1(_lives);
        }
        else
        {
            _UI_Manager.UpdateLives2(_lives);
        }
    }

    public override bool IsPlayer1()
    {
        return _isPlayer1;
    }
}
