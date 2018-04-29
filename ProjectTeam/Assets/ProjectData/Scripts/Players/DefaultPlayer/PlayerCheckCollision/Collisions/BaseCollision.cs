using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 코더 : 반재억
// 제작일 : 2018. 02. 22
// 사용목적 : 실질적인 충돌체크를 사용합니다.

// 모든 충돌체크에 대한 판정을 가지고 있습니다.
// 충돌체크 추가 시 이 스크립트에서 추가합니다.

//사용하는 곳 : PlayerCheckCollision 
// PlayerCheckCollision : 플레이어 충돌체크 컴포넌트.


 /************************************
  * 2018.04.04 갱신
  *  - 리팩토링 실시
  *  - GetComponent의 호출 수를 줄임
  ***********************************/

public class BaseCollision : Photon.PunBehaviour{

    private MathUtility mathUtility;

    

    private PlayerHealth playerHealth;              // 플레이어 체력 스크립트

    private PlayerState playerState;                // 플레이어 상태

    private Animator animator;              // 애니메이터 

    private CollisionObject collisionObject;            // 충돌 오브젝트 스크립트
    private CollisionObjectDamage collisionObjectDamage;
    private NumberOfCollisions numberOfCollisions;

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
                    // 데미지 체크
                    LeftApplyDamage(other);

                    // 디버프 체크
                    LeftDebuff(other);

                    // 최대 충돌횟수 체크
                    LeftNumberOfCollision(other);
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
            {

                // 있으면 true
                return true;
            }
        }

        // 충돌 대기시간 추가
        CollisionReCheck CRC = other.gameObject.AddComponent<CollisionReCheck>();

        // 충돌대상 등록
        CRC.SetPlayerObject(gameObject);

        // 충돌 재사용 대기시작 등록
        CRC.SetPlayerReCheckTime(collisionObject.GetCollisionReCheckTime());

        return false;
    }

    private void LeftNumberOfCollision(Collider other)
    {

        // 충돌 횟수 받아옴
        numberOfCollisions = other.gameObject.GetComponent<NumberOfCollisions>();

        // 충돌횟수 남았으면
        if (numberOfCollisions != null)
        {
            // 감소
            numberOfCollisions.DeCreaseNumberOfCollisions();


            // 오브젝트 주인만 사용
            if (collisionObject.PlayerIOwnerID == PhotonNetwork.player.ID)
            {
                // 충돌횟수 끝나면 삭제
                if (numberOfCollisions.GetNumberOfCollisions() == 0)
                {

                    // 어느 게임오브젝트를 삭제할 것인지 판단해야 함.

                    // 1. 해당 공격 오브젝트이름으로 오브젝트 풀링 오브젝트 탐지 

                    // 2. 탐지 성공 시 해당 오브젝트 집어넣기

                    Debug.Log("ID : " + other.gameObject.GetComponent<ObjectIDScript>().ID);

                    photonView.RPC("RPCPushObjectPool", PhotonTargets.All, other.gameObject.GetComponent<ObjectIDScript>().ID);
                                        
                }
            }


        }
    }

    private void LeftApplyDamage(Collider other)
    {
        // 충돌 데미지 받아옴
        collisionObjectDamage = other.gameObject.GetComponent<CollisionObjectDamage>();
        
        // 받았는지 체크
        if (collisionObjectDamage != null)
        {

            // 공격한 사람 클라이언트에서 처리함
           if(collisionObject.PlayerIOwnerID == PhotonNetwork.player.ID)  
            {

                // 데미지 주기
                playerHealth.CallApplyDamage(collisionObjectDamage.GetObjectDamage()) ;
                Debug.Log("데미지 : " + collisionObjectDamage.GetObjectDamage());

                // 이펙트 전송
                photonView.RPC("RPCCreateEffect", PhotonTargets.All , (int)collisionObjectDamage.EffectType);

               /* Debug.Log(PhotonNetwork.player.ID);
                Debug.Log(collisionObject.PlayerIOwnerID);*/



            }


           // 아래는 개인이 처리합니다.


            // 데미지 충돌횟수 감소
            collisionObjectDamage.DecreaseObjectDamageNumber();

            // 데미지 충돌횟수 확인
            if(collisionObjectDamage.GetObjectDamageNumber() == 0)
            {

            }

        }
    }

    private void LeftDebuff(Collider other)
    {

        // 마스터)
        //if (PhotonNetwork.isMasterClient)
        if(PhotonNetwork.player.ID == collisionObject.PlayerIOwnerID)
        {
            /*CollisionNotMoveDebuff collisionNotMoveDebuff = other.gameObject.GetComponent<CollisionNotMoveDebuff>();
            CollisionStunDebuff collisionStunDebuff = other.gameObject.GetComponent<CollisionStunDebuff>();
            CollisionDamagedDebuff collisionDamagedDebuff = other.gameObject.GetComponent<CollisionDamagedDebuff>();*/

            collisionNotMoveDebuff = other.gameObject.GetComponent<CollisionNotMoveDebuff>();
            collisionStunDebuff = other.gameObject.GetComponent<CollisionStunDebuff>();
            collisionDamagedDebuff = other.gameObject.GetComponent<CollisionDamagedDebuff>();
            collisionGroggyDebuff = other.gameObject.GetComponent<CollisionGroggyDebuff>();


            if (collisionNotMoveDebuff != null)
            {
                photonView.RPC("RPCNotMoveDebuff", PhotonTargets.All, collisionNotMoveDebuff.GetMaxTime());
            }

            if(collisionStunDebuff != null)
            {
                photonView.RPC("RPCStunDebuff", PhotonTargets.All, collisionStunDebuff.GetMaxTime());
            }

            if(collisionDamagedDebuff != null)
            {
                MathUtility.EnumDirVector DirVectorType;

                if (collisionObject.UsePlayerObject != null)
                    DirVectorType = mathUtility.VectorDirType(gameObject, collisionObject.UsePlayerObject.transform.position);

                else
                {
                    DirVectorType = mathUtility.VectorDirType(gameObject, other.gameObject.transform.position);
                    Debug.Log("공격 대상을 찾을 수 없음. 물체의 위치에 따라피격을 정해줌.");
                }





                photonView.RPC("RPCDamagedDebuff", PhotonTargets.All, (int)DirVectorType);
            }
            
            if (collisionGroggyDebuff != null)
            {
                photonView.RPC("RPCGroggyDebuff", PhotonTargets.All, collisionGroggyDebuff.GetMaxTime());
            }

        }
    }

    //private void ResetSkillOption(GameObject go)
    //{
    //    정보 초기화 필요
    //    if (collisionObject != null)
    //        collisionObject.ResetObject();

    //    if (collisionObjectDamage != null)
    //        collisionObjectDamage.ResetObject();

    //    if (numberOfCollisions != null)
    //        numberOfCollisions.ResetObject();



    //    if (collisionStunDebuff != null)
    //        Destroy(collisionStunDebuff);

    //    if (collisionNotMoveDebuff != null)
    //        Destroy(collisionNotMoveDebuff);

    //    if (collisionDamagedDebuff != null)
    //        Destroy(collisionDamagedDebuff);

    //    if (collisionGroggyDebuff != null)
    //        Destroy(collisionGroggyDebuff);

    //    ReCheck 스크립트 받아옴
    //   CollisionReCheck[] CRCs = go.GetComponents<CollisionReCheck>();

    //    for (int i = CRCs.Length - 1; i >= 0; i--)
    //    {
    //        Destroy(CRCs[i]);
    //    }


    //    CollisionObjectTime collisionObjectTime = go.GetComponent<CollisionObjectTime>();

    //    if (collisionObjectTime != null)
    //        collisionObjectTime.ResetObject();

    //}








    

    /************* RPC입니다. ****************/


    [PunRPC]
    private void RPCNotMoveDebuff(float MDT)
    {

        Debug.Log("sadf");
        // 속박 받아옴
        PlayerNotMoveDebuff playerNotMoveDebuff = gameObject.GetComponent<PlayerNotMoveDebuff>();

        // 속박 없으면 새로 추가
        if (playerNotMoveDebuff == null)
        {
            Debug.Log("sadf2");
            playerNotMoveDebuff = gameObject.AddComponent<PlayerNotMoveDebuff>();

        }

        // 속박 설정 추가
        playerNotMoveDebuff.SetMaxDebuffTime(MDT);
        playerNotMoveDebuff.SetNowDebuffTime(0);

        // 플레이어 상태에 속박상태라 알림
        playerState.SetplayerNotMoveDebuff(playerNotMoveDebuff);
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

        SoundManager.GetInstance().PlayHitSound();
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


    // 오브젝트 삭제용 RPC
    // 삭제할 오브젝트를 모른다. ID 값으로 찾는방법 알아봐야할듯.
    /* [PunRPC]
     void RPCDestroyObject()
     {
         for(int i = 0; i < PoolingManager.GetInstance().
     }*/
}
