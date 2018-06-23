using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationAction : MonoBehaviour
{

    public EventReader EventReader;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void doNotifaication()
    {
        EventReader.doNotification();
    }
}
