using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillControl : baseGUI
{

    public RectTransform FouseTop;

    public RectTransform FouseBottom;

    // Use this for initialization
    public override void Start () {
		
	}
	
	// Update is called once per frame
	public override  void Update ()
	{
        base.Update();

	    var rotZ = FouseTop.localEulerAngles.z;
	    var rotZbot = FouseBottom.localEulerAngles.z;

        FouseTop.localEulerAngles = new Vector3(0, 0, ++rotZ);
	    FouseBottom.localEulerAngles = new Vector3(0, 0, ++rotZbot);
    }
}
