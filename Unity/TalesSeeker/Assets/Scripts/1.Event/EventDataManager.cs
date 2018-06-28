using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDataManager : baseSingleton<EventDataManager> {

    #region
    public enum  DieType
    {
        /// <summary>
        /// HP < 0
        /// </summary>
        Tpye1,
        /// <summary>
        /// Karma > 100
        /// </summary>
        Tpye2,
        Tpye3,
        Type4,
        Type5,
        Type6,
        Type7,
        Type8,
        Type9,
        Type10,
        Type11,
        Type12,
        Type13,
        Type14,
        Type15,
        Type16,
        Type17,
        Type18,
        Type19,
        Type20
    }

    public enum NotificationType
    {
        Hp,
        Sp,
        Item,

        none
    }

    #endregion

    public const string EventFolder = "1.Event/EventControl";

    public List<GameObject> nowEventList = new List<GameObject>();
    public List<GameObject> removeEventList = new List<GameObject>();

    public  Dictionary<string , baseEventData> EventDataDictionary = new Dictionary<string, baseEventData>();

    [SerializeField]
    public List<baseEventData> EventDataObject;

    public List<baseEventData> SystemEventBattleResult;

    public List<baseEventData> SystemEventDie;

    public override void doStart()
    {
        base.doStart();

        foreach (var eventData in EventDataObject)
        {
            var eventKey = eventData.EventNo.ToString() + "_" + eventData.indexNo.ToString();
            EventDataDictionary.Add(eventKey, eventData);
        }
    }

    public override void doUpdate()
    {
        base.doUpdate();

        if(removeEventList.Count >0)
        {
            for(int i = 0; i <removeEventList.Count; i++)
            {
                GameObject.Destroy(removeEventList[i]);
            }
        }
    }

    public bool CheckLoad(int eventNo , int indexNo)
    {
        if(indexNo == -1 || indexNo == 0)
        {
            //randamlly , now just back to 0 by kao 20180521
            indexNo = 0;
        }

        var parent = GameObject.FindObjectOfType<EventReader>();
        if (parent)
        {
            foreach (var _event in EventDataObject)
            {
                if (_event.EventNo == eventNo)
                {
                    if (_event.indexNo == indexNo)
                    {
                        var eventObj = GameObject.Instantiate<GameObject>(_event.gameObject, parent.transform);
                        var eventData = eventObj.GetComponent<baseEventData>();
                        parent.SetNextEvent(eventData);
                        nowEventList.Add(eventObj);
                        return true;
                    }
                }
            }
        }

        Debug.LogError("Cant find no." + eventNo + "::"+ indexNo + " event at EventDataManager , Pls Check the prefabs");
        return false;
    }

    public bool CheckLoad(baseEventData eventdata)
    {
        var parent = GameObject.FindObjectOfType<EventReader>();
        if (parent)
        {
            var eventObj = GameObject.Instantiate<GameObject>(eventdata.gameObject, parent.transform);
            parent.SetNextEvent(eventdata);
            nowEventList.Add(eventObj);
            return true;
        }

        Debug.LogError("Cant find eventdata " + eventdata.name );
        return false;
    }

    public bool Next(int eventNo, int indexNo)
    {
        foreach (var obj in nowEventList)
        {
            removeEventList.Add(obj);
        }
        nowEventList.Clear();

        var result = CheckLoad(eventNo , indexNo);
        return result;
    }

    public bool Next(baseEventData eventdata)
    {
        foreach (var obj in nowEventList)
        {
            removeEventList.Add(obj);
        }
        nowEventList.Clear();

        var result = CheckLoad(eventdata);
        return result;
    }

    /// <summary>
    /// quick get eventdata
    /// </summary>
    /// <param name="EventNo"></param>
    /// <param name="IndexNo"></param>
    /// <returns></returns>
    public baseEventData getEventData(int EventNo, int IndexNo)
    {
        var eventKey = EventNo.ToString() + "_" + IndexNo.ToString();
        return EventDataDictionary[eventKey];
    }
}
