using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EventReader : MonoBehaviour {

    public enum ChoiceType{
        Left,
        Right,
    }

    public bool CheckNotification()
    {
        return preNotificationData != null;
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

    //
    public Text EventNotification;

    //
    public Image[] Equiment;

    /// <summary>
    /// last time choice NotificationData
    /// </summary>
    private NotificationData preNotificationData;
    #endregion
    public GUIEvent guiEvent;
    Sprite Pic;

    private Player player;

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

            if (noEventData.category == baseEventData.EventCategory.SystemDie)
            {
                EventBG.Alpha(0);
            }
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

            EventBG.Alpha(1);

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
        if (noEventData.category == baseEventData.EventCategory.SystemDie)
        {
            SceneManager.LoadScene(1);
            return;
        }

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
                    if (choicesData.ChoiceLeft.ShowBattleResult)
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
                    if (choicesData.ChoiceLeft.ShowBattleResult)
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
                    if (choicesData.ChoiceLeft.ShowBattleResult)
                    {
                        ShowBattleResult(noEventData.EventNo, 1);
                    }
                    else
                    {
                        EventManager.Instance.Next(choicesData.ChoiceLeft.orderEventNo, 1);
                    }
                }

                if (choicesData.ChoiceLeft.useNotification)
                { 
                    preNotificationData = choicesData.ChoiceLeft.NotificationData;
                    EventNotification.text = preNotificationData.NotificationText;
                }
                else
                {
                    preNotificationData = null;
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
                    if (choicesData.ChoiceRight.ShowBattleResult)
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
                    if (choicesData.ChoiceRight.ShowBattleResult)
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
                    if (choicesData.ChoiceRight.ShowBattleResult)
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


                if (choicesData.ChoiceRight.useNotification)
                {
                    preNotificationData = choicesData.ChoiceRight.NotificationData;
                    EventNotification.text = preNotificationData.NotificationText;
                }
                else
                {
                    preNotificationData = null;
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
        player = GameManager.Instance.MainPlayer;
        var hp = player.playerParam.hp + info.enemyDmg;

        if (hp < player.playerParam.Maxhp)
        {
            if (hp < 0)
            {
                player.playerParam.hp = 0;
                HP.value = 0;
            }
            else
            {
                player.playerParam.hp = hp;
                HP.value = player.playerParam.hp / player.playerParam.Maxhp;
            }
        }
        else
        {
            player.playerParam.hp = player.playerParam.Maxhp;
            HP.value = 1;
        }

        player.playerParam.mp -= 20;
        MP.value = player.playerParam.mp / player.playerParam.Maxmp;
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

    public void doNotification()
    {
        switch (preNotificationData.NotificationType)
        {
            case EventDataManager.NotificationType.Hp:
                player.playerParam.hp = preNotificationData.NotificationParam;
                break;
            case EventDataManager.NotificationType.Sp:
                player.playerParam.mp = preNotificationData.NotificationParam;
                break;
            case EventDataManager.NotificationType.Item:
                player.playerParam.EquimentList.Add((baseItem.ItemID)preNotificationData.NotificationParam);
                setEquiment();
                break;
        }
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

    void setEquiment()
    {
        for(int i = 0; i < player.playerParam.EquimentList.Count ; i++)
        {
            var itemData = Equiment[i].GetComponent<baseItemData>();
            if (itemData.ID != player.playerParam.EquimentList[i])
            {
                itemData = ItemDataManager.Instance.ItemDataObject[(int) player.playerParam.EquimentList[i]];
                Equiment[i].sprite = itemData.gameObject.GetComponent<Image>().sprite;
            }
        }
    }
}
