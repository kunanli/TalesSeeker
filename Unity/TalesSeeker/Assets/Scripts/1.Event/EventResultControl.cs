using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventResultControl : baseGUI {

	// Use this for initialization
	public override  void Start () {
        base.Start();

        RequestFadeIn(1);
	}
	
    public void OnClick()
    {
        if (!CheckFadeIn())
            return;

        SceneManager.LoadScene(1);
    }
}
