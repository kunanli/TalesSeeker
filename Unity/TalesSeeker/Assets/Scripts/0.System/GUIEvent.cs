using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using System;

public class GUIEvent : MonoBehaviour {

    public const float ScreenWidth = 1080;
    public const float ScreenHeigh = 1920;

    [Flags]
    public enum DragType
    {
        None =0,
        Up = 1 << 0,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
    }

    public bool IsCanTrigger { get { return isCanTrigger; } }

    public Text ChoiceLeft;
    public Text ChoiceRight;

    #region フィールド
    Vector2 ScreenScale = new Vector2();

    Vector3 startDragPoint;

    float rotz = 0;

    Vector2 OriPos;

    bool isDrag;

    DragType dragType = DragType.None;

    RectTransform rectTransform;

    bool isCanTrigger = true;
#endregion

    // Use this for initialization
    void Start () {

        rectTransform = this.gameObject.GetComponent<RectTransform>();

        OriPos = rectTransform.anchoredPosition;

        resetChoice();
    }

    // Update is called once per frame
    void Update() {

        float widthscale = (Screen.width / ScreenWidth);
        float heighscale = (Screen.height / ScreenHeigh);

        ScreenScale = new Vector2(widthscale , heighscale);

        if (!isDrag)
        {
            rectTransform.anchoredPosition = OriPos;
            rectTransform.localEulerAngles =  new Vector3(0, 0, 0);
        }
	}

    public void OnPicDragBegin()
    {
        isDrag = true;
        startDragPoint = Input.mousePosition;
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnPicDrag()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position); //觸控
        }
        var pervPosition = new Vector3();

        if (pervPosition != Input.mousePosition)
        {
            if ((dragType & DragType.Down) != 0)
            {
                if (pervPosition.y < Input.mousePosition.y)
                {
                    startDragPoint = Input.mousePosition;
                }
            }
            else if ((dragType & DragType.Up) != 0)
            {
                if (pervPosition.y > Input.mousePosition.y)
                {
                    startDragPoint = Input.mousePosition;
                }
            }

            if (pervPosition.y > Input.mousePosition.y)
            {
                dragType = DragType.Down;
            }
            if (pervPosition.y < Input.mousePosition.y)
            {
                dragType = DragType.Up;
            }
        }

        //1Frame prev position
        pervPosition = Input.mousePosition;
        var AddY = ((Input.mousePosition.y - startDragPoint.y  ) / ScreenScale.y) / 20;

        //Limit
        if (OriPos.y - rectTransform.anchoredPosition.y > 30 && AddY <0)
        {
            AddY = 0;
        }
        else if (OriPos.y - rectTransform.anchoredPosition.y < -30 && AddY > 0)
        {
            AddY = 0;
        }

        //X Rotation (Z)
        var Xpoint = Input.mousePosition.x / ScreenScale.x;
        var AddX = (Xpoint - (ScreenWidth/2));
        Debug.Log("Addx  " + AddX);
        if (AddX < 250 && AddX > -250)
        {
            rotz = (AddX / 250) * 7f;
        }
        else
        {
            if (AddX > 0)
                rotz = 7;
            else if (AddX < 0)
                rotz = -7;
        }

        //choice
        if (rotz > 5 || rotz < -5)
        {
            if (rotz > 0)
            {
                ChoiceLeft.color = new Color(ChoiceLeft.color.r, ChoiceLeft.color.g, ChoiceLeft.color.b, (rotz - 5) / 2);
            }
            else if (rotz < 0)
            {
                ChoiceRight.color = new Color(ChoiceRight.color.r, ChoiceRight.color.g, ChoiceRight.color.b, -((rotz + 5) / 2));
            }
        }
        else
        {
            resetChoice();
        }

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x , rectTransform.anchoredPosition.y + (AddY));
        rectTransform.localEulerAngles = new Vector3(0, 0, -rotz);
    }

    public void OnPicDragEnd()
    {
        isDrag = false;
        rotz = 0;
        resetChoice();
    }

    /// <summary>
    /// Reset Choice alpha
    /// </summary>
    void resetChoice()
    {
        ChoiceLeft.color = new Color(ChoiceLeft.color.r, ChoiceLeft.color.g, ChoiceLeft.color.b, 0);
        ChoiceRight.color = new Color(ChoiceRight.color.r, ChoiceRight.color.g, ChoiceRight.color.b, 0);
    }
}
