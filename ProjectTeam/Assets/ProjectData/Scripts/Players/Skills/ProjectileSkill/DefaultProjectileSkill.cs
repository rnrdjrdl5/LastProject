using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultProjectileSkill : DefaultSkill {
    // 인스펙터에서 지정하는 변수들 입니다.

    

    [Header(" - 오브젝트")]
    [Tooltip(" - 투사체의 FBX파일입니다. ")]
    public GameObject BulletPrefab;

    [Header(" - 스킬 속성")]
    [Tooltip(" - 투사체 데미지입니다.")]
    public float ObjectDamage = 10.0f;
    [Tooltip(" - 투사체 데미지 충돌 횟수입니다.")]
    public float ObjectDamageNumber = 10.0f;

    [Tooltip(" - 투사체 충돌 제한 횟수입니다.")]
    public float CollisionNumber = 1.0f;

    [Tooltip(" - 투사체 충돌체크 재사용 시간입니다.")]
    public float ReCheckTime = 1000f;

    [Tooltip(" - 투사체의 자연  소멸 시간입니다.")]
    public float DestroyTime = 3.0f;

    [Tooltip(" - 투사체의 이동속도입니다. 1초당 n 만큼의 속도로 이동합니다.")]
    public float ObjectSpeed = 3.0f;

    


    virtual protected void SetCollisionData(GameObject CGO, DefaultProjectileSkill DPS)
    {
        if (CGO.GetComponent<CollisionObject>() != null)
        {
            CGO.GetComponent<CollisionObject>().SetCollisionReCheckTime(DPS.ReCheckTime);
            CGO.GetComponent<CollisionObject>().SetUsePlayer("Player" + gameObject.GetComponent<PhotonView>().viewID);
        }

        if (CGO.GetComponent<CollisionObjectDamage>() != null)
        {
            CGO.GetComponent<CollisionObjectDamage>().SetObjectDamage(DPS.ObjectDamage);
            CGO.GetComponent<CollisionObjectDamage>().SetObjectDamageNumber(DPS.ObjectDamageNumber);
        }

        if (CGO.GetComponent<CollisionObjectTime>() != null)
        {
            CGO.GetComponent<CollisionObjectTime>().SetObjectTime(DPS.DestroyTime);
        }

        if (CGO.GetComponent<NumberOfCollisions>() != null)
        {
            CGO.GetComponent<NumberOfCollisions>().SetNumberOfCollisions(DPS.CollisionNumber);
        }

        if (CGO.GetComponent<CollisionMove>() != null)
        {
            CGO.GetComponent<CollisionMove>().SetCollisionMoveSpeed(DPS.ObjectSpeed);
            CGO.GetComponent<CollisionMove>().SetCollisionMoveDirect(DPS.transform.forward.normalized);
        }

    }





    override protected void Update()
    {
        base.Update();
    }


}
