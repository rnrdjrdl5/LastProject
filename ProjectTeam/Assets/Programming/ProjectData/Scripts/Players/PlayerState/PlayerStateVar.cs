using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerState
{ 

    private Animator animator;

    private GameObject HeadObject;


    private string PlayerType;

    public string GetPlayerType() { return PlayerType; }
    public void SetPlayerType(string pt)
    {
        PlayerType = pt;
    }


    public void SetHeadObject()
    {
        Transform[] tr = GetComponentsInChildren<Transform>();

        for (int i = 0; i < tr.Length; i++)
        {

            if (tr[i].name == "Bip001 Head")
            {
                HeadObject = tr[i].gameObject;
                break;
            }

        }
    }
    public GameObject GetHeadObject() { return HeadObject; }

    /********* 플레이어의 현재 상태를 가지는 열거형입니다. ******/
    public enum ConditionEnum
    { NONE , NOTUSINGSKILL , 
        IDLE, RUN, SPEEDRUN , JUMP,
        DAMAGE, STUN , GROGGY , NOTMOVE
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





    

    /*
    // 상태이상들 입니다.
    private PlayerNotMoveDebuff playerNotMoveDebuff;
    public void SetplayerNotMoveDebuff(PlayerNotMoveDebuff CNMD ) { playerNotMoveDebuff = CNMD; }
    public PlayerNotMoveDebuff GetplayerNotMoveDebuff() { return playerNotMoveDebuff; }
    */



}
