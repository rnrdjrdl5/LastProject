using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultProjectilePushGroggy : DefaultProjectileSkill
{
    [Header(" - 밀쳐내기 속성")]
    [Tooltip(" - 밀쳐내기 스피드입니다.")]
    public float MovePushSpeed = 3.0f;

    [Tooltip(" - 밀쳐내기 지속시간 입니다.")]
    public float MovePushTime = 2.0f;

    [Header(" - 경직 속성")]
    [Tooltip(" - 경직 지속시간 입니다. * 밀쳐내기와 동일하게 해주세요.")]
    public float GroggyTime = 2.0f;


    private Vector3 MoveDirection;

    public void SetMoveDirection(Vector3 MD) { MoveDirection = MD; }
    public Vector3 GetMoveDirection() { return MoveDirection; }
    



    protected override void SetCollisionData(GameObject CGO, DefaultProjectileSkill DPS)
    {
        
        base.SetCollisionData(CGO, DPS);

        if (CGO.GetComponent<CollisionPushDebuff>() != null)
        {
            CGO.GetComponent<CollisionPushDebuff>().SetMaxTime(MovePushTime);
            CGO.GetComponent<CollisionPushDebuff>().SetMoveSpeed(MovePushSpeed);

            // 밀려나가는 거리입니다.
            CGO.GetComponent<CollisionPushDebuff>().SetMoveDirection(CGO.GetComponent<CollisionMove>().GetCollisionMoveDirect());
            
        }

        if(CGO.GetComponent<CollisionDamagedDebuff>()!=null)
        {
            CGO.GetComponent<CollisionDamagedDebuff>().SetMaxTime(GroggyTime);
        }

    }

    override protected void Update()
    {
        base.Update();
    }
}
