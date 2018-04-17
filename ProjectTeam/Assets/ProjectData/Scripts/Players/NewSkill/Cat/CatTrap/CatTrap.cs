using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**************************
 * 작성일 2018. 04. 03 
 * 작성자 : 반재억
 * 
 * 목적 : 쥐덪스킬 제작
 * **************************/
public class CatTrap : DefaultNewSkill
{
    public TrapState trapState;


    protected override void Awake()
    {

        // 부모 Awake 사용
        base.Awake();
        
        // Normal 형태 조건 , 액션문 사용
        defaultCdtAct = new NormalCdtAct();

        // 값 설정
        defaultCdtAct.InitCondition(this);

        gameObject.GetComponent<PhotonAnimatorView>().SetParameterSynchronized("isCatTrap", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
    }

    public override bool CheckState()
    {
        if (
             playerState.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
            playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN))
        {
            return true;
        }

        else
            return false;
    }

    public override void UseSkill()
    {
        animator.SetBool("isCatTrap", true);
    }

    void CreateTrap()
    {

        // 마스터인 경우
        if(photonView.isMine)
        {

            // 모든 클라이언트에 전송
            photonView.RPC("RPCCreateTrap", PhotonTargets.All , PhotonNetwork.player.ID);
        }
    }

    void OffTrap()
    {

        animator.SetBool("isCatTrap", false);
    }


    /**** RPC ****/
    [PunRPC]
    void RPCCreateTrap(int ID)
    {
        float BulletDistance = 0f;
        float CharacterHeight = 0.5f;

        Vector3 BulletDefaultPlace = transform.forward * BulletDistance;

        BulletDefaultPlace.y += CharacterHeight;

        GameObject CharmBullet = Instantiate(trapState.ProjectileObject, transform.position + (BulletDefaultPlace), Quaternion.identity);

        trapState.SetData(CharmBullet, gameObject, ID);

        // 발사체에 디버프를 넣습니다.
        AddDebuffComponent(CharmBullet);
    }


}
