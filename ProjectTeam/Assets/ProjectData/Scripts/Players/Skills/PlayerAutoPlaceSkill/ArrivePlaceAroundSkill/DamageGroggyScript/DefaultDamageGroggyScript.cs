using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultDamageGroggyScript : DefaultArrivePlaceAroundSkill
{
    [Header(" - 오브젝트")]
    [Tooltip(" - 충돌체의 FBX파일입니다. ")]
    public GameObject BulletPrefab;

    [Tooltip(" - 충돌체의 크기 입니다.")]

    public float AroundObjectRadius = 3.0f;


    [Header(" - 스킬 속성")]

    [Tooltip(" - 충돌체의 데미지입니다.")]
    public float ObjectDamage = 10.0f;
    [Tooltip(" - 충돌체의 데미지 충돌 횟수입니다.")]
    public float ObjectDamageNumber = 10.0f;


    [Tooltip(" - 충돌체의 충돌체크 재사용 시간입니다.")]
    public float ReCheckTime = 1000f;

    [Header(" - 경직 속성")]
    [Tooltip(" - 경직 지속시간 입니다")]
    public float GroggyTime = 2.0f;




    [Tooltip(" - 충돌체의 자연  소멸 시간입니다.")]
    public float DestroyTime = 3.0f;



    virtual protected void SetCollisionData(GameObject CGO, DefaultDamageGroggyScript DPS)
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

        if (CGO.GetComponent<CollisionDamagedDebuff>() != null)
        {
            CGO.GetComponent<CollisionDamagedDebuff>().SetMaxTime(GroggyTime);
        }

    }
}
