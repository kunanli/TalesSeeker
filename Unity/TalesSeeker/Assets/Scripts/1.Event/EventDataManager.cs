using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EventDataManager : baseSingleton<EventDataManager> {

    public const string EventFolder = "1.Event/EventControl";

    [SerializeField]
    public List<GameObject> EventDataObject;

    public bool CheckLoad(int no)
    {
        var parent = GameObject.FindObjectOfType<EventReader>();
        if(parent)
        {
            var eventObj = GameObject.Instantiate<GameObject>(EventDataObject[no], parent.transform);
            var eventData = eventObj.GetComponent<baseEventData>();
            parent.SetNextEvent(eventData);
            return true;
        }
        return false;
    }
}
