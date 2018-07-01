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
    public float DropTime = 2.0f;
    public float DropSpeed = 2.0f;

    public Image EventPic;
    public Text ChoiceLeft;
    public Image ChoiceLeftBG;
    public Text ChoiceRight;
    public Image ChoiceRightBG;
    public EventReader EventReader;
    public Image EventDropPlayer;
    public Image EventDropPlayerPic;

    public bool CanDragY;

    public float SkillUIPressTime = 2;
    public SkillControl SkillUI;

    public Animator NotificationObj;

    #region フィールド

    private Animator Animator;

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
    /// Is turn on card Play Start
    /// </summary>
    bool isStartPlayTurnOn = false;

    /// <summary>
    /// 
    /// </summary>
    bool isStartPlayDrop = false;


    /// <summary>
    /// safe check
    /// </summary>
    bool onceChance = false;

    float timer = 0;

    float skillUItimer = 0;
    private bool isCheckLongPress = false;

    /// <summary>
    /// last time choice side
    /// </summary>
    private EventReader.ChoiceType preChoiceType;


    #endregion

    // Use this for initialization
    public override void Start () {
        base.Start();

        OriPos = RectTransform.anchoredPosition;
        Animator = gameObject.GetComponent<Animator>();

        NotificationObj.enabled = false;
        NotificationObj.gameObject.SetActive(false);

        resetChoice();
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
        
        ScreenScale = new Vector2(widthscale , heighscale);

        if (!isStartPlayTurnOn && isStartPlayDrop)
        {
            if (timer < DropTime)
            {
                switch (preChoiceType)
                {
                    case EventReader.ChoiceType.Right:
                        timer += Time.deltaTime;
                        var rate = (timer / DropTime);
                        var translation = EventDropPlayer.rectTransform.localEulerAngles.z;
                        translation = translation + (DropSpeed * timer);
                        EventDropPlayer.rectTransform.localEulerAngles = new Vector3(0, 0, translation);
                        RectTransform.localEulerAngles = new Vector3(0, 180, 0);
                        if (translation < 285 && translation > 75)
                        {
                            EventDropPlayer.gameObject.SetActive(false);
                            isStartPlayTurnOn = true;
                            timer = 0;
                        }
                        break;
                    case EventReader.ChoiceType.Left:
                        timer += Time.deltaTime;
                        translation = EventDropPlayer.rectTransform.localEulerAngles.z;
                        translation = translation - (DropSpeed * timer);
                        EventDropPlayer.rectTransform.localEulerAngles = new Vector3(0, 0, translation);
                        RectTransform.localEulerAngles = new Vector3(0, 180, 0);
                        if (translation < 285 && translation > 75)
                        {
                            EventDropPlayer.gameObject.SetActive(false);
                            isStartPlayTurnOn = true;
                            timer = 0;
                        }
                        break;
                }

            }
            else
            {
                switch (preChoiceType)
                {
                    case EventReader.ChoiceType.Right:
                            EventDropPlayer.gameObject.SetActive(false);
                            isStartPlayTurnOn = true;
                            timer = 0;
                        break;
                    case EventReader.ChoiceType.Left:
                            EventDropPlayer.gameObject.SetActive(false);
                            isStartPlayTurnOn = true;
                            timer = 0;
                        break;
                }
            }
        }
        else if (isStartPlayTurnOn)
        {
            if(!Animator.enabled)
                Animator.enabled = true;

            var state = Animator.GetCurrentAnimatorStateInfo(0);
            switch (preChoiceType)
            {
                case EventReader.ChoiceType.Right:
                    if (state.IsName("Idle"))
                    {
                        Animator.Play("TurnOnLeft");
                    }
                    else if (!state.IsName("Idle"))
                    {
                        float translation = RectTransform.eulerAngles.y;
                        if (translation > 270 && translation != 0)
                        {
                            EventReader.ChangeBG2PIC();
                        }
                        else if (translation == 0 || translation == 360)
                        {
                            RectTransform.localEulerAngles = new Vector3(0, 0, 0);
                            SetCardEnd();
                        }
                    }
                    break;
                case EventReader.ChoiceType.Left:
                    if (state.IsName("Idle"))
                    {
                        Animator.Play("TurnOnRight");
                    }
                    else if (!state.IsName("Idle"))
                    {
                        float translation = RectTransform.eulerAngles.y;
                        if (translation < 90 && translation != 0)
                        {
                            EventReader.ChangeBG2PIC();
                        }
                        else if (translation == 360 || translation==0)
                        {
                            RectTransform.localEulerAngles = new Vector3(0, 0, 0);
                            SetCardEnd();
                        }
                    }
                    break;
            }
            return;
        }

        if (!isDrag && !isStartPlayTurnOn)
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
        onceChance = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnPicDrag()
    {
        if (!isCanTrigger || onceChance)
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
        var Xpoint = (mousePosition.x - startDragPoint.x) / ScreenScale.x;
        if (Xpoint == 0)
            return;
        var AddX = (Xpoint );
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
                ChoiceLeft.gameObject.SetActive(true);
                ChoiceLeft.rectTransform.localEulerAngles = new Vector3(0, 0, +rotz);
                ChoiceLeftBG.gameObject.SetActive(true);
            }
            else if (rotz < 0)
            {
                ChoiceRight.gameObject.SetActive(true);
                ChoiceRight.rectTransform.localEulerAngles = new Vector3(0, 0, +rotz);
                ChoiceRightBG.gameObject.SetActive(true);
            }
        }
        else
        {
            resetChoice();
        }

        if (AddX > CardDecideLimitX )
        {
            EventReader.ToNext(EventReader.ChoiceType.Left);
            preChoiceType = EventReader.ChoiceType.Left;
            onceChance = true;
        }
        else if ( AddX < -CardDecideLimitX)
        {
            EventReader.ToNext(EventReader.ChoiceType.Right);
            preChoiceType = EventReader.ChoiceType.Right;
            onceChance = true;
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
        onceChance = false;
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
    /// <summary>
    /// card change effect start 
    /// </summary>
    public void SetCardStart()
    {
        isStartPlayDrop = true;
        EventDropPlayerPic.sprite = EventPic.sprite;
        EventDropPlayer.gameObject.SetActive(true);
        EventDropPlayer.rectTransform.localEulerAngles = EventPic.rectTransform.eulerAngles;
        switch (preChoiceType)
        {
            case EventReader.ChoiceType.Left:
                EventDropPlayer.rectTransform.localEulerAngles = new Vector3(0, 0, EventDropPlayer.rectTransform.localEulerAngles.z - 1);
                break;
            case EventReader.ChoiceType.Right:
                EventDropPlayer.rectTransform.localEulerAngles = new Vector3(0, 0, EventDropPlayer.rectTransform.localEulerAngles.z + 1);
                break;
        }
        isCanTrigger = false;

        RectTransform.anchoredPosition = OriPos;
        RectTransform.localEulerAngles = new Vector3(0, 180, 0);
        resetChoice();
        //image.Alpha(0);

    }

    public void SetCardEnd()
    {
        Animator.Play("Idle");
        Animator.enabled = false;
        if (EventReader.CheckNotification())
        {

            NotificationObj.gameObject.SetActive(true);
            NotificationObj.enabled = true;
            if (NotificationObj.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                isCanTrigger = true;
                isStartPlayTurnOn = false;
                isStartPlayDrop = false;
                timer = 0;
                preChoiceType = new EventReader.ChoiceType();

                NotificationObj.enabled = false;
                NotificationObj.gameObject.SetActive(false);
            }
        }
        else
        {
            isCanTrigger = true;
            isStartPlayTurnOn = false;
            isStartPlayDrop = false;
            timer = 0;
            preChoiceType = new EventReader.ChoiceType();
        }

    }

    /// <summary>
    /// Reset Choice alpha
    /// </summary>
    void resetChoice()
    {
        ChoiceLeft.gameObject.SetActive(false);
        ChoiceLeftBG.gameObject.SetActive(false);
        ChoiceRight.gameObject.SetActive(false);
        ChoiceRightBG.gameObject.SetActive(false);
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
