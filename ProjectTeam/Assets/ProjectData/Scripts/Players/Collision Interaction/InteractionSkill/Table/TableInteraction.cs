using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableInteraction : DefaultInteraction
{
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


    
    
    // UseAnimation에서 사용
    [PunRPC]
    void RPCInteraction()
    {
        animator.SetInteger("InteractionType", (int)InteractiveKeyType);
    }
    


    void OffTableInteraction()
    {
            // 원래 idlerun으로 돌아간다.
            animator.SetInteger("InteractionType", 0);
    }

    // 타이밍에 맞춰서 실행합니다.
    public void StartAcion()
    {
        /*// 상호작용이 사용 가능하다고 알립니다.
        for (int i = defaultPartCollisionConditions.Length - 1; i >= 0; i--)
        {
            // 공격 액션이 인식 가능한 상태라고 알립니다.
            defaultPartCollisionConditions[i].SetisCanInterAction(true);

            // 공격을 사용하고 있는 스크립트를 부위에 등록합니다.
            defaultPartCollisionConditions[i].SetdefaultInteraction(this);

             //공격받는 오브젝트의 타입을 저장합니다.
            defaultPartCollisionConditions[i].SetInterObjectType(InteractiveKeyType);
        }
        // 손에서는 등록한 대상이 누군지 파악합니다.*/
        InitStartAction();

    }

    // 액션 끝
    public void EndAction()
    {

        /*  for(int i = defaultPartCollisionConditions.Length -1; i>=0; i--)
          {
              // 공격액션을 꺼버립니다.
              defaultPartCollisionConditions[i].SetisCanInterAction(false);

              //공격을 사용하고 있는 스크립트가 없다고 설정해줍니다.
              defaultPartCollisionConditions[i].SetdefaultInteraction(null);

              //다시 한번 더 공격이 가능하다 알립니다.
              DefaultPartCollisionCondition.isUseInterAction = false;
          }*/

        InitEndAction();
    }


}
