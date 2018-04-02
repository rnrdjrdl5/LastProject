using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerState
{ 
    /********* 플레이어의 현재 상태를 가지는 열거형입니다. ******/
    public enum ConditionEnum
    { NONE , NOTUSINGSKILL , 
        IDLE, RUN, SPEEDRUN , 
        WINDBLAST, BLINK,
        DAMAGE, ATTACK , SEALCHARM ,STUN
     , THROW_FRYING_PAN
    , INTERACTION , };

    private ConditionEnum   PlayerCondition         = ConditionEnum.IDLE;


    public ConditionEnum GetPlayerCondition() { return PlayerCondition; }

    public void SetPlayerCondition(ConditionEnum CE) { PlayerCondition = CE; }

    public bool EqualPlayerCondition(ConditionEnum CE)
    {
        if (CE == PlayerCondition) return true;
        else return false;
    }




    /******* 스킬 중 입력값이 바뀌는 스킬인 경우  해당 상태를 참조해서 *****
    ********  키입력을 바꿉니다. *******/
    private bool            isUseSkillMode          = false;


    public bool GetisUseSkillMode() { return isUseSkillMode; }

    public void SetisUseSkillMode(bool USM) { isUseSkillMode = USM; }


    // 타겟 지정 스킬간의 상태변경 시 이동할 때 쓰입니다.
    // ex) 스마트 키 안쓰고 베이가 w , q  사용할 때. 
    // 범위가 보이는데, wqwqwq 할경우 범위가 바뀌면서 기존 범위 사라짐. 이때사용.
    public GameObject PlaceObject = null;


    // 어떤 스킬을 사용중인지 판단합니다. 
    private ConditionEnum PlayerSkillCondition = ConditionEnum.NONE;


    public ConditionEnum GetPlayerSkillCondition() { return PlayerSkillCondition; }
    public void SetPlayerSkillCondition(ConditionEnum CE , GameObject NewPlaceObject)
    {
        //1. 오브젝트가 있으면 삭제함.
        if (PlaceObject != null)
            Destroy(PlaceObject);

        // 일반상태가 아닌 상태 로 돌아가면, 새로운 Object를 등록한다.
        if (CE != ConditionEnum.NONE)
            PlaceObject = NewPlaceObject;

        PlayerSkillCondition = CE;
    }

    public void SetPlayerSkillCondition(ConditionEnum CE)
    {
        PlayerSkillCondition = CE;
    }
    public bool EqualSkillCondition(ConditionEnum CE)
    {
        if (PlayerSkillCondition == CE)        
            return true;
        else
            return false;
    }



    // 상태이상들 입니다.
    private PlayerNotMoveDebuff playerNotMoveDebuff;
    public void SetplayerNotMoveDebuff(PlayerNotMoveDebuff CNMD ) { playerNotMoveDebuff = CNMD; }
    public PlayerNotMoveDebuff GetplayerNotMoveDebuff() { return playerNotMoveDebuff; }




}
