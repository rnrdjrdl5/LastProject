using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerMove
{
    void SetAwake()
    {
        RecvPosition = transform.position;
        RecvRotation = transform.rotation;

        RecvDirectionX = 0.0f;
        RecvDirectionY = 0.0f;

        OriginalPlayerSpeed = PlayerSpeed;

        animator = gameObject.GetComponent<Animator>();

        PlayerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>(); ;
        if (PlayerCamera == null)
        {
            Debug.Log(" 카메라 못찾음.에러.");
        }

    }

    void SendTransform(PhotonStream stream)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(Input.GetAxis("Horizontal"));
            stream.SendNext(Input.GetAxis("Vertical"));
        }
    }

    void RecvTransform(PhotonStream stream)
    {
        if (!stream.isWriting)
        {
            RecvPosition = (Vector3)stream.ReceiveNext();
            RecvRotation = (Quaternion)stream.ReceiveNext();
            RecvDirectionX = (float)stream.ReceiveNext();
            RecvDirectionY = (float)stream.ReceiveNext();
        }
    }




    Vector3 MoveDir = Vector3.zero;
    void PlayerTransform()
    {

        if (photonView.isMine)
        {
            PlayerState ps = gameObject.GetComponent<PlayerState>();

            if ((gameObject.GetComponent<PlayerState>().GetplayerNotMoveDebuff() == null) &&
               (ps.EqualPlayerCondition(PlayerState.ConditionEnum.RUN) ||
                ps.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
                ps.EqualPlayerCondition(PlayerState.ConditionEnum.ATTACK) ||
                ps.EqualPlayerCondition(PlayerState.ConditionEnum.INTERACTION)))
            {


                if (gameObject.GetComponent<CharacterController>().isGrounded)
                {



                    // 위, 아래 움  직임 셋팅. 
                    //MoveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;


                    // 소수값이 안들어가서 속도에 에러가 있음.
                    MoveDir = new Vector3(HSpeed, 0, VSpeed);

                    float sqrMag = MoveDir.normalized.sqrMagnitude;

                    if(MoveDir.sqrMagnitude > sqrMag && 
                        sqrMag > 0)
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

                    if (Input.GetAxisRaw("Vertical") >= 0 && VSpeed >= 0)
                    {
                        MoveDir *= PlayerSpeed;
                    }

                    // 스피드 증가.
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





            PlayerRotateX();
        }
    }

    public float AniSpeed = 3.0f;

    void PlayerMoveAnimation()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {



            PlayerState ps = gameObject.GetComponent<PlayerState>();


            
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





            /* Debug.Log("SpeedLocationTypeX : " + (EnumSpeedLocation)SpeedLocationTypeX);
             Debug.Log("SpeedLocationTypeY : " + (EnumSpeedLocation)SpeedLocationTypeY);
             Debug.Log("SpeedMultiX : " + (EnumSpeedMulti)SpeedMultiTypeX);
             Debug.Log("SpeedMultiY : " + (EnumSpeedMulti)SpeedMultiTypeY);*/
            

            // 2. 기본 값.
            // 기존에 + 로 가고있는데 , - 값이 들어오면 반대방향으로 .  + 배속 
            // none이 들어오면 none으로. + 일반. 
            // 기존에 +로 가고있는데 , + 로 들어오면 / 기존 유지.

            // 기존에 - 로 가고있는데, +값이 들어오면 반대방향으로, + 배속
            // none이 들어오면 none으로,  + 일반.
            // 기존에 - 로 가고있는데, -로 들어오면 기존 유지.

            // 기존에 none 인데, + 들어오면 +으로, 일반
            // 기존에 none인데, - 들어오면 -으로 , 일반
            // 기;존에 none인데, none으로 들어오면 ㅇnone. 

            //   사용해야 할 것 :  + - 0  / 배속 여부

            // 반대방향? 




        }
    }

    // 플레이어가 마우스 x축으로 이동하는지에 대한 여부.
    void PlayerRotateX()
    {
        if (PlayerCamera.GetCameraModeType() == PlayerCamera.EnumCameraMode.FOLLOW)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * RotationSpeed * Input.GetAxis("Mouse X"));
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

        // 2. 기본 값.
        // 기존에 + 로 가고있는데 , - 값이 들어오면 반대방향으로 .  + 배속 
        // none이 들어오면 none으로. + 일반. 
        // 기존에 +로 가고있는데 , + 로 들어오면 / 기존 유지.

        // 기존에 - 로 가고있는데, +값이 들어오면 반대방향으로, + 배속
        // none이 들어오면 none으로,  + 일반.
        // 기존에 - 로 가고있는데, -로 들어오면 기존 유지.

        // 기존에 none 인데, + 들어오면 +으로, 일반
        // 기존에 none인데, - 들어오면 -으로 , 일반
        // 기;존에 none인데, none으로 들어오면 ㅇnone. 

        //   사용해야 할 것 :  + - 0  / 배속 여부

        // 반대방향? 
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
        //  현재 위치가 + 인데 - 입력이 들어오면 - 
        // 현재 위치가 + 인데 0 입력이 들어오면 - , 0일경우 0
        // 현재 위치가 + 인데 + 입력이 들어오면 +

        // 현재 위치가 0 인데 - 입력이 들어오면 -
        // 현재 위치가 0 인데 0 입력이 들어오면 0
        // 현재 위치가 0 인데 + 위치가 들어오면 + 

        // 현재 위치가 - 인데 - 입력이 들어오면 -
        // 현재 위치가 - 인데 0 입력이 들어오면 +
        // 현재 위치가 - 인데 + 입력이 들어오면 +

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
                // AniSpeedUp 애니속도
                // SpeedUp 추가가속도
                // Speed 원래가속도
                // (AniSpeedUp) * Time.deltaTime; 원래공식
                // (AniSpeedUp) * Time.deltaTime * Speed
               // AniSpeedUp * SppedMulti
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
