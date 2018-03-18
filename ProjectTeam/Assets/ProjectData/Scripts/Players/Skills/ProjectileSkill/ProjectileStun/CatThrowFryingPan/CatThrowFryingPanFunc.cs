using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CatThrowFryingPan
{
    public override bool CheckState()
    {
        if ((EqualState(PlayerState.ConditionEnum.RUN) ||
             EqualState(PlayerState.ConditionEnum.IDLE)))
        {
            return true;
        }
        else
            return false;
    }

    public override void UseSkill()
    {
        gameObject.GetComponent<Animator>().SetTrigger("ThrowFryingPanTrigger");
        gameObject.GetComponent<PhotonView>().RPC("ThrowFryingPanAnimation", PhotonTargets.Others);
    }


    //// RPC입니다.

    [PunRPC]
    void ThrowFryingPanAnimation()
    {
        gameObject.GetComponent<Animator>().SetTrigger("ThrowFryingPanTrigger");
    }



    //// 애니메이션 이벤트입니다.

    void CreateFryingPan()
    {
        // 위치 똑바로 잡아주기. 손하고 연결해서. 나중에.
        float BulletDistance = 1.0f;
        float CharacterHeight = 1.2f;

        Vector3 BulletDefaultPlace = transform.forward * BulletDistance;
        BulletDefaultPlace.y += CharacterHeight;

        GameObject TGO = Instantiate(BulletPrefab, transform.position + (BulletDefaultPlace), Quaternion.identity);

        SetCollisionData(TGO, this);
    }


}