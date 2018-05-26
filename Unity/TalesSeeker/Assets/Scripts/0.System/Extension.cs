using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static partial class Extension
{
    public static void Alpha(this Image image, float alpha)
    {
        image.color = new Color(image.color.r , image.color.g, image.color.b , alpha);
    }

    public static void Alpha(this Text text, float alpha)
    {

        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    }

}
