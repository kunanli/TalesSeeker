using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixResolution : MonoBehaviour
{
    private CanvasScaler CanvasScaler;

    // Use this for initialization
    void Start ()
    {
        CanvasScaler = this.GetComponent<CanvasScaler>();

        float Swidth = Screen.width;
        float Sheight = Screen.height;

        //Debug.Log("Resolution " + Swidth / Sheight  + " 16:9 " + 1080f/1920f + " 16:10 " + 1200f/1920f);

        if ((Swidth / Sheight) >= (1080f / 1920f) && (Swidth / Sheight) < (1200f / 1920f))
        {
            CanvasScaler.referenceResolution = new Vector2(1080f, 1920f);
        }
        else if ((Swidth / Sheight) >= (1200f / 1920f))
        {
            CanvasScaler.referenceResolution = new Vector2(1200f, 1920f);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
