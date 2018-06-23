using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    bool BattleMode = false;
    int BattleNextEvent = -1;
    int BattleNextIndex = -1;
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

    bool fristTime = true;
    public void Start()
    {
        //guiEvent = EventPic.GetComponent<GUIEvent>();
    }

    public void SetNextEvent(baseEventData _event)
    {
        if (!fristTime)
        {
            guiEvent.SetCardStart();

            noEventData = _event;
            EventPic.sprite = Resources.Load<Sprite>(_event.bgPath);
            Pic = Resources.Load<Sprite>(_event.picPath);
            EventText.text = _event.text[0];
            EventNameText.text = _event.cardName;
            textStepMax = _event.text.Length;

            EventChoiceTextLeft.text = _event.eventChoices.ChoiceLeft.choiceText;
            EventChoiceTextRight.text = _event.eventChoices.ChoiceRight.choiceText;
        }
        else
        {
            noEventData = _event;
            EventPic.sprite = Resources.Load<Sprite>(_event.picPath);
            EventBG.sprite = Resources.Load<Sprite>(_event.bgPath);
            EventText.text = _event.text[0];
            EventNameText.text = _event.cardName;
            textStepMax = _event.text.Length;

            EventChoiceTextLeft.text = _event.eventChoices.ChoiceLeft.choiceText;
            EventChoiceTextRight.text = _event.eventChoices.ChoiceRight.choiceText;

            fristTime = false;
        }
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

                if (BattleMode)
                {
                    BattleMode = false;
                    EventManager.Instance.Next(BattleNextEvent, BattleNextIndex);
                    BattleNextEvent = -1;
                    BattleNextIndex = -1;
                    return;
                }

                if (choicesData.ChoiceLeft.randomNextIndex)
                {
                    var indexNo = randomEventPickUp(choicesData, choicesData.ChoiceLeft.randomIndexNo);
                    if (noEventData.ShowBattleResult)
                    {
                        ShowBattleResult(noEventData.EventNo , indexNo);
                    }
                    else
                    {
                        EventManager.Instance.Next(noEventData.EventNo, indexNo);
                    }
                }
                else if (choicesData.ChoiceLeft.orderNextIndex)
                {
                    if (noEventData.ShowBattleResult)
                    {
                        ShowBattleResult(noEventData.EventNo, choicesData.ChoiceLeft.orderIndexNo);
                    }
                    else
                    {
                        EventManager.Instance.Next(noEventData.EventNo, choicesData.ChoiceLeft.orderIndexNo);
                    }
                }
                else
                {
                    //for kunAn
                    //20180617 start with index 1
                    if (noEventData.ShowBattleResult)
                    {
                        ShowBattleResult(noEventData.EventNo, 1);
                    }
                    else
                    {
                        EventManager.Instance.Next(choicesData.ChoiceLeft.orderEventNo, 1);
                    }
                }

                break;
            case ChoiceType.Right:
                updateDmg(choicesData.ChoiceRight);
                updateKarma(choicesData.ChoiceRight);

                if (BattleMode)
                {
                    BattleMode = false;
                    EventManager.Instance.Next(BattleNextEvent, BattleNextIndex);
                    BattleNextEvent = -1;
                    BattleNextIndex = -1;
                    return;
                }

                if (choicesData.ChoiceRight.randomNextIndex)
                {
                    var indexNo = randomEventPickUp(choicesData, choicesData.ChoiceRight.randomIndexNo);
                    if (noEventData.ShowBattleResult)
                    {
                        ShowBattleResult(noEventData.EventNo, indexNo);
                    }
                    else
                    {
                        EventManager.Instance.Next(noEventData.EventNo, indexNo);
                    }
                }
                else if (choicesData.ChoiceRight.orderNextIndex)
                {
                    if (noEventData.ShowBattleResult)
                    {
                        ShowBattleResult(noEventData.EventNo, choicesData.ChoiceRight.orderIndexNo);
                    }
                    else
                    {
                        EventManager.Instance.Next(noEventData.EventNo, choicesData.ChoiceRight.orderIndexNo);
                    }
                }
                else
                {
                    if (noEventData.ShowBattleResult)
                    {
                        ShowBattleResult(noEventData.EventNo, 1);
                    }
                    else
                    {
                        //for kunAn
                        //20180617 start with index 1
                        EventManager.Instance.Next(choicesData.ChoiceRight.orderEventNo, 1);
                    }
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
        if (pl.playerParam.hp <= 0)
        {
            SceneManager.LoadScene(0);
        }
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


    int randomEventPickUp(EventChoice choicesData , List<EventChoice.EventChoiceResult.RandomIndexSetting> randomIndexList)
    {
        var randomMaxValue = 0f;
        foreach (var randomEvent in randomIndexList)
        {
            randomMaxValue += randomEvent.randomWeight;
        }
        var nextRandomEvent = -1;
        while (nextRandomEvent == -1)
        {
            foreach (var randomEvent in randomIndexList)
            {
                //check weight
                var randomValue = Random.value * randomMaxValue;
                if (randomValue < randomEvent.randomWeight)
                {
                    var nextEventData =
                        EventDataManager.Instance.getEventData(noEventData.EventNo, randomEvent.indexNo);
                    //only one check
                    if (nextEventData.OnlyOneEvent)
                    {
                        if (!EventManager.Instance.CheckIndexOnlyOne(randomEvent.indexNo))
                        {
                            // need Index check
                            if (nextEventData.needIndexNo.Count > 0)
                            {
                                //check need
                                foreach (var needIndex in nextEventData.needIndexNo)
                                {
                                    if (!EventManager.Instance.CheckIndexNeed(needIndex))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        nextRandomEvent = randomEvent.indexNo;
                                    }
                                }
                            }
                            else
                            {
                                nextRandomEvent = randomEvent.indexNo;
                            }
                        }
                    }
                    else
                    {
                        // need Index check
                        if (nextEventData.needIndexNo.Count > 0)
                        {
                            //check need
                            foreach (var needIndex in nextEventData.needIndexNo)
                            {
                                if (!EventManager.Instance.CheckIndexNeed(needIndex))
                                {
                                    continue;
                                }
                                else
                                {
                                    nextRandomEvent = randomEvent.indexNo;
                                }
                            }
                        }
                        else
                        {
                            nextRandomEvent = randomEvent.indexNo;
                        }
                    }
                }
            }
        }

        return nextRandomEvent;
    }

    void ShowBattleResult(int eventNo, int IndexNo)
    {
        BattleMode = true;
        BattleNextEvent = eventNo;
        BattleNextIndex = IndexNo;

        EventManager.Instance.BattleResult();
    }
}
