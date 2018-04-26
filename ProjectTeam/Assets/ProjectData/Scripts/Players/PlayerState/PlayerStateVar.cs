using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerState
{ 

    private Animator animator;




    /********* 플레이어의 현재 상태를 가지는 열거형입니다. ******/
    public enum ConditionEnum
    { NONE , NOTUSINGSKILL , 
        IDLE, RUN, SPEEDRUN , JUMP,
        DAMAGE, /*ATTACK  ,*/STUN , GROGGY
     , THROW_FRYING_PAN , TRAP
    , INTERACTION , };

    public ConditionEnum   PlayerCondition         = ConditionEnum.IDLE;

    public ConditionEnum GetPlayerCondition() { return PlayerCondition; }

    public void SetPlayerCondition(ConditionEnum CE) { PlayerCondition = CE; }

    public bool EqualPlayerCondition(ConditionEnum CE)
    {
        if (CE == PlayerCondition) return true;
        else return false;
    }




    private bool isUseAttack = false;          // 공격 사용중인가


    public void SetisUseAttack(bool UA)
    {
        isUseAttack = UA;
    }

    public bool GetisUseAttack() { return isUseAttack; }





    


    // 상태이상들 입니다.
    private PlayerNotMoveDebuff playerNotMoveDebuff;
    public void SetplayerNotMoveDebuff(PlayerNotMoveDebuff CNMD ) { playerNotMoveDebuff = CNMD; }
    public PlayerNotMoveDebuff GetplayerNotMoveDebuff() { return playerNotMoveDebuff; }


    // 디버프를 겁니다.
    public void AddDebuffState(DefaultPlayerSkillDebuff.EnumSkillDebuff ESD , float MaxTime)
    {
        if (ESD == DefaultPlayerSkillDebuff.EnumSkillDebuff.STUN)
        {
            // 스턴 애니메이션 재생
            animator.SetBool("StunOnOff", true);

            PlayerStunDebuff playerStunDebuff = gameObject.GetComponent<PlayerStunDebuff>();

            if (playerStunDebuff == null)
            {
                playerStunDebuff = gameObject.AddComponent<PlayerStunDebuff>();
            }


            playerStunDebuff.SetMaxDebuffTime(MaxTime);
            playerStunDebuff.SetNowDebuffTime(0);

        }
    }




}
