using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventReader : MonoBehaviour {

    public enum ChoiceType{
        Left,
        Right,
    }

    #region field
    public int textStep = 0;

    public int textStepMax = 1;

    baseEventData noEventData;
    #endregion

    #region UIComponent
    public Image EventBG;
    public Image EventPic;


    public Text EventText;
    public Text EventChoiceTextLeft;
    public Text EventChoiceTextRight;

    //temp 
    public Slider HP;
    public Slider MP;
    #endregion
    public GUIEvent guiEvent;
    Sprite Pic;

    public void Start()
    {
        guiEvent = EventPic.GetComponent<GUIEvent>();
    }

    public void SetNextEvent(baseEventData _event)
    {
        noEventData = _event;
        EventPic.sprite = Resources.Load<Sprite>(_event.bgPath);
        Pic = Resources.Load<Sprite>(_event.picPath);
        EventText.text = _event.text[0];
        textStepMax = _event.text.Length;

        EventChoiceTextLeft.text    = _event.eventChoices.ChoiceLeft.choiceText;
        EventChoiceTextRight.text   = _event.eventChoices.ChoiceRight.choiceText;

        guiEvent.SetCardStart();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="choice"></param>
    public void ToNext(ChoiceType choice)
    {
        switch (choice)
        {
            case ChoiceType.Left:
                updateDmg(noEventData.eventChoices.ChoiceLeft);
                EventManager.Instance.Next(noEventData.eventChoices.ChoiceLeft.orderEventNo);
                break;
            case ChoiceType.Right:
                updateDmg(noEventData.eventChoices.ChoiceRight);
                EventManager.Instance.Next(noEventData.eventChoices.ChoiceRight.orderEventNo);
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ChangeBG2PIC()
    {
        EventPic.sprite = Pic;
    }

    public void updateDmg(EventChoice.EventChoiceResult info)
    {
        var pl = GameManager.Instance.MainPlayer;
        var hp = pl.playerParam.hp + info.enemyDmg;
        if (hp < pl.playerParam.Maxhp)
        {
            if (hp < 0)
            {
                pl.playerParam.hp = 0;
                HP.value = 0;
            }
            else
            {
                pl.playerParam.hp = hp;
                HP.value = pl.playerParam.hp / pl.playerParam.Maxhp;
            }
        }
        else
        {
            pl.playerParam.hp = pl.playerParam.Maxhp;
            HP.value = 1;
        }

        pl.playerParam.mp -= 20;
        MP.value = pl.playerParam.mp / pl.playerParam.Maxmp;
    }

}
