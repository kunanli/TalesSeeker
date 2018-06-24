using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KarmaControl : MonoBehaviour
{

    public Sprite[] KarmaSprites;

    public Image UiImage;

    void Start()
    {
        UiImage = gameObject.GetComponent<Image>();
    }

    void Update()
    {

    }

    public void SetValue(float value)
    {
        if (value == 1.0f)
        {
            UiImage.sprite = KarmaSprites[0];
        }
        else if (value > 0.9f)
        {
            UiImage.sprite = KarmaSprites[1];
        }
        else if (value > 0.8f)
        {
            UiImage.sprite = KarmaSprites[2];
        }
        else if (value > 0.7f)
        {
            UiImage.sprite = KarmaSprites[3];
        }
        else if (value > 0.6f)
        {
            UiImage.sprite = KarmaSprites[4];
        }
        else if (value > 0.5f)
        {
            UiImage.sprite = KarmaSprites[5];
        }
        else if (value > 0.4f)
        {
            UiImage.sprite = KarmaSprites[6];
        }
        else if (value > 0.3f)
        {
            UiImage.sprite = KarmaSprites[7];
        }
        else if (value > 0.2f)
        {
            UiImage.sprite = KarmaSprites[8];
        }
        else if (value > 0)
        {
            UiImage.sprite = KarmaSprites[9];
        }
        else if (value == 0.0f)
        {
            UiImage.sprite = KarmaSprites[10];
        }
    }
}
