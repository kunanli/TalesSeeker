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

    public float widthscale { get { return Screen.width / ScreenWidth; } }
    public float heighscale { get { return Screen.height / ScreenHeigh; } }

    public float CardLimitY = 30f;
    public float CardShowChoiceLimitX = 250;
    public float CardDecideLimitX = 450;

    public float CardMaxRotaionZ = 7;
    public float CardShowChoiceRotaionZ = 5;

    public float FadeInTime = 2.0f;

    public Text ChoiceLeft;
    public Text ChoiceRight;
    public EventReader EventReader;

    #region フィールド
    Vector2 ScreenScale = new Vector2();

    Vector3 startDragPoint;

    float rotz = 0;

    Vector2 OriPos;

    bool isDrag;

    DragType dragType = DragType.None;

    //componet
    RectTransform rectTransform;

    /// <summary>
    /// Is can interact
    /// </summary>
    bool isCanTrigger = true;

    /// <summary>
    /// Is 
    /// </summary>
    bool isStartPlay = false;


    /// <summary>
    /// safe check
    /// </summary>
    bool onceChace = false;

    float timer = 0;
    #endregion

    // Use this for initialization
    void Start () {

        //gather component
        rectTransform = this.gameObject.GetComponent<RectTransform>();

        OriPos = rectTransform.anchoredPosition;

        resetChoice();
    }

    // Update is called once per frame
    void Update() {
        ScreenScale = new Vector2(widthscale , heighscale);

        if (isStartPlay)
        {
            if(timer < FadeInTime)
            {
                timer += Time.deltaTime;
                float translation = 180 - (timer * 300 );
                if (translation < 90)
                {
                    EventReader.ChangeBG2PIC();
                }
                rectTransform.localEulerAngles = new Vector3(0, translation, 0);
                //image.Alpha(timer/ FadeInTime);
            }
            else
            {
                rectTransform.localEulerAngles = new Vector3(0, 0, 0);
                SetCardEnd();
            }
            return;
        }

        if (!isDrag && !isStartPlay)
        {
            rectTransform.anchoredPosition = OriPos;
            rectTransform.localEulerAngles =  new Vector3(0, 0, 0);
        }
	}

    public void OnPicDragBegin()
    {
        if (!isCanTrigger)
            return;

        isDrag = true;
        startDragPoint = Input.mousePosition;
        onceChace = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnPicDrag()
    {
        if (!isCanTrigger || onceChace)
            return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position); //觸控
        }
        var pervPosition = new Vector3();

        var mousePosition = Input.mousePosition;

        //check the mouse or point Reversal
        if (pervPosition != mousePosition)
        {
            if ((dragType & DragType.Down) != 0)
            {
                if (pervPosition.y < mousePosition.y)
                {
                    startDragPoint = mousePosition;
                }
            }
            else if ((dragType & DragType.Up) != 0)
            {
                if (pervPosition.y > mousePosition.y)
                {
                    startDragPoint = mousePosition;
                }
            }

            if (pervPosition.y > mousePosition.y)
            {
                dragType = DragType.Down;
            }
            if (pervPosition.y < mousePosition.y)
            {
                dragType = DragType.Up;
            }
        }

        //1Frame prev position
        pervPosition = mousePosition;
        var AddY = ((mousePosition.y - startDragPoint.y  ) / ScreenScale.y) / 20;

        //Limit Card PosY
        if (OriPos.y - rectTransform.anchoredPosition.y > CardLimitY && AddY <0)
        {
            AddY = 0;
        }
        else if (OriPos.y - rectTransform.anchoredPosition.y < -CardLimitY && AddY > 0)
        {
            AddY = 0;
        }

        //X Rotation (Z)
        //calc card rot Z
        var Xpoint = mousePosition.x / ScreenScale.x;
        var AddX = (Xpoint - (ScreenWidth/2));
        //Debug.Log("Addx  " + AddX);
        if (AddX < CardShowChoiceLimitX && AddX > -CardShowChoiceLimitX)
        {
            rotz = (AddX / CardShowChoiceLimitX) * CardMaxRotaionZ;
        }
        else
        {
            if (AddX > 0)
                rotz = CardMaxRotaionZ;
            else if (AddX < 0)
                rotz = -CardMaxRotaionZ;
        }

        // show choice
        if (rotz > CardShowChoiceRotaionZ || rotz < -CardShowChoiceRotaionZ)
        {
            if (rotz > 0)
            {
                ChoiceLeft.Alpha((rotz - 5) / 2);
            }
            else if (rotz < 0)
            {
                ChoiceRight.Alpha(-((rotz + 5) / 2));
            }
        }
        else
        {
            resetChoice();
        }

        if (AddX > CardDecideLimitX )
        {
            EventReader.ToNext(EventReader.ChoiceType.Left);
            onceChace = true;
        }
        else if ( AddX < -CardDecideLimitX)
        {
            EventReader.ToNext(EventReader.ChoiceType.Right);
            onceChace = true;
        }

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x , rectTransform.anchoredPosition.y + (AddY));
        rectTransform.localEulerAngles = new Vector3(0, 0, -rotz);
    }

    public void OnPicDragEnd()
    {
        if (!isCanTrigger)
            return;

        isDrag = false;
        rotz = 0;
        resetChoice();
        onceChace = false;
    }

    public void SetCardStart()
    {
        isStartPlay = true;
        isCanTrigger = false;

        rectTransform.anchoredPosition = OriPos;
        rectTransform.localEulerAngles = new Vector3(0, 180, 0);
        resetChoice();
        //image.Alpha(0);

    }

    public void SetCardEnd()
    {
        isCanTrigger = true;
        isStartPlay = false;
        timer = 0;
    }


    /// <summary>
    /// Reset Choice alpha
    /// </summary>
    void resetChoice()
    {
        ChoiceLeft.Alpha( 0);
        ChoiceRight.Alpha( 0);
    }

}
