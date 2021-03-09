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
        transform.rotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(Entrance());
    }
}
