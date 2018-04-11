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



    
    private const int GroggyType = 1;               // 데미지 피격 타입

    private PlayerHealth playerHealth;              // 플레이어 체력 스크립트

    private PlayerState playerState;                // 플레이어 상태

    private Animator animator;              // 애니메이터 

    private CollisionObject collisionObject;            // 충돌 오브젝트 스크립트

    private void Awake()
    {
        
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
        NumberOfCollisions numberOfCollisions = other.gameObject.GetComponent<NumberOfCollisions>();

        // 충돌횟수 남았으면
        if (numberOfCollisions != null)
        {
            // 감소
            numberOfCollisions.DeCreaseNumberOfCollisions();
            
            // 충돌횟수 끝나면 삭제
            if(numberOfCollisions.GetNumberOfCollisions() == 0)
            {

                Destroy(other.gameObject);
            }
               

        }
    }

    private void LeftApplyDamage(Collider other)
    {
        // 충돌 데미지 받아옴
        CollisionObjectDamage collisionObjectDamage = other.gameObject.GetComponent<CollisionObjectDamage>();
        
        // 받았는지 체크
        if (collisionObjectDamage != null)
        {

            // 마스터) 
            if (PhotonNetwork.isMasterClient)
            {

                // 데미지 주기
                playerHealth.CallApplyDamage(collisionObjectDamage.GetObjectDamage(),collisionObjectDamage.EffectType) ;
                Debug.Log("데미지 : " + collisionObjectDamage.GetObjectDamage());
                

            }

            // 데미지 충돌횟수 감소
            collisionObjectDamage.DecreaseObjectDamageNumber();

            // 데미지 충돌횟수 확인
            if(collisionObjectDamage.GetObjectDamageNumber() == 0)
            {

                // 데미지 충돌 끝
                Destroy(collisionObjectDamage);
            }
        }
    }

    private void LeftDebuff(Collider other)
    {

        // 마스터)
        if (PhotonNetwork.isMasterClient)
        {

            // 디버프 받기
            CollisionPushDebuff collisionPushDebuff = other.gameObject.GetComponent<CollisionPushDebuff>();
            CollisionNotMoveDebuff collisionNotMoveDebuff = other.gameObject.GetComponent<CollisionNotMoveDebuff>();
            CollisionStunDebuff collisionStunDebuff = other.gameObject.GetComponent<CollisionStunDebuff>();
            CollisionDamagedDebuff collisionDamagedDebuff = other.gameObject.GetComponent<CollisionDamagedDebuff>();

            // 충돌체들에 따른 버프 사용
            if (collisionPushDebuff != null)
            {
                photonView.RPC("RPCPushDebuff",PhotonTargets.All, collisionPushDebuff.GetMoveDirection(), 
                    collisionPushDebuff.GetMoveSpeed(), collisionPushDebuff.GetMaxTime());
            }

            if(collisionNotMoveDebuff != null)
            {
                photonView.RPC("RPCNotMoveDebuff", PhotonTargets.All, collisionNotMoveDebuff.GetMaxTime());
            }

            if(collisionStunDebuff != null)
            {
                photonView.RPC("RPCStunDebuff", PhotonTargets.All, collisionStunDebuff.GetMaxTime());
            }

            if(collisionDamagedDebuff != null)
            {
                if (playerState.GetPlayerCondition() != PlayerState.ConditionEnum.STUN)
                {
                    photonView.RPC("RPCDamagedDebuff", PhotonTargets.All,collisionDamagedDebuff.GetMaxTime());
                }
            }

        }
    }


    /************* RPC입니다. ****************/
    [PunRPC]
    private void RPCPushDebuff(Vector3 MD, float MPS, float MDT)
    {
        // 밀쳐내기 여부 확인
        PlayerPushDebuff playerPushDebuff = gameObject.GetComponent<PlayerPushDebuff>();

        // 밀쳐내기 없으면 새로 등록
        if (playerPushDebuff == null)
        {

            playerPushDebuff = gameObject.AddComponent<PlayerPushDebuff>();
        }

        // 밀쳐내기 정보 갱신
        playerPushDebuff.SetMoveDirection(MD);
        playerPushDebuff.SetMovePushSpeed(MPS);
        playerPushDebuff.SetMaxDebuffTime(MDT);
    }

    [PunRPC]
    private void RPCNotMoveDebuff(float MDT)
    {

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

    [PunRPC]
    private void RPCDamagedDebuff(float DD)
    {
        // 피격 애니메이션 랜덤 설정
        animator.SetInteger("DamageOnOff", Random.Range(1, GroggyType + 1));

        // 피격 디버프  받기
        PlayerDamagedDebuff playerDamagedDebuff = gameObject.GetComponent<PlayerDamagedDebuff>();

        // 피격 디버프 없으면 추가하기
        if (playerDamagedDebuff == null)
        {
            playerDamagedDebuff = gameObject.AddComponent<PlayerDamagedDebuff>();
            Debug.Log("피격 추가");
        }

        // 피격 속성 설정
        playerDamagedDebuff.SetMaxDebuffTime(DD);
        playerDamagedDebuff.SetNowDebuffTime(0);
    }

}
