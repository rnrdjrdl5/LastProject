using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSpeedRun : DefaultNewSkill {

    private float PlayerOriginalSpeed;
    public float PlayerRunSpeed;

    PlayerMove BPM;

    protected override void Awake()
    {
        base.Awake();
        BPM = gameObject.GetComponent<PlayerMove>();
    }


    protected override bool CheckState()
    {
        //이동중이거나 가만히 있을 때 가능합니다.
        if (playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN))

        {
            return true;
        }
        else
            return false;
    }

    protected override void UseSkill()
    {
        // 스킬사용
        PlayerOriginalSpeed = BPM.PlayerSpeed;

        BPM.PlayerSpeed = PlayerRunSpeed;

        Debug.Log("스킬사용");
        Debug.Log(" 이동속도 저장값 : " + PlayerOriginalSpeed);
    }

    protected override bool CheckCtnState()
    {
        if (playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN))
        {
            return true;
        }
        return false;
    }

    protected override void UseCtnSkill()
    {
        // 지속형 스킬 을 사용합니다.
    }

    protected override void ExitCtnSkill()
    {
        Debug.Log(" 이동속도 저장값 : " + PlayerOriginalSpeed);
        BPM.PlayerSpeed = PlayerOriginalSpeed;

        Debug.Log(BPM.PlayerSpeed);
        Debug.Log("스킬해제");
    }

}
