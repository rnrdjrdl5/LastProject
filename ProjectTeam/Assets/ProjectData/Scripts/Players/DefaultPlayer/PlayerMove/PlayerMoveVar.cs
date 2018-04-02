using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerMove
{

    /**** enum ****/

    public enum EnumSpeedLocation { PLUS, NONE, MINUS };                // 애니메이션 블렌딩
    public enum EnumSpeedMulti { MULTI, NONE };             // 애니메이션 블렌딩



    /**** public ****/


    public PlayerCamera PlayerCamera;               // 카메라 스크립트

    public float PlayerSpeed = 10.0f;               // 캐릭터 이동속도
    public float PlayerBackSpeed = 5.0f;                // 카메라 뒤로 이동
    public float RotationSpeed = 100.0f;             // 캐릭터 회전속도
    public float SpeedMulti;                    // 애니메이션 배수
    public float AniSpeedUp;                    // 애니메이션 속도
    

    /**** private ****/


    private Animator animator;
    private PlayerState ps;

    private Vector3 MoveDir = Vector3.zero;             // 플레이어 이동속도
    private float OriginalPlayerSpeed = 0.0f;               // SpeedRun) 일반 이동속도 저장함
    private float HSpeed = 0;               // 플레이어 가로 속도
    private float VSpeed = 0;               // 플레이어 세로 속도
    private float gravity = 20.0f;              // 중력
    private float PlayerRotateEuler = 0;                // 플레이어 오일러 회전값
    

    EnumSpeedLocation SpeedLocationTypeX = EnumSpeedLocation.NONE;              // 애니메이션 블렌딩
    EnumSpeedLocation SpeedLocationTypeY = EnumSpeedLocation.NONE;              // 애니메이션 블렌딩
    EnumSpeedMulti SpeedMultiTypeX = EnumSpeedMulti.NONE;               // 애니메이션 블렌딩
    EnumSpeedMulti SpeedMultiTypeY = EnumSpeedMulti.NONE;               // 애니메이션 블렌딩


    /**** 접근자 ****/


    public float GetOriginalPlayerSpeed() { return OriginalPlayerSpeed; }
    public void SetOriginalPlayerSpeed(float SPS) { OriginalPlayerSpeed = SPS; }

    public float GetHSpeed() { return HSpeed; }
    public void SetHSpeed(float HS) { HSpeed = HS; }

    public float GetVSpeed() { return VSpeed; }
    public void SetVSpeed(float VS) { VSpeed = VS; }

    public float GetPlayerRotateEuler() { return PlayerRotateEuler; }
    public void SetPlayerRotateEuler(float RPE) { PlayerRotateEuler = RPE; }







}
