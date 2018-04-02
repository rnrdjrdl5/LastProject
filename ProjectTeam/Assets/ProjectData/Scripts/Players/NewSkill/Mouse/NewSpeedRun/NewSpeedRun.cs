using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSpeedRun : DefaultNewSkill {

    private float PlayerOriginalSpeed;
    public float PlayerRunSpeed;

    private PlayerMove playerMove;               // 플레이어 이동 스크립트
    private PlayerCamera playerCamera;

    protected override void Awake()
    {
        base.Awake();

        playerMove = gameObject.GetComponent<PlayerMove>();


        playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();
        
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
        
        // 1. 플레이어 기존 이동속도 저장
        PlayerOriginalSpeed = playerMove.PlayerSpeed;

        // 2. 플레이어 이동속도 갱신
        playerMove.PlayerSpeed = PlayerRunSpeed;


        Debug.Log("스킬사용");
        Debug.Log(" 이동속도 저장값 : " + PlayerOriginalSpeed);

        // 3. 애니메이션 변경
        animator.SetBool("isSpeedRun", true);

        // 4. 카메라 설정 변경
        playerCamera.SetCameraModeType(PlayerCamera.EnumCameraMode.SPEEDRUN);
    }

    protected override bool CheckCtnState()
    {
        if (playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN) ||
            (playerState.EqualPlayerCondition(PlayerState.ConditionEnum.SPEEDRUN)))
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

        // 1. 이동속도 원래대로 돌림
        playerMove.PlayerSpeed = PlayerOriginalSpeed;

        // 2. 애니메이션 일반 상태로 돌림
        animator.SetBool("isSpeedRun", false);

        // 3. 카메라 일반상태로 돌림.
        playerCamera.SetCameraModeType(PlayerCamera.EnumCameraMode.FOLLOW);
        Debug.Log(playerMove.PlayerSpeed);
        Debug.Log("스킬해제");
    }

}
