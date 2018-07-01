using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixResolutionCanvas : MonoBehaviour
{
    public CanvasScaler CanvasScaler;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float Swidth = Screen.width;
        float Sheight = Screen.height;

        if ((Swidth / Sheight) >= (1080f / 1920f) && (Swidth / Sheight) < (1200f / 1920f))
        {
            CanvasScaler.referenceResolution = new Vector2(1080,1920);
        }
        else
        {
            CanvasScaler.referenceResolution = new Vector2(1200, 1920);
        }

    }
}
