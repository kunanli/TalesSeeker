﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[SerializeField]
public class EventManager : baseSingleton<EventManager>
{

    #region define
    public enum EventStatue
    {
        None = 0,
        Load,
        Play,
        Finish
    }
    #endregion

    #region Property
    public EventStatue NowState
    {
        get { return nowState; }
    }
    #endregion

    #region field

    public Player Player;

    EventStatue nowState;
    EventStatue nextStatue;

    /// <summary>
    /// 
    /// </summary>
    int nowEvent = -1;
    #endregion

    public override void doAwake()
    {
        base.doAwake();
    }

    public override void doStart()
    {
        base.doStart();

        //temp force start
        nowState = EventStatue.Load;
    }

    public override void doUpdate()
    {
        base.doUpdate();

        switch (nowState)
        {
            case EventStatue.Load:
                //savedate
                //nothing to do, just skip

                if (nowEvent == -1)
                {
                    //start with prologue
                    nowEvent = 0;
                    if (!EventDataManager.Instance.CheckLoad(nowEvent , 0))
                    {
                        Debug.Log("Loading faild!!!!!!!!");
                    }
                }
                else
                {
                    nowState = EventStatue.Play;
                }
                break;
        }
    }

    public void Next(int eventNo, int indexNo)
    {
        var karma = Player.playerParam.karma;
        if (karma > 0 && karma < Player.PlayerParam.MaxKarma)
        {
            if (!EventDataManager.Instance.Next(eventNo, indexNo))
            {
                Debug.Log("Loading faild!!!!!!!!");
            }
            nowEvent = eventNo;
        }
        else
        {
            if (karma <= 0)
            {
                if (!EventDataManager.Instance.Next(EventDefine.BeastMode.EventNo, EventDefine.BeastMode.IndexNo))
                {
                    Debug.Log("Loading faild!!!!!!!!");
                }
                nowEvent = EventDefine.BeastMode.EventNo;
            }
            else if (karma >= Player.PlayerParam.MaxKarma)
            {
                //die
            }
        }
    }
}
