﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSpeedRun : DefaultNewSkill {

    /**** public ****/

    public float PlayerRunSpeed;                    // 플레이어 가속 속도


    /**** private ****/

    private float PlayerOriginalSpeed;              // 플레이어 원래 속도
    private float PlayerOriginalBackSpeed;          // 플레이어 원래 속도
    private float OriginalAniSpeed;                 // 플레이어 원래 애니메이션 전환속도
    private float CheckTime;                     // 시간 흐름 저장용


    private PlayerMove playerMove;               // 플레이어 이동 스크립트
    private PlayerCamera playerCamera;          // 플레이어 카메라 스크립트

    /**** 접근자 ****/

    public PlayerMove GetplayerMove() { return playerMove; }
    public void SetplayerMove(PlayerMove PM) { playerMove = PM; }

    public float GetPlayerOriginalSpeed() { return PlayerOriginalSpeed; }
    public void SetPlayerOriginalSpeed(float OS) { PlayerOriginalSpeed = OS; }

    public float GetPlayerOriginalBackSpeed() { return PlayerOriginalBackSpeed; }
    public void SetPlayerOriginalBackSpeed(float OB) { PlayerOriginalBackSpeed = OB; }

    public float GetOriginalAniSpeed() { return OriginalAniSpeed; }
    public void SetOriginalAniSpeed(float AS) { OriginalAniSpeed = AS; }

    public float GetCheckTime() { return CheckTime; }
    public void SetCheckTime(float CT) { CheckTime = CT; }



    protected override void Awake()
    {

        // 부모에서 설정
        base.Awake();

        // 스킬 스크립트 설정
        defaultCdtAct = new RunCdtAct();
        defaultCdtAct.InitCondition(this);

        // 스크립트 설정
        playerMove = gameObject.GetComponent<PlayerMove>();

        // 카메라 설정
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();

    }


    public override bool CheckState()
    {
        //이동중이거나 가만히 있을 때 가능합니다.
        if (playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN))

        {
            return true;
        }
        else
            return false;
    }

    public override void UseSkill()
    {
        
        // 1. 플레이어 기존 이동속도 저장
        PlayerOriginalSpeed = playerMove.PlayerSpeed;
        PlayerOriginalBackSpeed = playerMove.PlayerBackSpeed;

        // 2. 플레이어 이동속도 갱신
        playerMove.PlayerSpeed = PlayerRunSpeed;
        playerMove.PlayerBackSpeed = PlayerRunSpeed;

        // 3. 플레이어 애니메이션 전환속도 저장
        OriginalAniSpeed = playerMove.AniSpeedUp;

        // 4. 플레이어 이동속도 갱신 (즉발)
        playerMove.AniSpeedUp = 20.0f;


        Debug.Log("스킬사용");
        Debug.Log(" 이동속도 저장값 : " + PlayerOriginalSpeed);

        // 3. 애니메이션 변경
        animator.SetBool("isSpeedRun", true);
    }

    public override bool CheckCtnState()
    {


        // 이동 or 대쉬
        // 
        if ((playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN) ||
            playerState.EqualPlayerCondition(PlayerState.ConditionEnum.SPEEDRUN)) &&
            (CheckTime < 0.05f))
        {
            return true;
        }
        else
        {
            return false;
        }


    }

    public override void UseCtnSkill()
    {
        if (animator.GetFloat("DirectionX") == 0 &&
            animator.GetFloat("DirectionY") == 0)
        {
            CheckTime += Time.deltaTime;
        }
        else
        {
            CheckTime = 0.0f;
        }
    }

    public override void ExitCtnSkill()
    {
        Debug.Log("사라짐");

        // 1. 이동속도 원래대로 돌림
        playerMove.PlayerSpeed = PlayerOriginalSpeed;
        playerMove.PlayerBackSpeed = PlayerOriginalBackSpeed;

        // 2. 애니메이션 일반 상태로 돌림
        animator.SetBool("isSpeedRun", false);

        // 3. 애니메이션 속도 복구시킴
        playerMove.AniSpeedUp = OriginalAniSpeed;

        // 4. 지속시간 초기화
        CheckTime = 0.0f;

    }

}
