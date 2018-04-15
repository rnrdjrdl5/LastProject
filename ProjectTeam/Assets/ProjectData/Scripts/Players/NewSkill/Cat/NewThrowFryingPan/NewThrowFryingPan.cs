using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// DefaultNewSkill을 상속받습니다.
public class NewThrowFryingPan : DefaultNewSkill
{
    // 스킬의 옵션이 결정됩니다.
    // 스킬에서 사용하지 않는 옵션은 조정해봤자 의미 없습니다.
    public TorqueProjectileState torqueProjectileState ;

    protected override void Awake()
    {
        base.Awake();

        defaultCdtAct = new NormalCdtAct();
        defaultCdtAct.InitCondition(this);

        // 애니메이션 포톤 뷰 설정
        gameObject.GetComponent<PhotonAnimatorView>().SetParameterSynchronized("isThrowFryingPan", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
    }

    // 재정의
    public override bool CheckState()
    {
        //이동중이거나 가만히 있을 때 가능합니다.
        if ((
            playerState.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
            playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN)))

        {
            return true;
        }
        else
            return false;
    }

    // 재정의
    public override void UseSkill()
    {
        animator.SetBool("isThrowFryingPan", true);
    }

    // 애니메이션 이벤트입니다.
    void CreateFryingPan()
    {

        // 마스터인 경우
        if(PhotonNetwork.isMasterClient)
        {

            // 모든 클라이언트에게 공격을 전송
            photonView.RPC("RPCCreateFryingPan", PhotonTargets.All);
        }
    }

    void OffFryingPan()
    {
        
        animator.SetBool("isThrowFryingPan", false);
    }



    /**** RPC ****/

    [PunRPC]
    void RPCCreateFryingPan()
    {
        float BulletDistance = 1.0f;
        float CharacterHeight = 1.2f;

        Vector3 BulletDefaultPlace = transform.forward * BulletDistance;

        BulletDefaultPlace.y += CharacterHeight;


        Quaternion BulletRotation = transform.rotation;

        Vector3 v3 = transform.rotation.eulerAngles;
        v3.y += -90;
        v3.x += 135;
        

        GameObject CharmBullet = Instantiate(torqueProjectileState.ProjectileObject, transform.position + (BulletDefaultPlace), Quaternion.Euler(v3));

        torqueProjectileState.SetData(CharmBullet, gameObject);

        // 발사체에 디버프를 넣습니다.
        AddDebuffComponent(CharmBullet);
    }

}
