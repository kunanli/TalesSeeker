using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Runtime.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Random = UnityEngine.Random;

#if UNITY_EDITOR
[CustomEditor(typeof(baseEventData))]
public class baseEventDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //not so important ,do it late
        base.OnInspectorGUI();
        /*
        var myScript = target as baseEventData;

        myScript.eventChoices.ChoiceLeft.orderNextIndex =
            EditorGUILayout.Toggle("orderNextIndex", myScript.eventChoices.ChoiceLeft.orderNextIndex);

        using (var group =
            new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.eventChoices.ChoiceLeft.orderNextIndex)))
        {
            if (group.visible == true)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PrefixLabel("Text");
                myScript.eventChoices.ChoiceLeft.orderIndexNo =
                    EditorGUILayout.IntField(myScript.eventChoices.ChoiceLeft.orderIndexNo);
            }
        }*/
    }
}

#endif

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
    public int EventNo;

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
    [DataMember,Multiline]
    public string[] text;

    /// <summary>
    /// story or text
    /// </summary>
    [DataMember]
    public string cardName;

    [DataMember]
    public EventChoice eventChoices;

    [DataMember]
    public List<int> needIndexNo;

    [DataMember]
    public bool OnlyOneEvent;
}

[System.Serializable]
public class EventChoice
{
    [System.Serializable]
    public class EventChoiceResult
    {
        [System.Serializable]
        public class RandomIndexSetting
        {
            public int indexNo;

            public float randomWeight;
        }

        public string choiceText;
        public float enemyDmg;

        public float karma;

        public float money;

        /// <summary>
        /// Order Next Event ,if false will random
        /// </summary>
        public bool orderNextEvent;

        public int orderEventNo = -1;

        /// <summary>
        /// Order Next Index 
        /// </summary>
        public bool orderNextIndex;

        public int orderIndexNo = -1;

        public bool randomNextIndex;
        public List<RandomIndexSetting> randomIndexNo = new List<RandomIndexSetting>();

        [DataMember]
        public bool ShowBattleResult;

        [DataMember]
        public bool ForceDie;
        [DataMember]
        public EventDataManager.DieType ForceDieSetting;

        [DataMember]
        public bool useNotification;
        [DataMember]
        public NotificationData NotificationData;
    }

    public EventChoiceResult ChoiceLeft;

    public EventChoiceResult ChoiceRight;
}

[Serializable]
public class NotificationData
{
    [DataMember]
    public EventDataManager.NotificationType NotificationType;
    [DataMember]
    public int NotificationParam;

    [DataMember,Multiline]
    public string NotificationText;
}

