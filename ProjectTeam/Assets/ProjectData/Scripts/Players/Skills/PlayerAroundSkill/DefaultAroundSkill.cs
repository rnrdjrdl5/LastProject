using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAroundSkill : DefaultSkill {

    // Use this for initialization

    [Header(" - 오브젝트")]
    [Tooltip(" - 충돌체의 FBX파일입니다. ")]
    public GameObject AroundObject;
    [Tooltip(" - 오브젝트 파일의 범위입니다.  scale로 정합니다.")]
    public float AroundObjectRadius = 3.0f;


    [Header(" - 스킬 속성")]
    [Tooltip(" - 충돌체의 데미지입니다.")]
    public float ObjectDamage = 10.0f;
    [Tooltip(" - 충돌체의 데미지 충돌 횟수입니다.")]
    public float ObjectDamageNumber = 10.0f;

    [Tooltip(" - 충돌체의 충돌 제한 횟수입니다.")]
    public float CollisionNumber = 1.0f;

    [Tooltip(" - 충돌체의 충돌체크 재사용 시간입니다.")]
    public float ReCheckTime = 1000f;

    virtual protected void SetCollisionData(GameObject CGO, DefaultAroundSkill DPS)
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


        if (CGO.GetComponent<NumberOfCollisions>() != null)
        {
            CGO.GetComponent<NumberOfCollisions>().SetNumberOfCollisions(DPS.CollisionNumber);
        }

    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Awake()
    {
        base.Awake();
    }




    // Update is called once per frame

}
