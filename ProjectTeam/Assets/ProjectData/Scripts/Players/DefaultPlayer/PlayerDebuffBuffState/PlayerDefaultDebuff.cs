using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultDebuff : MonoBehaviour {

    


    protected float MaxDebuffTime;



    public void SetMaxDebuffTime(float MDT) { MaxDebuffTime = MDT; }
    public float GetMaxDebuffTime() { return MaxDebuffTime; }


    protected float NowDebuffTime;

    public void SetNowDebuffTime(float NDT) { NowDebuffTime = NDT; }
    public float GetNowDebuffTime() { return NowDebuffTime; }


    // Use this for initialization
    void Start () {
        NowDebuffTime = 0.0f;
        
    }
	
	// Update is called once per frame
	virtual protected void Update () {
        if (MaxDebuffTime <= NowDebuffTime)
        {
            ExitDebuff();
            Destroy(this);
        }
        else
            NowDebuffTime += Time.deltaTime;
	}

    protected virtual void ExitDebuff()
    {

    }
}
