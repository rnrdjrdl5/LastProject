using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMeleeGroggy : DefaultMeleeAttack
{

    [Header(" - 피격 설정")]
    [Tooltip(" - 피격 지속시간")]
    public float GroggyTime = 0.0f;


    override protected void Update()
    {
        base.Update();
    }
}
