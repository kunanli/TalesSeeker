using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class baseGUI : MonoBehaviour
{

    float fadeInTime = 0;
    float fadeInMaxTime = 0;

    //gather component
    public RectTransform RectTransform ;
    public Image Image;

    // Use this for initialization
    public virtual void Start () {
        //gather component
	    RectTransform = this.gameObject.GetComponent<RectTransform>();
        Image = this.gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    public virtual void Update () {
        if (Image)
        {
            if (fadeInTime > 0)
            {
                fadeInTime -= Time.deltaTime;
                Image.Alpha( 1- (fadeInTime / fadeInMaxTime));
            }
        }
	}

    public void RequestFadeIn(float timer )
    {
        if (Image)
        {
            fadeInTime = fadeInMaxTime = timer;
            Image.Alpha(0);
        }
    }
}
