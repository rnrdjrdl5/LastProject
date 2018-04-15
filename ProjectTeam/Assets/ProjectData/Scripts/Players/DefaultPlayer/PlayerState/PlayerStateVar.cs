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

    public ConditionEnum   PlayerCondition         = ConditionEnum.IDLE;


    public ConditionEnum GetPlayerCondition() { return PlayerCondition; }

    public void SetPlayerCondition(ConditionEnum CE) { PlayerCondition = CE; }

    public bool EqualPlayerCondition(ConditionEnum CE)
    {
        if (CE == PlayerCondition) return true;
        else return false;
    }









    // 상태이상들 입니다.
    private PlayerNotMoveDebuff playerNotMoveDebuff;
    public void SetplayerNotMoveDebuff(PlayerNotMoveDebuff CNMD ) { playerNotMoveDebuff = CNMD; }
    public PlayerNotMoveDebuff GetplayerNotMoveDebuff() { return playerNotMoveDebuff; }




}
