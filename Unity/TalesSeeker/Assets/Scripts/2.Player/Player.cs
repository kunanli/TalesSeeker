using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [System.Serializable]
    public class PlayerParam
    {
        public float Maxhp;
        public float Maxmp;

        public float hp = 1000;
        public float mp = 1000;
    }

    public PlayerParam playerParam = new PlayerParam();

	// Use this for initialization
	void Start()
    {
        playerParam.Maxhp = playerParam.hp;
        playerParam.Maxmp = playerParam.mp;
    }
	
	// Update is called once per frame
	void Update() {
		
	}
}
