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
}
