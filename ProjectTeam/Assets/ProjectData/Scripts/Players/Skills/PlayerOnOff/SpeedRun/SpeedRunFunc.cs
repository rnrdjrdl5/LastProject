using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SpeedRun
{
    override public bool CheckState()
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
        // 스피드 조절. 
        // 마나감소시작.

        gameObject.GetComponent<PlayerManaPoint>().SetisReTimeMana(false);
        gameObject.GetComponent<PlayerManaPoint>().SetManaDecreasePoint(CostSpeedRun);
        gameObject.GetComponent<BoxPlayerMove>().PlayerSpeed = SpeedRunSpeed;
    }

    protected override void OffUseSkill()
    {
        Debug.Log("스킬오프감.");
        gameObject.GetComponent<PlayerManaPoint>().SetisReTimeMana(true);

        gameObject.GetComponent<BoxPlayerMove>().PlayerSpeed =
            gameObject.GetComponent<BoxPlayerMove>().GetSavePlayerSpeed();
    }
}
