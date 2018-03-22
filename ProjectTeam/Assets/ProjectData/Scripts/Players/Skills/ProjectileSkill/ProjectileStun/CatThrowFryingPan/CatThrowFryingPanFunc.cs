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



}