using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixResolution_Item : MonoBehaviour
{
    public const int spacefor1200 = 100;
    public const int spacefor1080 = 80;

    private HorizontalLayoutGroup HorizontalLayoutGroup;

    private float rate;

    private float sizex;

    private float posx;


    // Use this for initialization
    void Start()
    {
        HorizontalLayoutGroup = this.GetComponent<HorizontalLayoutGroup>();

        rate = (1080f / 1200f);

        sizex = spacefor1080 * rate;
        posx = HorizontalLayoutGroup.padding.left * rate;
    }

    // Update is called once per frame
    void Update()
    {
        float Swidth = Screen.width;
        float Sheight = Screen.height;

        if ((Swidth / Sheight) >= (1080f / 1920f) && (Swidth / Sheight) < (1200f / 1920f))
        {
            HorizontalLayoutGroup.padding.left = (int)posx;
            HorizontalLayoutGroup.padding.right = (int)posx;
            HorizontalLayoutGroup.spacing = sizex;
        }

    }
}
