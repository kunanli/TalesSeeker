using System.Collections;
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
                    if (!EventDataManager.Instance.CheckLoad(nowEvent))
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
}
