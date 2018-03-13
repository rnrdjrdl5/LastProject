using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultDefaultProjectileNotMove : DefaultProjectileSkill
{
    [Header(" - 이동불가 속성")]
    [Tooltip(" - 이동불가 지속시간입니다.")]
    public float NotMoveTime = 3.0f;


    protected override void SetCollisionData(GameObject CGO, DefaultProjectileSkill DPS)
    {

        base.SetCollisionData(CGO, DPS);

        if (CGO.GetComponent<CollisionNotMoveDebuff>() != null)
        {
            CGO.GetComponent<CollisionNotMoveDebuff>().SetMaxTime(NotMoveTime);

        }

    }

    protected override void Update()
    {
        base.Update();
    }
}
