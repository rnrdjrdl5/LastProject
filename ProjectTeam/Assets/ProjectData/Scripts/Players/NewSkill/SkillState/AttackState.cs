﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackState{

    [Header(" - 데미지")]
    [Tooltip(" - 프라이팬의 데미지입니다.")]
    public float AttackDamage;

    [Tooltip(" - 프라이팬의 데미지 충돌 횟수입니다.")]
    public float DamageNumber;

    [Tooltip(" - 같은 대상에게 재충돌체크 제한 시간입니다.")]
    public float RecheckTime;

    [Tooltip(" - 충돌체크 이펙트 입니다.")]
    public PoolingManager.EffctType effectType;


    // 1. 이전에 스크립트를 받은 적이 없을 때
    public void SetData(GameObject CollisionGameObject, PhotonView pv, int ID)
    {
        // 1. 무기로부터 각종 정보를 받습니다.
        CollisionObjectDamage collisionObjectDamage = 
            CollisionGameObject.GetComponent<CollisionObjectDamage>();

        CollisionObject collisionObject =
            CollisionGameObject.GetComponent<CollisionObject>();

        InitData(collisionObjectDamage, collisionObject, pv, ID);

    }



    // 2. 이전에 스크립트를 받은 적이 있을 때
    public void SetData(CollisionObjectDamage cod,
        CollisionObject co,
        PhotonView pv, int ID)

    {
        InitData(cod, co, pv,ID);
    }



    // 받은 데이터를 기반으로 스크립트에 데미지를 넣습니다.
    private void InitData(CollisionObjectDamage cod,
        CollisionObject co,
        PhotonView pv , int ID)

    {
        cod.SetObjectDamage(AttackDamage);
        cod.SetObjectDamageNumber(DamageNumber);
        cod.EffectType = effectType;

        co.SetUsePlayer("Player" + pv.viewID);
        co.SetCollisionReCheckTime(RecheckTime);

        if(pv.isMine)
            co.PlayerIOwnerID = ID;

        
    }
}
