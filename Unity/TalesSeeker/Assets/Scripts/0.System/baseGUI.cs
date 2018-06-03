using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseGUI : MonoBehaviour {

    //gather component
    public RectTransform RectTransform ;

	// Use this for initialization
	public virtual void Start () {
        //gather component
	    RectTransform = this.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    public virtual void Update () {
		
	}
}
