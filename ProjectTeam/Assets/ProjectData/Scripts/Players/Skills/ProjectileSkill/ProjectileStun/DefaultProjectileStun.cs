using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultProjectileStun : DefaultProjectileSkill {
    [Header(" - 스턴 속성")]
    [Tooltip(" - 스턴 지속시간입니다.")]
    public float StunTime = 1.0f;



    protected override void SetCollisionData(GameObject CGO, DefaultProjectileSkill DPS)
    {

        base.SetCollisionData(CGO, DPS);

        if (CGO.GetComponent<CollisionStunDebuff>() != null)
        {
            CGO.GetComponent<CollisionStunDebuff>().SetMaxTime(StunTime);
        }

    }
}
