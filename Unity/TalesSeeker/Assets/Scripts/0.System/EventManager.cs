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

    public Player Player;

    EventStatue nowState;
    EventStatue nextStatue;

    /// <summary>
    /// 
    /// </summary>
    int nowEvent = -1;

    List<int> onlyOneIndexList = new List<int>();
    List<int> needIndexList = new List<int>();
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
                    //20180616
                    //for KunAn : start with 1
                    if (!EventDataManager.Instance.CheckLoad(nowEvent , 1))
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

    public void Next(int eventNo, int indexNo )
    {
        //change event, clear vitList
        if (nowEvent != eventNo)
        {
            onlyOneIndexList.Clear();
            needIndexList.Clear();
        }

        var hp = Player.playerParam.hp;
        if (hp <= 0)
        {
            Die(EventDataManager.DieType.Tpye1);
            return;
        }

        var karma = Player.playerParam.karma;
        if (karma > 0 && karma < Player.PlayerParam.MaxKarma)
        {
            if (!EventDataManager.Instance.Next(eventNo, indexNo))
            {
                Debug.Log("Loading faild!!!!!!!!");
            }
            nowEvent = eventNo;

            if(EventDataManager.Instance.getEventData(eventNo, indexNo).OnlyOneEvent)
                onlyOneIndexList.Add(indexNo);

            needIndexList.Add(indexNo);
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

                if (EventDataManager.Instance.getEventData(eventNo, indexNo).OnlyOneEvent)
                    onlyOneIndexList.Add(indexNo);

                needIndexList.Add(indexNo);
            }
            else if (karma >= Player.PlayerParam.MaxKarma)
            {
                //die
                Die(EventDataManager.DieType.Tpye2);
            }
        }
    }

    public void BattleResult()
    {
        if (!EventDataManager.Instance.Next(EventDataManager.Instance.SystemEventBattleResult[0]))
        {
            Debug.Log("Battle Result Loading faild!!!!!!!!");
        }
    }

    public void Die(EventDataManager.DieType die)
    {
        if (!EventDataManager.Instance.Next(EventDataManager.Instance.SystemEventDie[(int)die]))
        {
            Debug.Log("Die Loading faild!!!!!!!!");
        }
    }

    public bool CheckIndexOnlyOne(int indexNO)
    {
        return onlyOneIndexList.Exists(a => a == indexNO);
    }

    public bool CheckIndexNeed(int indexNO)
    {
        return needIndexList.Exists(a => a == indexNO);
    }
}
