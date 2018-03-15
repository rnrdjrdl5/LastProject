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

    public void DecreaseMana(float MP)
    {
        NowManaPoint -= MP;
        if (NowManaPoint < 0)
            NowManaPoint = 0;
    }


    [Header(" - 마나 리젠속도")]
    [Tooltip(" - 마나 리젠속도입니다. ")]
    public float ManaRetimePoint = 5.0f;




}
