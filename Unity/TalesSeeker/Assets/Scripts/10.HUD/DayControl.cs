using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayControl : MonoBehaviour {

    public Sprite[] DaySprites;

    public Image DigitTenImage;
    public Image DigitOneImage;

    public int Days;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddDay()
    {
        Days += 1;

        if (Days < 10)
        {
            DigitTenImage.sprite = DaySprites[0];
            DigitOneImage.sprite = DaySprites[Days];
        }
        else
        {
            var digitTen = Days / 10;
            var digitOne = Days % 10;
            DigitTenImage.sprite = DaySprites[digitTen];
            DigitOneImage.sprite = DaySprites[digitOne];
        }
    }

    public void AddDay(int day)
    {
        Days += day;
        if (Days < 10)
        {
            DigitTenImage.sprite = DaySprites[0];
            DigitOneImage.sprite = DaySprites[Days];
        }
        else
        {
            var digitTen = Days / 10;
            var digitOne = Days % 10;
            DigitTenImage.sprite = DaySprites[digitTen];
            DigitOneImage.sprite = DaySprites[digitOne];
        }
    }
}
