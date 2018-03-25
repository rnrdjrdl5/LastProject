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

    //중요, 해당 스크립트를 상속받아서 사용하는 객체들을 밑의 RPC를 반드시 구현해야합니다.
    protected void UseAnimation()
    {
        

        animator.SetInteger("InteractionType", (int)InteractiveKeyType);

        pv.RPC("RPCInteraction", PhotonTargets.Others);
    }

    virtual protected void BaseTimeBarScript()
    {
        // 타임바 패널을 생성합니다.
        TimeBarScript.CreateTimeBarPanel();

        // 타임바 시간을 조절합니다.
        TimeBarScript.SetNowTime(0);
        TimeBarScript.SetMaxTime(fo.GetObjectState().InteractiveTime);

        // 타임바 카운트를 시작합니다.
        TimeBarScript.SetisCount(true);
    }


    // 상호작용의 액션을 시작합니다.
    public void StartInterAction(GameObject go)
    {

        defaultPushObject.InitInterData(go);

        defaultPushObject.Action();
    }



    protected void InitPushObject()
    {
        defaultPushObject.InitData(gameObject, PlayerCamera.gameObject);

        defaultPushObject.SetXZPower(XZPower);
        defaultPushObject.SetYPower(YPower);
        defaultPushObject.SetTorquePower(TorquePower);
    }


    // 스크립트를 고릅니다.
    protected void ChoosePushScript()
    {
        switch(InteractiveKeyType)
        {
            case EnumInteractiveKey.TABLE:
                defaultPushObject = new TablePushObject();
                break;
        }
    }

    // 애니메이션 이벤트) 공격 판정 시작 시 설정합니다.
    protected void InitStartAction()
    {
        for (int i = defaultPartCollisionConditions.Length - 1; i >= 0; i--)
        {
            // 공격 액션이 인식 가능한 상태라고 알립니다.
            defaultPartCollisionConditions[i].SetisCanInterAction(true);

            // 공격을 사용하고 있는 스크립트를 부위에 등록합니다.
            defaultPartCollisionConditions[i].SetdefaultInteraction(this);

            //공격받는 오브젝트의 타입을 저장합니다.
            defaultPartCollisionConditions[i].SetInterObjectType(InteractiveKeyType);
        }
    }

    protected void InitEndAction()
    {
        for (int i = defaultPartCollisionConditions.Length - 1; i >= 0; i--)
        {
            // 공격액션을 꺼버립니다.
            defaultPartCollisionConditions[i].SetisCanInterAction(false);

            //공격을 사용하고 있는 스크립트가 없다고 설정해줍니다.
            defaultPartCollisionConditions[i].SetdefaultInteraction(null);

            //다시 한번 더 공격이 가능하다 알립니다.
            DefaultPartCollisionCondition.isUseInterAction = false;
        }
    }



    // RPC입니다. 
    [PunRPC]
    void RPCUseFirstInterObjectOther()
    {
        InteractiveObject.GetComponent<InteractiveState>().SetisCanFirstStart(true);
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

