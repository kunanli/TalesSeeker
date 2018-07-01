using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GoldControl : MonoBehaviour {


    public Sprite[] GoldSprites;

    public Image DigitHundredImage;
    public Image DigitTenImage;
    public Image DigitOneImage;

    public void setMoney(int gold)
    {
        if (gold < 10)
        {
            DigitHundredImage.sprite = GoldSprites[0];
            DigitTenImage.sprite = GoldSprites[0];
            DigitOneImage.sprite = GoldSprites[gold];
        }
        else if(gold < 100)
        {
            var digitTen = gold / 10;
            var digitOne = gold % 10;
            DigitHundredImage.sprite = GoldSprites[0];
            DigitTenImage.sprite = GoldSprites[digitTen];
            DigitOneImage.sprite = GoldSprites[digitOne];
        }
        else
        {
            var digitHundred = gold / 100;
            var digitTen = (gold % 100) / 10;
            var digitOne = (gold % 100) % 10;
            DigitHundredImage.sprite = GoldSprites[digitHundred];
            DigitTenImage.sprite = GoldSprites[digitTen];
            DigitOneImage.sprite = GoldSprites[digitOne];
        }
    }
}
