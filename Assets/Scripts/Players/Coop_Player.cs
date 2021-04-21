using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coop_Player : Player
{
    [SerializeField]
    private bool Player1 = false;

    protected override void Start()
    {
        base.Start();

        rotation_fix = true;

        if (Player1)
        {
            //UI initial set
            _UI_Manager.UpdateScore1(Player_kills);
            _UI_Manager.UpdateLives1(Player_health);
            //Player 1 initial set
            transform.position = new Vector3(-5f, -6f, 0);                     
        }
        else
        {
            //UI initial set
            _UI_Manager.UpdateScore2(Player_kills);
            _UI_Manager.UpdateLives2(Player_health);
            //Player 2 initial set
            transform.position = new Vector3(5f, -6f, 0);            
        }
    }

    protected override void Update()
    {
        if (User_Control && !Game_Manager.isPaused)
        {
            if ((Input.GetKeyDown(KeyCode.Space) && Player1 && Fire_enabled) || (Input.GetMouseButtonDown(0) && !Player1 && Fire_enabled))
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
        if (Player1)
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
        if (Player1)
        {
            return Input.GetAxis("Vertical_Player1");
        }
        else
        {
            return Input.GetAxis("Vertical_Player2");
        }
    }

    protected override IEnumerator Entrance_timer()
    {
        yield return StartCoroutine(base.Entrance_timer());
        if (Player1)
        {
            _UI_Manager.Trigger_UI();
        }
    }

    protected override void Entrance_Movement()
    {
        if (t_entrance != 0)
        {
            base.Entrance_Movement();
        }
        else
        {
            if (Player1)
            {
                transform.position = new Vector3(-5f, -1f, 0);
            }
            else
            {
                transform.position = new Vector3(5f, -1f, 0);
            }
        }
    }    

    public override void Kill_count(int add)
    {
        base.Kill_count(add);
        if (Player1)
        {
            _UI_Manager.UpdateScore1(Player_kills);
        }
        else
        {
            _UI_Manager.UpdateScore2(Player_kills);
        }
    }

    public override void Take_Damage(int Damage)
    {
        base.Take_Damage(Damage);

        if (Player1)
        {
            _UI_Manager.UpdateLives1(Player_health);
        }
        else
        {
            _UI_Manager.UpdateLives2(Player_health);
        }
    }

    public override bool Is_Player1()
    {
        return Player1;
    }
}
