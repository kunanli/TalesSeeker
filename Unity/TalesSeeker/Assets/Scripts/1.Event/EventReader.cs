using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventReader : MonoBehaviour {

    #region field
    public int textStep = 0;

    public int textStepMax = 1;
    #endregion

    #region UIComponent
    public Image EventBG;
    public Image EventPic;

    public Text EventText;
    public Text EventChoiceTextLeft;
    public Text EventChoiceTextRight;
    #endregion


    public void SetNextEvent(baseEventData _event)
    {
        EventPic.sprite = Resources.Load<Sprite>(_event.bgPath);

        EventText.text = _event.text[0];
        textStepMax = _event.text.Length;

        EventChoiceTextLeft.text    = _event.eventChoices.ChoiceLeft.choiceText;
        EventChoiceTextRight.text   = _event.eventChoices.ChoiceRight.choiceText;
    }
}
