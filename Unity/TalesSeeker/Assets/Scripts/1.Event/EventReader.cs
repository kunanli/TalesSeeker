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
    public Text EventNameText;
    public Text EventChoiceTextLeft;
    public Text EventChoiceTextRight;

    //temp 
    public Slider HP;
    public Slider MP;
    public KarmaControl Karma;
    #endregion
    public GUIEvent guiEvent;
    Sprite Pic;

    public void Start()
    {
        //guiEvent = EventPic.GetComponent<GUIEvent>();
    }

    public void SetNextEvent(baseEventData _event)
    {
        noEventData = _event;
        EventPic.sprite = Resources.Load<Sprite>(_event.bgPath);
        Pic = Resources.Load<Sprite>(_event.picPath);
        EventText.text = _event.text[0];
        EventNameText.text = _event.cardName;
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
        var choicesData = noEventData.eventChoices;
        switch (choice)
        {
            case ChoiceType.Left:
                updateDmg(choicesData.ChoiceLeft);
                updateKarma(choicesData.ChoiceLeft);
                if (choicesData.ChoiceLeft.orderNextIndex)
                { 
                    EventManager.Instance.Next(noEventData.EventNo, choicesData.ChoiceLeft.orderIndexNo);
                }
                else
                {
                    EventManager.Instance.Next(choicesData.ChoiceLeft.orderEventNo, 0);
                }

                break;
            case ChoiceType.Right:
                updateDmg(choicesData.ChoiceRight);
                updateKarma(choicesData.ChoiceRight);
                if (choicesData.ChoiceRight.orderNextIndex)
                { 
                    EventManager.Instance.Next(noEventData.EventNo, choicesData.ChoiceRight.orderIndexNo);
                }
                else
                {
                    EventManager.Instance.Next(choicesData.ChoiceRight.orderEventNo, 0);
                }
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

    public void updateKarma(EventChoice.EventChoiceResult info)
    {
        var pl = GameManager.Instance.MainPlayer;
        var karma = pl.playerParam.karma + info.karma;
        if (karma < 0)
        {
            karma = 0;
            Karma.SetValue(0);
        }
        else if (karma >= 100)
        {
            karma = 100;
            Karma.SetValue(1);
        }
        else
        {
            Karma.SetValue(karma / Player.PlayerParam.MaxKarma);
        }

        pl.playerParam.karma = karma;
    }

}
