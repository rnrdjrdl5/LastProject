using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class DefaultInteraction
{

    virtual protected bool CheckState()
    {
        Debug.Log("Dummy File, Click Me!");
        return false;
    }


    // 상호작용 키작동에 대한 여부.
   protected bool CheckSkillKey()
    {
        bool ReturnType = false;
        switch (SkillKeyType)
        {
            case EnumSkillKey.LEFTMOUSE:
                ReturnType = Input.GetMouseButtonUp(0);
                break;

            case EnumSkillKey.F:
                ReturnType = Input.GetKeyUp(KeyCode.F);
                break;

            default:
                Debug.Log("에러 , Click Me!");
                break;

        }
        return ReturnType;
    }






    public bool EqualState(PlayerState.ConditionEnum CE)
    {
        if (gameObject.GetComponent<PlayerState>().GetPlayerCondition() == CE)
            return true;
        else
            return false;
    }

    protected void UseAnimation()
    {
        animator.SetBool("isInteraction", true);

        animator.SetInteger("InteractionType", (int)InteractiveKeyType);

        pv.RPC("RPCInteraction", PhotonTargets.Others, (int)InteractiveKeyType);

        Debug.Log("b");
    }

    virtual protected void BaseTimeBarScript()
    {
        TimeBarScript.CreateTimeBarPanel();
        TimeBarScript.SetNowTime(0);
        TimeBarScript.SetMaxTime(fo.GetObjectState().InteractiveTime);
        TimeBarScript.SetisCount(true);
    }



    /**
    [PunRPC]
    void RPCInteraction(int Interac)
    {
        Debug.Log("a");
        animator.SetBool("isInteraction", true);
        animator.SetInteger("InteractionType", Interac);

    }**/
    // RPC는 상속이 안된다.

}

