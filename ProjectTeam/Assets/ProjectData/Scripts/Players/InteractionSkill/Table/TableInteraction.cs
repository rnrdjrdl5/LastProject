using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableInteraction : DefaultInteraction
{
    protected override void Awake()
    {
        base.Awake();

        // 애니메이션 이름 적기
        // RPC 애니메이션 이름 적기.
        //SetRPCAnimationName();
        InteractiveKeyType = EnumInteractiveKey.TABLE;


    }


    protected override bool CheckState()
    {
        if ((EqualState(PlayerState.ConditionEnum.RUN) ||
           EqualState(PlayerState.ConditionEnum.IDLE)))
        {
            return true;
        }
        else
            return false;
    }


    

    [PunRPC]
    void RPCInteraction(int Interac)
    {
        animator.SetInteger("InteractionType", Interac);

    }
    


    void OffTableInteraction()
    {
        // 원래 idlerun으로 돌아간다.
        animator.SetInteger("InteractionType", 0);

        //다시 상호작용 물체를 찾는다.
        GetFindObjectScript().SetisUseFindObject(true);

        //타임바를 파괴한다.
        GetTimeBarScript().DestroyObjects();

        //카메라를 재설정한다.
        PlayerCamera.SetCameraModeType(PlayerCamera.EnumCameraMode.FOLLOW);

        //다음번에 상호작용을 못하도록 막는다.
        GetInteractiveObject().GetComponent<InteractiveState>().SetCanUseObject(false);


    }

    void SetPhysics()
    {
        InteractionAction ia = gameObject.GetComponent<InteractionAction>();
        ia.SetInteractionObject(GetInteractiveObject());
        ia.Action();

    }



}
