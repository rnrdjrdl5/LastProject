using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerMove
{

    // 초기 설정
    void SetAwake()
    {

        animator = gameObject.GetComponent<Animator>();
        ps = gameObject.GetComponent<PlayerState>();
        PlayerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();

        if (PlayerCamera == null)
        {
            Debug.Log(" 카메라 못찾음.에러.");
        }

        OriginalPlayerSpeed = PlayerSpeed;
    }



    void PlayerTransform()
    {

        if (photonView.isMine)
        {

            if ((gameObject.GetComponent<PlayerState>().GetplayerNotMoveDebuff() == null) &&
               (ps.EqualPlayerCondition(PlayerState.ConditionEnum.RUN) ||
                ps.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
                ps.EqualPlayerCondition(PlayerState.ConditionEnum.SPEEDRUN) ||
                ps.EqualPlayerCondition(PlayerState.ConditionEnum.ATTACK) ||
                ps.EqualPlayerCondition(PlayerState.ConditionEnum.INTERACTION)))
            {
                
                if (gameObject.GetComponent<CharacterController>().isGrounded)
                {

                    // 1. 플레이어 이동방향 설정
                    MoveDir = new Vector3(HSpeed, 0, VSpeed);

                    // 2. 노말처리
                    float NormalsqrMag = MoveDir.normalized.sqrMagnitude;

                    // 대쉬가 아닌 경우 방향전환
                    if (!ps.EqualPlayerCondition(PlayerState.ConditionEnum.SPEEDRUN))
                        MoveDir = new Vector3(HSpeed, 0, VSpeed);

                    // 대쉬인 경우 직진(회전에서 방향처리)
                    else {
                        MoveDir = Vector3.forward * MoveDir.magnitude;
                    }

                    // 노말벡터 길이 vs 일반벡터 길이
                    if (MoveDir.sqrMagnitude > NormalsqrMag &&
                        NormalsqrMag > 0)
                    {
                        MoveDir = MoveDir.normalized; 
                    }


                    if (ps.EqualPlayerCondition(PlayerState.ConditionEnum.INTERACTION) &&
                        MoveDir != Vector3.zero)
                    {

                        // 플레이어 다시 이동으로 변경
                        animator.SetInteger("InteractionType", 0);


                    }

                    // 벡터를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환한다.
                    MoveDir = transform.TransformDirection(MoveDir);

                    // 앞으로 이동 시
                    if (Input.GetAxisRaw("Vertical") >= 0 && VSpeed >= 0)
                    {
                        MoveDir *= PlayerSpeed;
                    }

                    // 뒤로 이동 시
                    else
                    {
                        MoveDir *= PlayerBackSpeed;
                    }

                }

                // 캐릭터에 중력 적용.
                MoveDir.y -= gravity * Time.deltaTime;

                // 캐릭터 움직임.
                gameObject.GetComponent<CharacterController>().Move(MoveDir * Time.deltaTime);
            }

            // 캐릭터 조건부 x방향 회전
            SetPlayerRotateX();

        }
    }


    void PlayerMoveAnimation()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {


                //0. 입력에 따라서 Speed를 결정합니다.

                if (HSpeed + SetMoveAnimation(HSpeed, Input.GetAxisRaw("Horizontal")) > -(AniSpeedUp) * Time.deltaTime &&
                     HSpeed + SetMoveAnimation(HSpeed, Input.GetAxisRaw("Horizontal")) < (AniSpeedUp) * Time.deltaTime)
                {
                    HSpeed = 0.0f;
                }

                else
                {
                    HSpeed = HSpeed + SetMoveAnimation(HSpeed, Input.GetAxisRaw("Horizontal"));
                }

                if (VSpeed + SetMoveAnimation(VSpeed, Input.GetAxisRaw("Vertical")) > -(AniSpeedUp) * Time.deltaTime &&
                     VSpeed + SetMoveAnimation(VSpeed, Input.GetAxisRaw("Vertical")) < (AniSpeedUp) * Time.deltaTime)
                {
                    VSpeed = 0.0f;
                }

                else
                {
                    VSpeed = VSpeed + SetMoveAnimation(VSpeed, Input.GetAxisRaw("Vertical"));
                }

                // 1. 키 입력을 받습니다.
                float HKey = Input.GetAxisRaw("Horizontal");
                float VKey = Input.GetAxisRaw("Vertical");

                // 2. 이동에 따라서 열거형을 설정합니다.
                CalcKey_X(HSpeed, HKey);
                CalcKey_Y(VSpeed, VKey);

                //3. 배수가 있으면 열거형을 수정합니다.
                HSpeed = CalcPlayerMoveMulti(SpeedMultiTypeX, SpeedLocationTypeX, HSpeed);
                VSpeed = CalcPlayerMoveMulti(SpeedMultiTypeY, SpeedLocationTypeY, VSpeed);

                //4. 특정 값 (1)이 넘어가지 못하도록 고정합니다.
                HSpeed = CalcPlusMinus(HSpeed);
                VSpeed = CalcPlusMinus(VSpeed);

                animator.SetFloat("DirectionX", HSpeed);
                animator.SetFloat("DirectionY", VSpeed);
           
        }
    }

    
    

    // 플레이어가 마우스 x축으로 이동하는지에 대한 여부.
    void SetPlayerRotateX()
    {
       
        // 시점 자유 여부
        if (PlayerCamera.GetCameraModeType() != PlayerCamera.EnumCameraMode.FREE)
        {

            
            Vector3 v3 = transform.rotation.eulerAngles;

            // 캐릭터 회전값 받기
            PlayerRotateEuler += Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime;

            // 전력질주 상태
            if (ps.GetPlayerCondition() == PlayerState.ConditionEnum.SPEEDRUN)
            {
                
                // 변수 초기화
                float atan2 = 0.0f;
                float YRotate = 0.0f;

                // 움직임 여부
                if (!(animator.GetFloat("DirectionX") == 0 && animator.GetFloat("DirectionY") == 0))
                {
                    
                    // 애니메이션 float으로 각도 구하기
                    atan2 = Mathf.Atan2(animator.GetFloat("DirectionX"), animator.GetFloat("DirectionY")) * Mathf.Rad2Deg;

                    // 각도 적용한 YROTATE 생성
                    YRotate = PlayerRotateEuler + atan2;
                }

                // 갱신값으로 회전
                transform.rotation = Quaternion.Euler(v3.x, YRotate, v3.z);

            }

            // 전력질주 상태가 아닌 경우
            else
                transform.rotation = Quaternion.Euler(v3.x, PlayerRotateEuler, v3.z);

            
        }
    }


    void CalcKey_X(float aniKey, float key)
    {
        if (aniKey > 0)
        {
            if (key < 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.MINUS;
                SpeedMultiTypeX = EnumSpeedMulti.MULTI;
            }

            else if (key == 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.NONE;
                SpeedMultiTypeX = EnumSpeedMulti.NONE;
            }

            else if (key > 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.PLUS;
            }
        }

        else if (aniKey < 0)
        {
            if (key > 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.PLUS;
                SpeedMultiTypeX = EnumSpeedMulti.MULTI;
            }

            else if (key == 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.NONE;
                SpeedMultiTypeX = EnumSpeedMulti.NONE;
            }

            else if (key < 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.MINUS;
            }
        }
        else if (aniKey == 0)
        {
            if (key > 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.PLUS;
            }
            else if (key == 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.NONE;
            }
            else if (key < 0)
            {
                SpeedLocationTypeX = EnumSpeedLocation.MINUS;
            }
        }
    }

    void CalcKey_Y(float aniKey, float key)
    {
        if (aniKey > 0)
        {
            if (key < 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.MINUS;
                SpeedMultiTypeY = EnumSpeedMulti.MULTI;
            }

            else if (key == 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.NONE;
                SpeedMultiTypeY = EnumSpeedMulti.NONE;
            }

            else if (key > 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.PLUS;
            }
        }

        else if (aniKey < 0)
        {
            if (key > 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.PLUS;
                SpeedMultiTypeY = EnumSpeedMulti.MULTI;
            }

            else if (key == 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.NONE;
                SpeedMultiTypeY = EnumSpeedMulti.NONE;
            }

            else if (key < 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.MINUS;
            }
        }
        else if (aniKey == 0)
        {
            if (key > 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.PLUS;
            }
            else if (key == 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.NONE;
            }
            else if (key < 0)
            {
                SpeedLocationTypeY = EnumSpeedLocation.MINUS;
            }
        }
    }

    float SetMoveAnimation(float NowSpeed, float aniKey)
    {

        if (NowSpeed > 0)
        {
            if (aniKey < 0)
            {
                
                return -(AniSpeedUp) * Time.deltaTime;
            }

            else if (aniKey == 0)
            {
                
                return -(AniSpeedUp) * Time.deltaTime;
            }
            else if (aniKey > 0)
            {
                return (AniSpeedUp) * Time.deltaTime;
            }
        }

        else if (NowSpeed == 0)
        {
            if (aniKey < 0)
            {
                return -(AniSpeedUp) * Time.deltaTime;
            }
            else if (aniKey == 0)
            {
                return 0;
            }
            else if (aniKey > 0)
            {
                return (AniSpeedUp) * Time.deltaTime;
            }
        }

        else if (NowSpeed < 0)
        {
            if (aniKey < 0)
            {
                return -(AniSpeedUp) * Time.deltaTime;
            }
            else if (aniKey == 0)
            {
                return (AniSpeedUp) * Time.deltaTime;
            }
            else if (aniKey > 0)
            {
                return (AniSpeedUp) * Time.deltaTime;
            }
        }
        return 0.0f;
    }


    float CalcPlayerMoveMulti(EnumSpeedMulti esm, EnumSpeedLocation esl , float Speed)
    {
        if (esm == EnumSpeedMulti.MULTI)
        {
            if (esl == EnumSpeedLocation.PLUS)
            {
                Speed += AniSpeedUp * SpeedMulti * Time.deltaTime;
            }
            else if (esl == EnumSpeedLocation.MINUS)
            {
                Speed += -AniSpeedUp * SpeedMulti * Time.deltaTime;
            }
        }
        return Speed;
    }

    float CalcPlusMinus(float Speed)
    {
        if (Speed > 1.0f)
        {
            Speed = 1.0f;
        }

        else if (Speed < -1.0f)
        {
            Speed = -1.0f;
        }

        return Speed;
    }
}
