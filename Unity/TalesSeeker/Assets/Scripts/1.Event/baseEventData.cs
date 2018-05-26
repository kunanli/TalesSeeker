using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

public class baseEventData : MonoBehaviour
{
    /// <summary>
    /// temp category
    /// </summary>
    public enum EventCategory
    {
        Village,
        Castle,
        Field,
        Legendary
    }

    /// <summary>
    /// Event No.
    /// </summary>
    [DataMember]
    public int indexNo;
    /// <summary>
    /// category
    /// </summary>
    [DataMember]
    public EventCategory category;
    /// <summary>
    /// pic
    /// </summary>
    [DataMember]
    public string picPath;
    /// <summary>
    /// bg
    /// </summary>
    [DataMember]
    public string bgPath;
    /// <summary>
    /// story or text
    /// </summary>
    [DataMember]
    public string[] text;

    [DataMember]
    public EventChoice eventChoices;
}

[System.Serializable]
public class EventChoice
{
    [System.Serializable]
    public class EventChoiceResult
    {
        public string choiceText;
        public float enemyDmg;

        /// <summary>
        /// Order Next Event ,if false will random
        /// </summary>
        public bool orderNextEvent;

        public int orderEventNo = -1;
    }

    public EventChoiceResult ChoiceLeft;

    public EventChoiceResult ChoiceRight;
}
