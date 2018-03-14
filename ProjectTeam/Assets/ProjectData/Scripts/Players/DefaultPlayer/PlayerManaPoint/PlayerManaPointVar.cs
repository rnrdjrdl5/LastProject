using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerManaPoint
{

    private float MaxManaPoint = 100.0f;

    public void SetMaxManaPoint(float MP) { MaxManaPoint = MP; }
    public float GetMaxManaPoint() { return MaxManaPoint; }

    private float NowManaPoint = 100.0f;

    public void SetNowManaPoint(float MP) { NowManaPoint = MP; }
    public float GetNowManaPoint() { return NowManaPoint; }

    private bool isReTimeMana = true;
    
    public void SetisReTimeMana(bool s) { isReTimeMana = s; }
    public bool GetisReTimeMana() { return isReTimeMana; }


    [Header(" - 마나 리젠속도")]
    [Tooltip(" - 마나 리젠속도입니다. ")]
    public float ManaRetimePoint = 5.0f;


    // 마나 감소 속도  , 외부로부터 받아옵니다. 
    private float ManaDecreasePoint = 0.0f;

    public void SetManaDecreasePoint(float MP) { ManaDecreasePoint = MP; }
    public float GetManaDecreasePoint() { return ManaDecreasePoint; }



}
