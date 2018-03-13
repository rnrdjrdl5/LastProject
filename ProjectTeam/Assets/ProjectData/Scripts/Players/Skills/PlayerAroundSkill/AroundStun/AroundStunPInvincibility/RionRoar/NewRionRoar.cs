using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRionRoar : DefaultAroundStunPInvincibility
{


    public override bool CheckState()
    {
        if (EqualState(PlayerState.ConditionEnum.IDLE) ||
            EqualState(PlayerState.ConditionEnum.RUN))
        {
            return true;
        }
        else
            return false;
    }

    public override void UseSkill()
    {
        gameObject.GetComponent<Animator>().SetTrigger("isRionRoar");
        gameObject.GetComponent<PhotonView>().RPC("RionRushAnimation", PhotonTargets.Others);
    }




    /***************아래부터는 RPC 입니다. ********************************/
    [PunRPC]
    void RionRushAnimation()
    {
        gameObject.GetComponent<Animator>().SetTrigger("isRionRoar");
    }


    /******************** 아래 부터는 애니메이션 이벤트 입니다. ************************/

    GameObject TGO;
    void CreateRionRoar()
    {

        TGO = Instantiate(AroundObject, transform.position , Quaternion.identity);
        TGO.GetComponent<MeshRenderer>().enabled = false;
        TGO.GetComponent<Transform>().localScale = Vector3.one * AroundObjectRadius;
        SetCollisionData(TGO, this);
        SetInvincibility();
    }

    void DeleteRionRoar()
    {
        Destroy(TGO);
        Destroy(PIB);
    }


}
