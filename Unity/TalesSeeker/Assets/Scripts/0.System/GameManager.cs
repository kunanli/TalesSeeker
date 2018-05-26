using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : baseSingleton<GameManager> {

    public Player MainPlayer;

    public override void doStart()
    {
        base.doStart();

        MainPlayer = GameObject.Find("Player").GetComponent<Player>();
    }

    public override void doUpdate()
    {
        base.doUpdate();
    }
}
