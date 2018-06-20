using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsButton : baseGUI
{

    public Vector2 ShowPosX;

    public Vector2 HidePosX;

    public GameObject ItemContent;

    public GameObject ItemSlot;

    public Sprite[] tempItemIcon;

    public override void Start()
    {
        base.Start();

        //init
        RectTransform.anchoredPosition = HidePosX;
    }

    public void OnMouseEnter()
    {
        RectTransform.anchoredPosition = ShowPosX;
    }

    public void OnMouseExit()
    {
#if UNITY_EDITOR
        RectTransform.anchoredPosition = HidePosX;
#endif
    }

    public void OnMouseClick()
    {
        var item = GameObject.Instantiate(ItemSlot, ItemContent.transform);

        //randomlly Icon
        var image = item.GetComponent<Image>();
        var random = Random.value * 4f;
        if (random < 1)
        {
            image.sprite = tempItemIcon[0];
        }
        else if (random >= 1 && random < 2)
        {
            image.sprite = tempItemIcon[1];
        }
        else if (random >= 2 && random < 3)
        {
            image.sprite = tempItemIcon[2];
        }
        else if (random >= 3 && random < 4)
        {
            image.sprite = tempItemIcon[3];
        }
    }

}
