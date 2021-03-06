﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 코더 : 반재억
// 제작일 : 2018. 02. 22
// 사용목적 : 실질적인 충돌체크를 사용합니다.

// 모든 충돌체크에 대한 판정을 가지고 있습니다.
// 충돌체크 추가 시 이 스크립트에서 추가합니다.

//사용하는 곳 : PlayerCheckCollision 
// PlayerCheckCollision : 플레이어 충돌체크 컴포넌트.


public class BaseCollision : Photon.PunBehaviour{

    private MathUtility mathUtility;

    

    private PlayerHealth playerHealth;              // 플레이어 체력 스크립트

    private PlayerState playerState;                // 플레이어 상태

    private Animator animator;              // 애니메이터 

    private CollisionObject collisionObject;            // 충돌 오브젝트 스크립트
    private CollisionObjectDamage collisionObjectDamage;
    private NumberOfCollisions numberOfCollisions;
    private CollisionAnimator collisionAnimator;        // 애니메이션 여부를 결정지음.

    CollisionNotMoveDebuff collisionNotMoveDebuff;
    CollisionStunDebuff collisionStunDebuff;
    CollisionDamagedDebuff collisionDamagedDebuff;
    CollisionGroggyDebuff collisionGroggyDebuff;

    private void Awake()
    {
        mathUtility = new MathUtility();

        // 플레이어 체력 스크립트 받기
        playerHealth = gameObject.GetComponent<PlayerHealth>();

        // 플레이어 상태 받기
        playerState = gameObject.GetComponent<PlayerState>();
        
        // 애니메이터 받기
        animator = gameObject.GetComponent<Animator>();
    }



    public void UseCollision(Collider other)
    {

        
        // collisionObject 받아오기
        collisionObject = other.gameObject.GetComponent<CollisionObject>();


        // 성공적으로 받음
        if (collisionObject != null)
        {
            // 충돌체 주인 확인
            if (collisionObject.GetUsePlayer() != "Player" + photonView.viewID)
            {

                // 충돌 된적 없는 경우
                if (!ReCheck(other))
                {
                    Debug.Log("충돌실시");
                    // 데미지 체크
                    LeftApplyDamage(other.gameObject , false);

                    // 디버프 체크
                    LeftDebuff(other.gameObject, false);

                    // 최대 충돌횟수 체크
                    LeftNumberOfCollision(other.gameObject , false);

                    // 애니메이션 여부 판단해서 재생함.
                    LeftAnimation(other.gameObject,false);
                }
            }
        }
    }

    private bool ReCheck(Collider other)
    {

        // ReCheck 스크립트 받아옴
        CollisionReCheck[] CRCs = other.gameObject.GetComponents<CollisionReCheck>();

        // 충돌 이미 있는지 판단
        foreach (CollisionReCheck crc in CRCs)
        {

            if (crc.GetPlayerObject() == gameObject)
                return true;
        }

        // 충돌 대기시간 추가
        CollisionReCheck CRC = other.gameObject.AddComponent<CollisionReCheck>();

        // 충돌대상 등록
        CRC.SetPlayerObject(gameObject);

        // 충돌 재사용 대기시작 등록
        CRC.SetPlayerReCheckTime(collisionObject.GetCollisionReCheckTime());

        return false;
    }

    private void LeftNumberOfCollision(GameObject go, bool isItem)
    {

        // 충돌 횟수 받아옴
        numberOfCollisions = go.GetComponent<NumberOfCollisions>();
        collisionAnimator = go.GetComponent<CollisionAnimator>();


        if (numberOfCollisions == null)
            return;

        // 감소
        numberOfCollisions.DeCreaseNumberOfCollisions();


        if (!isItem)
        {
            if (collisionObject.PlayerIOwnerID != PhotonNetwork.player.ID)
                return;
        }


        // 충돌횟수 끝나면 삭제
        // 애니메이션 용 충돌이 아니라면.
        if (numberOfCollisions.GetNumberOfCollisions() == 0 &&
            collisionAnimator == null)
        {

            Debug.Log("ID : " + go.GetComponent<ObjectIDScript>().ID);

            photonView.RPC("RPCPushObjectPool", PhotonTargets.All, go.GetComponent<ObjectIDScript>().ID);
        }

        // 단 오브젝트 풀링은 애니메이션일 경우 알아서 처리함.

    }
    

    private void LeftApplyDamage(GameObject go, bool isItem)
    {

        // 받았는지 체크
        if (collisionObject == null)
            return;

        // 충돌 데미지 받아옴
        collisionObjectDamage = go.gameObject.GetComponent<CollisionObjectDamage>();


        if (!isItem)
        {
            if (collisionObject.PlayerIOwnerID != PhotonNetwork.player.ID)
                return;
        }

        // 공격한 사람 클라이언트에서 처리함

        // 데미지 주기
        playerHealth.CallApplyDamage(collisionObjectDamage.GetObjectDamage());
        Debug.Log("데미지 : " + collisionObjectDamage.GetObjectDamage());

        // 이펙트 전송
        photonView.RPC("RPCCreateEffect", PhotonTargets.All, (int)collisionObjectDamage.EffectType);






        // 아래는 개인이 처리합니다.


        // 데미지 충돌횟수 감소
        collisionObjectDamage.DecreaseObjectDamageNumber();

        // 데미지 충돌횟수 확인
        if (collisionObjectDamage.GetObjectDamageNumber() == 0)
        {

        }

    
    }

    private void LeftDebuff(GameObject go, bool isItem)
    {

        if (!isItem)
        {
            if (PhotonNetwork.player.ID != collisionObject.PlayerIOwnerID)
                return;
        }

        collisionNotMoveDebuff = go.GetComponent<CollisionNotMoveDebuff>();
        collisionStunDebuff = go.GetComponent<CollisionStunDebuff>();
        collisionDamagedDebuff = go.GetComponent<CollisionDamagedDebuff>();
        collisionGroggyDebuff = go.GetComponent<CollisionGroggyDebuff>();


        if (collisionNotMoveDebuff != null)
        {
            photonView.RPC("RPCNotMoveDebuff", PhotonTargets.All, collisionNotMoveDebuff.GetMaxTime());
        }

        if (collisionStunDebuff != null)
        {
            photonView.RPC("RPCStunDebuff", PhotonTargets.All, collisionStunDebuff.GetMaxTime());
        }

        if (collisionDamagedDebuff != null)
        {
            MathUtility.EnumDirVector DirVectorType;

            if (collisionObject.UsePlayerObject != null)
                DirVectorType = mathUtility.VectorDirType(gameObject, collisionObject.UsePlayerObject.transform.position);

            else
            {
                DirVectorType = mathUtility.VectorDirType(gameObject, go.transform.position);
                Debug.Log("공격 대상을 찾을 수 없음. 물체의 위치에 따라피격을 정해줌.");
            }





            photonView.RPC("RPCDamagedDebuff", PhotonTargets.All, (int)DirVectorType);
        }

        if (collisionGroggyDebuff != null)
        {
            photonView.RPC("RPCGroggyDebuff", PhotonTargets.All, collisionGroggyDebuff.GetMaxTime());
        }


    }

    private void LeftAnimation(GameObject go ,bool isItem)
    {

        if (collisionAnimator == null)
            return;

        if (!isItem)
        {
            if (PhotonNetwork.player.ID != collisionObject.PlayerIOwnerID)
                return;
        }

        photonView.RPC("RPCSetAnimationMode", PhotonTargets.All , collisionObject.GetComponent<ObjectIDScript>().ID);
    }


    // 아이템 전용 충돌체크

    public void UseItemCollision(Collider other)
    {
        Debug.Log("1");
        collisionObject = other.GetComponent<CollisionObject>();
        if (collisionObject == null)
            return;
        Debug.Log("2");
        if (!photonView.isMine)
            return;
        Debug.Log("3");
        if (collisionObject.GetUsePlayer() == "Player" + photonView.viewID)
            return;
        Debug.Log("4");
        if (ReCheck(other))
            return;
        Debug.Log("5");
        photonView.RPC("RPCCheckUseItem", PhotonTargets.All, collisionObject.PlayerIOwnerID, other.gameObject.GetComponent<ObjectIDScript>().ID);

        


    }

    public void ActiveItem(GameObject go)
    {


        // 데미지 체크
        LeftApplyDamage(go , true);

        // 디버프 체크
        LeftDebuff(go , true);

        // 최대 충돌횟수 체크
        LeftNumberOfCollision(go , true);

        // 애니메이션 여부 판단해서 재생함.
        LeftAnimation(go , true);
    }








    /************* RPC입니다. ****************/


    [PunRPC]
    private void RPCNotMoveDebuff(float MDT)
    {

        animator.SetBool("isNotMove", true);
        // 속박 받아옴
        PlayerNotMoveDebuff playerNotMoveDebuff = gameObject.GetComponent<PlayerNotMoveDebuff>();

        // 속박 없으면 새로 추가
        if (playerNotMoveDebuff == null)
        {
            playerNotMoveDebuff = gameObject.AddComponent<PlayerNotMoveDebuff>();

        }

        // 속박 설정 추가
        playerNotMoveDebuff.SetMaxDebuffTime(MDT);
        playerNotMoveDebuff.SetNowDebuffTime(0);

        // 플레이어 상태에 속박상태라 알림
       // playerState.SetplayerNotMoveDebuff(playerNotMoveDebuff);
    }

    [PunRPC]
    private void RPCStunDebuff(float ST)
    {
        
        // 스턴 애니메이션 재생
        animator.SetBool("StunOnOff", true);

        // 스턴 디버프 받아오기
        PlayerStunDebuff playerStunDebuff = gameObject.GetComponent<PlayerStunDebuff>();

        // 스턴 없으면 새로 추가
        if (playerStunDebuff == null)
        {
            playerStunDebuff = gameObject.AddComponent<PlayerStunDebuff>();
        }

        // 스턴 속성 설정
        playerStunDebuff.SetMaxDebuffTime(ST);
        playerStunDebuff.SetNowDebuffTime(0);

    }


    // 해당 디버프는 따로 시간을 지정할 수 없음. 애니메이션 시간을 지정해야함.
    [PunRPC]
    private void RPCDamagedDebuff(int DirVectorType)
    {

        if ((MathUtility.EnumDirVector)DirVectorType == MathUtility.EnumDirVector.UP)
        {
            animator.SetInteger("DamagedType", 3);
        }

        else if ((MathUtility.EnumDirVector)DirVectorType == MathUtility.EnumDirVector.DOWN)
        {
            animator.SetInteger("DamagedType", 4);
        }

        else if ((MathUtility.EnumDirVector)DirVectorType == MathUtility.EnumDirVector.LEFT)
        {
            animator.SetInteger("DamagedType", 2);
        }

        else if ((MathUtility.EnumDirVector)DirVectorType == MathUtility.EnumDirVector.RIGHT)
        {
            animator.SetInteger("DamagedType", 1);
        }
    }

    [PunRPC]
    private void RPCGroggyDebuff(float GD)
    {

        // Groggy 설정

        // 경직 애니메이션 재생
        animator.SetBool("isGroggy", true);

        // 경직 디버프 받아오기
        PlayerGroggyDebuff playerGroggyDebuff = gameObject.GetComponent<PlayerGroggyDebuff>();

        // 경직 없으면 새로 추가
        if (playerGroggyDebuff == null)
        {
            playerGroggyDebuff = gameObject.AddComponent<PlayerGroggyDebuff>();
        }

        // 경직 속성 설정
        playerGroggyDebuff.SetMaxDebuffTime(GD);
        playerGroggyDebuff.SetNowDebuffTime(0);
    }

    // 이펙트용 RPC
    [PunRPC]
    void RPCCreateEffect(int EffectType)
    {
        GameObject effect = PoolingManager.GetInstance().CreateEffect((PoolingManager.EffctType)EffectType);
        effect.transform.position = transform.position + Vector3.up * 0.5f;

        playerHealth.FlushEffect();

        SoundManager.GetInstance().PlayHitSound((PoolingManager.EffctType)EffectType);
    }



    // 오브젝트 풀에서 탐지하는 용도

        // 주의점
        // 동기화가 깨질 경우
        // 다른 클라이언트에서는 이 오브젝트를 찾지 못한다.

        // 즉 , 연결방법이 없어진다.
    [PunRPC]
    void RPCPushObjectPool(int ObjectID)
    {
        GameObject go = PoolingManager.GetInstance().FindObjectUseObjectID(ObjectID);

        // 오브젝트를 다시 push에 넣어준다.

        go.GetComponent<CollisionObject>().ResetSkillOption();
       //ResetSkillOption(go);

        if (go != null)
            PoolingManager.GetInstance().PushObject(go);
    }

    [PunRPC]
    void RPCSetAnimationMode(int ObjectID)
    {
        GameObject go = PoolingManager.GetInstance().FindObjectUseObjectID(ObjectID);
        if (go == null)
        {
            Debug.Log("에러거나, RPC늦어서 비활성화됨.");
            return;
        }


        collisionAnimator = go.GetComponent<CollisionAnimator>();
        if (collisionAnimator == null)
            return;

        collisionAnimator.SetAnimatorMode();
    }

    [PunRPC]
    void RPCCheckUseItem(int ownerID , int ObjectID)
    {
        Debug.Log("a");
        if (ownerID != PhotonNetwork.player.ID)
            return;

        GameObject go = PoolingManager.GetInstance().FindObjectUseObjectID(ObjectID);

        CollisionItem collisionItem = go.GetComponent<CollisionItem>();
        Debug.Log("b");
        if (collisionItem == null)
            return;

        if (collisionItem.IsUseItem)
            photonView.RPC("RPCReturnFail", PhotonTargets.All);
        // RPC
        else
        {
            photonView.RPC("RPCReturnUseItem", PhotonTargets.All, ObjectID);
            Debug.Log("c");
        }

    }

    [PunRPC]
    void RPCReturnUseItem(int ObjectID)
    {
        Debug.Log("!");
        if (!photonView.isMine)
        {
            Debug.Log("본인아님");
            return;
        }

        GameObject go = PoolingManager.GetInstance().FindObjectUseObjectID(ObjectID);

        if (go == null)
        {
            Debug.Log("못찾았거나, 설치형 스킬 밟는 도중 RPC 시간이 늦어져서 사라졌거나. ");
            return;
        }

        ActiveItem(go);
    }

    [PunRPC]
    void RPCReturnFail()
    {
        Debug.Log("실패");
    }
}