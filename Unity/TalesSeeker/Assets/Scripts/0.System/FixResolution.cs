using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixResolution : MonoBehaviour
{
    private CanvasScaler CanvasScaler;

    private RectTransform RectTransform;

    private float rate;

    private float sizex;

    private float posx;
    // Use this for initialization
    void Start ()
    {
        CanvasScaler = this.GetComponent<CanvasScaler>();
        RectTransform = this.GetComponent<RectTransform>();

        var size = RectTransform.sizeDelta.x;
        var pos = RectTransform.anchoredPosition.x;
        rate = (1080f / 1200f);
        sizex = size * rate;
        posx = pos * rate;

    }
	
	// Update is called once per frame
	void Update () {
	    float Swidth = Screen.width;
	    float Sheight = Screen.height;

        if ((Swidth / Sheight) >= (1080f / 1920f) && (Swidth / Sheight) < (1200f / 1920f))
	    {
	        RectTransform.sizeDelta = new Vector2(sizex, RectTransform.sizeDelta.y);
            RectTransform.anchoredPosition = new Vector2(posx , RectTransform.anchoredPosition.y);
	    }

    }
}
