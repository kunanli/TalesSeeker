using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EventDataManager : baseSingleton<EventDataManager> {

    public const string EventFolder = "1.Event/EventControl";

    public List<GameObject> nowEventList = new List<GameObject>();
    public List<GameObject> removeEventList = new List<GameObject>();

    [SerializeField]
    public List<GameObject> EventDataObject;

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

    public bool CheckLoad(int no)
    {
        var parent = GameObject.FindObjectOfType<EventReader>();
        if(parent)
        {
            var eventObj = GameObject.Instantiate<GameObject>(EventDataObject[no], parent.transform);
            var eventData = eventObj.GetComponent<baseEventData>();
            parent.SetNextEvent(eventData);
            nowEventList.Add(eventObj);
            return true;
        }
        return false;
    }

    public bool Next(int no)
    {
        foreach (var obj in nowEventList)
        {
            removeEventList.Add(obj);
        }
        nowEventList.Clear();

        var result = CheckLoad(no);
        return result;
    }
}
