using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainControl : baseGUI
{

    public float FadeInTime;

    public override void Start()
    {
        base.Start();

        //init
        RequestFadeIn(FadeInTime);
    }

    public override void Update()
    {
        base.Update();



    }

    public void OnMouseClick()
    {
        SceneManager.LoadScene(2);
    }
}
