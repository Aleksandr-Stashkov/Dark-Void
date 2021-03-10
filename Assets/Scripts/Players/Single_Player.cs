using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_Player : Player
{
    protected override void Start()
    {
        base.Start();

        //UI initial set
        _UI_Manager.Score_update(Player_kills);
        _UI_Manager.Lives_update(Player_health);
        //Player initial set
        transform.position = new Vector3(0, -6f, 0);
    }

    protected override IEnumerator Entrance_timer()
    {
        yield return StartCoroutine(base.Entrance_timer());
        _UI_Manager.Trigger_UI();
    }

    protected override void Entrance_Movement()
    {
        if (t_entrance != 0)
        {
            base.Entrance_Movement();
        }
        else
        {
            transform.position = new Vector3(0, -1f, 0);
        }
    }

    public override void Kill_count(int add)
    {
        base.Kill_count(add);
        _UI_Manager.Score_update(Player_kills);
    }

    public override void Take_Damage(int Damage)
    {
        base.Take_Damage(Damage);

        _UI_Manager.Lives_update(Player_health);
    }
}