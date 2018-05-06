using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerMove
{

    // 초기 설정
    void SetAwake()
    {


        characterController = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
        ps = gameObject.GetComponent<PlayerState>();
        //PlayerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();
        playerCamera = PlayerCamera.GetInstance();
        newInteractionSkill = GetComponent<NewInteractionSkill>();
        findObject = GetComponent<FindObject>();
      //  timeBar = GetComponent<TimeBar>();
        playerHealth = GetComponent<PlayerHealth>();

        if (playerCamera == null)
        {
            Debug.Log(" 카메라 못찾음.에러.");
        }

        OriginalPlayerSpeed = PlayerSpeed;

        // 애니메이션 뷰 설정
        PhotonAnimatorView pav = gameObject.GetComponent<PhotonAnimatorView>();
        if (pav == null)
            return;
        pav.SetParameterSynchronized("DirectionX", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Continuous);
        pav.SetParameterSynchronized("DirectionY", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Continuous);
        pav.SetParameterSynchronized("JumpType", PhotonAnimatorView.ParameterType.Int, PhotonAnimatorView.SynchronizeType.Discrete);
        pav.SetParameterSynchronized("StunOnOff", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
    }



    void PlayerTransform()
    {

        if (photonView.isMine)
        {

              if(ps.EqualPlayerCondition(PlayerState.ConditionEnum.RUN) ||
                ps.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
                ps.EqualPlayerCondition(PlayerState.ConditionEnum.SPEEDRUN) ||
                ps.EqualPlayerCondition(PlayerState.ConditionEnum.INTERACTION))
            {

                if (characterController.isGrounded)
                {
                    // 1. 플레이어 이동방향 설정
                    MoveDir = new Vector3(HSpeed, 0, VSpeed);

                    // 2. 노말처리
                    float NormalsqrMag = MoveDir.normalized.sqrMagnitude;

                    // 대쉬가 아닌 경우 방향전환
                    if (!ps.EqualPlayerCondition(PlayerState.ConditionEnum.SPEEDRUN))
                        MoveDir = new Vector3(HSpeed, 0, VSpeed);

                    // 대쉬인 경우 직진(회전에서 방향처리)
                    else
                    {
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

                        ResetSkill();


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

                
            }


            // 캐릭터 조건부 x방향 회전
            if(!ps.EqualPlayerCondition(PlayerState.ConditionEnum.STUN) &&
                !ps.EqualPlayerCondition(PlayerState.ConditionEnum.GROGGY))
                SetPlayerRotateX();



            // 점프했으면
            if ( (ps.EqualPlayerCondition(PlayerState.ConditionEnum.RUN) ||
                 ps.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
                 ps.EqualPlayerCondition(PlayerState.ConditionEnum.SPEEDRUN)) &&
                 Input.GetKeyDown(KeyCode.Space) && 
                 characterController.isGrounded)
            {
                // 1. 점프 준비자세 시작

                // - 원래 이동속도로 돌립니다.
                if (ps.EqualPlayerCondition(PlayerState.ConditionEnum.SPEEDRUN))
                {
                    MoveDir /= PlayerSpeed;

                    PlayerSpeed = OriginalPlayerSpeed;

                    MoveDir *= PlayerSpeed;

                    Debug.Log(OriginalPlayerSpeed);
                }

                // - 점프 시작
                animator.SetInteger("JumpType", 1);

                // - 점프 추가
                MoveDir.y += JumpSpeed;

                
            }

            // 점프 아니면 중력.
            else
            {
                MoveDir.y -= gravity * Time.deltaTime;
            }

            // 캐릭터 움직임.
            characterController.Move(MoveDir * Time.deltaTime);



            // 공중에 떠있으면 , 착륙중일때.
            // 1. 높은 건물에서 떨어질 때
            // 2. 사용자가 점프해서 떨어질 때
            if (!characterController.isGrounded &&
                MoveDir.y < 0 &&
                animator.GetInteger("JumpType") != 2)
              {
                  animator.SetInteger("JumpType", 2);

                // 플레이어 최대 위치등록
                PreFallPosition = transform.position.y;

                Debug.Log("높이 갱신 : " + PreFallPosition);
                  
              }


              // 지상일때, 착지 애니메이션 사용중일때 
              else if (characterController.isGrounded &&
                  animator.GetInteger("JumpType") == 2 )
              {
                
                  animator.SetInteger("JumpType", 0);

                //착지 시 높이 판단.
                // 1. 쥐인경우
                if ((string)PhotonNetwork.player.CustomProperties["PlayerType"] == "Mouse")
                {
                    Debug.Log("착륙높이 :" + transform.position.y);
                    if (PreFallPosition - transform.position.y >= AtLeastFallPosition)
                    {
                        // 데미지주기
                        playerHealth.CallFallDamage(FallDamage);

                        // 스턴 주기
                        ps.AddDebuffState(DefaultPlayerSkillDebuff.EnumSkillDebuff.STUN,FallStunTime);

                        // 속도값 없애기
                        MoveDir = Vector3.up * MoveDir.y;
                    }
                }
              
              }









        }
    }


    void PlayerMoveAnimation()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {

            // 키를 누를 수 있는 상태라면.
            if (isCanKey)
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
    }

    
    

    // 플레이어가 마우스 x축으로 이동하는지에 대한 여부.
    void SetPlayerRotateX()
    {
        // 시점 자유 여부
        if (playerCamera.GetCameraModeType() != PlayerCamera.EnumCameraMode.FREE)
        {

            
            Vector3 v3 = transform.rotation.eulerAngles;

            // 캐릭터 회전값 받기
            PlayerRotateEuler += Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime;
            if (PlayerRotateEuler >= 360)
                PlayerRotateEuler -= 360;

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


    private void CheckResetCanKey()
    {

        // 입력 받을 수 없는상태
        // + 키입력이 없는 상태
        if ((Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) &&
            isCanKey ==false)
        {
            isCanKey = true;
        }

    }
    
    // 애니메이션 스테이트 exit역할을 하던 것. 
    // exit 는 늦기 때문에 직접적으로 호출
    // interaction에서.
    public void ResetSkill()
    {
        if (photonView.isMine)
        {

            // 애니메이션 타입이고
            // 액션이 사용되지 않은 상태라면
            if (newInteractionSkill.GetinteractiveState().ActionType == InteractiveState.EnumAction.ANIMATION &&
                newInteractionSkill.GetinteractiveState().IsUseAction == false)
            {
                newInteractionSkill.GetinteractiveState().CallRPCCancelActionAnimation();
            }
            // FIndObject의 활성화 탐지 시작
            findObject.SetisUseFindObject(true);


            //timeBar.DestroyObjects()
            //      Debug.Log(UIManager.GetInstance().timerBarPanelScript.DestroyTimebar());
            UIManager.GetInstance().timerBarPanelScript.DestroyTimebar();



            Debug.Log(UIManager.GetInstance());
            //.DestroyTimebar();


            // 1. 카메라를 따라가는 상태로 변경.
            playerCamera.SetCameraModeType(PlayerCamera.EnumCameraMode.FOLLOW);

            // 2. 플레이어의 회전값을 free값으로 변경
            animator.GetComponent<PlayerMove>().SetPlayerRotateEuler(-playerCamera.GetCameraRadX());




            Debug.Log("해제해제해제 , + " + newInteractionSkill.GetinteractiveState().photonView.viewID);

            // 모든 클라이언트에게 RPC전송 하는 함수 콜
            //interactiveState.SetisCanFirstCheck(true);
            // ** 마스터에서 처리하기에는 애니메이션 동기화 문제가 있어서 안됨
            newInteractionSkill.GetinteractiveState().CallOnCanFirstCheck();
        }
    }

    public void ResetMoveSpeed()
    {
        MoveDir = new Vector3 { x = 0, y = MoveDir.y, z = 0 };

    }

}
