using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsButton : baseGUI
{

    public Vector2 ShowPosX;

    public Vector2 HidePosX;

    public GameObject ItemContent;

    public GameObject ItemSlot;

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
        RectTransform.anchoredPosition = HidePosX;
    }

    public void OnMouseClick()
    {
        GameObject.Instantiate(ItemSlot, ItemContent.transform);
    }

}
