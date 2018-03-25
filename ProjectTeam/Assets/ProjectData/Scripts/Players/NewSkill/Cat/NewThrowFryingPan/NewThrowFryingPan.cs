using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// DefaultNewSkill을 상속받습니다.
public class NewThrowFryingPan : DefaultNewSkill
{
    // 스킬의 옵션이 결정됩니다.
    // 스킬에서 사용하지 않는 옵션은 조정해봤자 의미 없습니다.
    public ProjectileState projectileState;


    // 재정의
    protected override bool CheckState()
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
    protected override void UseSkill()
    {
        animator.SetTrigger("ThrowFryingPanTrigger");
        photonView.RPC("RPCThrowFryingPanTrigger",PhotonTargets.Others);
    }


    // RPC입니다.
    [PunRPC]
    void RPCThrowFryingPanTrigger()
    {
        animator.SetTrigger("ThrowFryingPanTrigger");
    }

    // 애니메이션 이벤트입니다.
    void CreateFryingPan()
    {

        float BulletDistance = 1.0f;
        float CharacterHeight = 1.2f;

        Vector3 BulletDefaultPlace = transform.forward * BulletDistance;

        BulletDefaultPlace.y += CharacterHeight;

        GameObject CharmBullet = Instantiate(projectileState.ProjectileObject, transform.position + (BulletDefaultPlace), Quaternion.identity);

        projectileState.SetData(CharmBullet,gameObject);

        // 발사체에 디버프를 넣습니다.
        AddDebuffComponent(CharmBullet);
    }



    //[PunRPC]

}
