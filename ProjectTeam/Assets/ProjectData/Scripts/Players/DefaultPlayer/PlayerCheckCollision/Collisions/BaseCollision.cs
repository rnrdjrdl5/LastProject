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



public class BaseCollision : Photon.PunBehaviour {

    // Damage 피격 애니메이션 랜덤 적용 용도, 추가 시 개선 바람.
    // 0 : 중립
    // 1~ n  : 랜덤 경우의 수
    // 10 : 탈출 수.
    private const int GroggyType = 1;

    public void UseCollision(Collider other)
    {


        // 충돌체크 있을 때 충돌체크 시도하면.
        if (gameObject.GetComponent<PlayerInvincibilityBuff>() == null)
        {

            if (other.gameObject.GetComponent<CollisionObject>() != null)
            {
                if (other.gameObject.GetComponent<CollisionObject>().GetUsePlayer() != "Player" + gameObject.GetComponent<PhotonView>().viewID)
                {
                    if (!ReCheck(other))
                    {

                        Debug.Log("충돌성공.");

                        LeftApplyDamage(other);

                        LeftDebuff(other);

                        LeftNumberOfCollision(other);
                    }
                }
            }
        }
    }

    private bool ReCheck(Collider other)
    {
            
        // 충돌체의 recheck를 모두 받아옵니다.
            CollisionReCheck[] CRCs = other.gameObject.GetComponents<CollisionReCheck>();

         // 루프를 돌립니다.
            foreach (CollisionReCheck crc in CRCs)
            {

            // 충돌체의 플레이어 오브젝트와 이 오브젝트와 동일하다면
                if (crc.GetPlayerObject() == gameObject)
                {
                // 이미 있다.
                Debug.Log("충돌체크 대기중.");
                    return true;
                }
            }

            // 이후에 없으니까, 리체크 했다는 의미로 recheck등록해준다.
          CollisionReCheck CRC = other.gameObject.AddComponent<CollisionReCheck>();

        // 등록한 리체크의 정보를 등록합니다.
        CRC.SetPlayerObject(gameObject);
        // 시간도 설정합니다.
        CRC.SetPlayerReCheckTime(other.gameObject.GetComponent<CollisionObject>().GetCollisionReCheckTime());

        Debug.Log("충돌체크 등록성공");
            return false;
    }
    
    private void LeftNumberOfCollision(Collider other)
    {
        if(other.gameObject.GetComponent<NumberOfCollisions>() != null)
        {

            NumberOfCollisions NOC = other.gameObject.GetComponent<NumberOfCollisions>();

            NOC.DeCreaseNumberOfCollisions();
            
            if(NOC.GetNumberOfCollisions() == 0)
            {
                Destroy(other.gameObject);
                Debug.Log("충돌체크 N회 완료 .삭제함.");
            }
               

        }
    }

    private void LeftApplyDamage(Collider other)
    {
        if (other.gameObject.GetComponent<CollisionObjectDamage>() != null)
        {

            // 누가 때렸는지 등록함.
            gameObject.GetComponent<PlayerHealth>().SetEnemyObject(other.gameObject);

            // 데미지 주기는 Master만.
            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("데미지 : " + other.gameObject.GetComponent<CollisionObjectDamage>().GetObjectDamage());

                gameObject.GetComponent<PlayerHealth>().CallApplyDamage(other.gameObject.GetComponent<CollisionObjectDamage>().GetObjectDamage());
            }

            CollisionObjectDamage COD = other.gameObject.GetComponent<CollisionObjectDamage>();

            COD.DecreaseObjectDamageNumber();

            if(COD.GetObjectDamageNumber() == 0)
            {
                Destroy(other.gameObject.GetComponent<CollisionObjectDamage>());
                Debug.Log("데미지 충돌 끝, 삭제.");
            }
        }
    }

    private void LeftDebuff(Collider other)
    {
        if (PhotonNetwork.isMasterClient)
        {
            if(other.gameObject.GetComponent<CollisionPushDebuff>() != null)
            {

                photonView.RPC("RPCPushDebuff",PhotonTargets.All,
                    other.gameObject.GetComponent<CollisionPushDebuff>().GetMoveDirection(),
                    other.gameObject.GetComponent<CollisionPushDebuff>().GetMoveSpeed(),

                    other.gameObject.GetComponent<CollisionPushDebuff>().GetMaxTime());
                Debug.Log("테스트" + other.gameObject.GetComponent<CollisionPushDebuff>().GetMaxTime());
            }

            if(other.gameObject.GetComponent<CollisionNotMoveDebuff>() != null)
            {

                photonView.RPC("RPCNotMoveDebuff", PhotonTargets.All,
                    other.gameObject.GetComponent<CollisionNotMoveDebuff>().GetMaxTime()
                    );
                

            }

            if(other.gameObject.GetComponent<CollisionStunDebuff>() != null)
            {
                photonView.RPC("RPCStunDebuff", PhotonTargets.All,
                    other.gameObject.GetComponent<CollisionStunDebuff>().GetMaxTime()
                    );
            }

            if(other.gameObject.GetComponent<CollisionDamagedDebuff>() != null)
            {
                if (gameObject.GetComponent<PlayerState>().GetPlayerCondition() != PlayerState.ConditionEnum.STUN)
                {
                    photonView.RPC("RPCDamagedDebuff", PhotonTargets.All,
                        other.gameObject.GetComponent<CollisionDamagedDebuff>().GetMaxTime());
                }
            }

            //if(other.gameObject.GetComponent<>)

        }
    }


    /************* RPC입니다. ****************/
    [PunRPC]
    private void RPCPushDebuff(Vector3 MD , float MPS, float MDT)
    {
            if (gameObject.GetComponent<PlayerPushDebuff>() == null)
            {
                gameObject.AddComponent<PlayerPushDebuff>();
            }

            gameObject.GetComponent<PlayerPushDebuff>().SetMoveDirection(MD);

            gameObject.GetComponent<PlayerPushDebuff>().SetMovePushSpeed(MPS);

            gameObject.GetComponent<PlayerPushDebuff>().SetMaxDebuffTime(MDT);
    }

    [PunRPC]
    private void RPCNotMoveDebuff(float MDT)
    {


            if(gameObject.GetComponent<PlayerNotMoveDebuff>() == null)
            {
                gameObject.AddComponent<PlayerNotMoveDebuff>();
                
            }
            gameObject.GetComponent<PlayerNotMoveDebuff>().SetMaxDebuffTime(MDT);
            gameObject.GetComponent<PlayerNotMoveDebuff>().SetNowDebuffTime(0);

              gameObject.GetComponent<PlayerState>().SetplayerNotMoveDebuff(gameObject.GetComponent<PlayerNotMoveDebuff>());
    }




    [PunRPC]
    private void RPCStunDebuff(float ST)
    {


        gameObject.GetComponent<Animator>().SetBool("StunOnOff", true);
        if (gameObject.GetComponent<PlayerStunDebuff>() == null)
        {
            gameObject.AddComponent<PlayerStunDebuff>();
        }
        gameObject.GetComponent<PlayerStunDebuff>().SetMaxDebuffTime(ST);
        gameObject.GetComponent<PlayerStunDebuff>().SetNowDebuffTime(0);
    }

    [PunRPC]
    private void RPCDamagedDebuff(float DD)
    {
       // Random.Range(1, GroggyType + 1);
        gameObject.GetComponent<Animator>().SetInteger("DamageOnOff", Random.Range(1, GroggyType + 1));
        if(gameObject.GetComponent<PlayerDamagedDebuff>() == null)
        {
            gameObject.AddComponent<PlayerDamagedDebuff>();
            Debug.Log("피격 추가");
        }
        gameObject.GetComponent<PlayerDamagedDebuff>().SetMaxDebuffTime(DD);
        gameObject.GetComponent<PlayerDamagedDebuff>().SetNowDebuffTime(0);
        Debug.Log("피격 충돌체크 시간 설정");
    }

}
