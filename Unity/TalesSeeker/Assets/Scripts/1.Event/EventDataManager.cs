using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EventDataManager : baseSingleton<EventDataManager> {

    public const string EventFolder = "1.Event/EventControl";

    public List<GameObject> nowEventList = new List<GameObject>();
    public List<GameObject> removeEventList = new List<GameObject>();

    [SerializeField]
    public List<baseEventData> EventDataObject;

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
        if(no == -1)
        {
            //randamlly , now just back to 0 by kao 20180521
            no = 0;
        }

        var parent = GameObject.FindObjectOfType<EventReader>();
        if (parent)
        {
            foreach (var _event in EventDataObject)
            {
                if (_event.indexNo == no)
                {
                    var eventObj = GameObject.Instantiate<GameObject>(_event.gameObject, parent.transform);
                    var eventData = eventObj.GetComponent<baseEventData>();
                    parent.SetNextEvent(eventData);
                    nowEventList.Add(eventObj);
                    return true;
                }
            }
        }

        Debug.LogError("Cant find no." + no + " event at EventDataManager , Pls Check the prefabs");
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
