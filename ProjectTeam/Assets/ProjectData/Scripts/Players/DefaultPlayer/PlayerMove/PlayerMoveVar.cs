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

    [Header(" 이동속도")]
    public float PlayerSpeed = 10.0f;               // 캐릭터 이동속도

    [Header("뒤 이동속도")]
    public float PlayerBackSpeed = 5.0f;                // 카메라 뒤로 이동

    [Header("카메라 회전속도")]
    public float RotationSpeed = 100.0f;             // 캐릭터 회전속도

    [Header("애니메이션 배수")]
    [Tooltip("다른 방향으로 이동 시 추가 배수값으로 이동 " )]
    public float SpeedMulti;                    // 애니메이션 배수

    [Header("애니메이션 재생 속도")]
    public float AniSpeedUp;                    // 애니메이션 속도

    [Header("점프 값 ")]
    [Tooltip(" 점프 속도에 따른 수치.")]
    public float JumpSpeed;

    [Header("중력값")]
    [Tooltip(" 중력수치")]
    public float gravity = 20;


    /**** private ****/


    private Animator animator;
    private PlayerState ps;
    private CharacterController characterController;                // 캐릭터 컨트롤러
    private NewInteractionSkill newInteractionSkill;            // 상호작용
    private FindObject findObject;                      // 탐지
    private TimeBar timeBar;                // 타임바 , **** 나중에 반드시 바뀔것임.

    private Vector3 MoveDir = Vector3.zero;             // 플레이어 이동속도
    private Vector3 JumpMoveDir = Vector3.zero;             // 플레이어 이동속도,  점프 이전에 저장값. 점프후에 다시 돌려줌

    private bool isJumping = false;             // 점프 사용 여부 , 떨어질 때 구분용도
    private bool isCanKey = true;               // 키 누를 수 있는지 여부

    private float OriginalPlayerSpeed = 0.0f;               // SpeedRun) 일반 이동속도 저장함
    private float HSpeed = 0;               // 플레이어 가로 속도
    private float VSpeed = 0;               // 플레이어 세로 속도
    private float PlayerRotateEuler = 0;                // 플레이어 오일러 회전값 , 플레이어 회전 담당

    

    


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

    public Vector3 GetMoveDir() { return MoveDir; }
    public void SetMoveDir(Vector3 md) { md = MoveDir; }

    public Vector3 GetJumpMoveDir() { return JumpMoveDir; }
    public void SetJumpMoveDir(Vector3 md) { md = JumpMoveDir; }

    public bool GetisCanKey() { return isCanKey; }
    public void SetisCanKey(bool ck) { isCanKey = ck; }






}
