using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerManaPoint
{
    void ReTimeMana()
    {
        if(isReTimeMana)
        {
            if(NowManaPoint < 100)
            {
                NowManaPoint += Time.deltaTime * ManaRetimePoint;
                if(NowManaPoint >= 100)
                {
                    ManaRetimePoint = 100;
                }
            }
        }
    }

    void DecreaseMana()
    {
        if(!isReTimeMana)
        {
            if(NowManaPoint>0)
            {
                NowManaPoint -= Time.deltaTime * ManaDecreasePoint;
                if(NowManaPoint <= 0)
                {
                    NowManaPoint = 0;
                }
            }
        }
    }
}
