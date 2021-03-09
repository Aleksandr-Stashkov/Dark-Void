using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coop_Player : Player
{
    [SerializeField]
    private bool Player1= false;

    protected override void Start()
    {
        base.Start();

        if (Player1)
        {
            //UI initial set
            _UI_Manager.Score_update(Player_kills);
            _UI_Manager.Lives_update(Player_health);
            //Player initial set
            transform.position = new Vector3(-5f, -6f, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            StartCoroutine(Entrance());
        }
        else
        {
            //UI initial set
            _UI_Manager.Score_update_2(Player_kills);
            _UI_Manager.Lives_update_2(Player_health);
            //Player initial set
            transform.position = new Vector3(5f, -6f, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
