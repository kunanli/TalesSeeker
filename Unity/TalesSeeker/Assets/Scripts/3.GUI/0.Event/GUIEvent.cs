using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using System;

public class GUIEvent : baseGUI
{

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

    public bool CanDragY;

    public float SkillUIPressTime = 2;
    public SkillControl SkillUI;

    #region フィールド
    Vector2 ScreenScale = new Vector2();

    Vector3 startDragPoint;

    float rotz = 0;

    Vector2 OriPos;

    bool isDrag;

    DragType dragType = DragType.None;

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

    float skillUItimer = 0;
    private bool isCheckLongPress = false;
    #endregion

    // Use this for initialization
    public override void Start () {
        base.Start();

        OriPos = RectTransform.anchoredPosition;

        resetChoice();
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
        
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
                RectTransform.localEulerAngles = new Vector3(0, translation, 0);
                //image.Alpha(timer/ FadeInTime);
            }
            else
            {
                RectTransform.localEulerAngles = new Vector3(0, 0, 0);
                SetCardEnd();
            }
            return;
        }

        if (!isDrag && !isStartPlay)
        {
            RectTransform.anchoredPosition = OriPos;
            RectTransform.localEulerAngles =  new Vector3(0, 0, 0);
        }

        if (!isDrag && isCheckLongPress)
        {
           CheckPicLongPress();
        }

    }

    #region  buttonEvent
    public void OnPicDragBegin()
    {
        if (!isCanTrigger)
            return;
        isCheckLongPress = false;

        isDrag = true;
#if UNITY_EDITOR
        startDragPoint = Input.mousePosition;
#else
        startDragPoint = Input.touches[0].position;
#endif
        onceChace = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnPicDrag()
    {
        if (!isCanTrigger || onceChace)
            return;

        if (SkillUI.gameObject.activeInHierarchy)
            return;

        isCheckLongPress = false;
#if UNITY_EDITOR
        var pervPosition = new Vector3();
        var mousePosition = Input.mousePosition;
#else
        var pervPosition = new Vector2();
        var mousePosition = Input.touches[0].position;
#endif

        if (CanDragY)
        {
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
            var AddY = ((mousePosition.y - startDragPoint.y) / ScreenScale.y) / 20;

            //Limit Card PosY
            if (OriPos.y - RectTransform.anchoredPosition.y > CardLimitY && AddY < 0)
            {
                AddY = 0;
            }
            else if (OriPos.y - RectTransform.anchoredPosition.y < -CardLimitY && AddY > 0)
            {
                AddY = 0;
            }

                RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x,
                    RectTransform.anchoredPosition.y + (AddY));
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

        RectTransform.localEulerAngles = new Vector3(0, 0, -rotz);
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

    public void OnPicLongPress()
    {
        isCheckLongPress = true;
        if (skillUItimer < SkillUIPressTime)
        {
            skillUItimer += Time.deltaTime;
        }
        else
        {
            if (!SkillUI.gameObject.activeInHierarchy)
            {
                SkillUI.gameObject.SetActive(true);
            }
        }
    }

    public void OnPicExit()
    {
        isCheckLongPress = false;
        skillUItimer = 0;
        SkillUI.gameObject.SetActive(false);
    }
    #endregion
    public void SetCardStart()
    {
        isStartPlay = true;
        isCanTrigger = false;

        RectTransform.anchoredPosition = OriPos;
        RectTransform.localEulerAngles = new Vector3(0, 180, 0);
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

    public void CheckPicLongPress()
    {
        if (skillUItimer < SkillUIPressTime)
        {
            skillUItimer += Time.deltaTime;
        }
        else
        {
            if (!SkillUI.gameObject.activeInHierarchy)
            {
                SkillUI.gameObject.SetActive(true);
            }
        }
    }
}
