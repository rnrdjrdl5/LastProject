using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BoxPlayerMove
{
    void SetAwake()
    {
        RecvPosition = transform.position;
        RecvRotation = transform.rotation;

        RecvDirectionX = 0.0f;
        RecvDirectionY = 0.0f;

        OriginalPlayerSpeed = PlayerSpeed;

        PlayerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>(); ;
        if(PlayerCamera == null)
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



                    // 위, 아래 움직임 셋팅. 
                    MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                    if (ps.EqualPlayerCondition(PlayerState.ConditionEnum.INTERACTION) && 
                        MoveDir != Vector3.zero)
                    {
                        
                        // 플레이어 다시 이동으로 변경
                        gameObject.GetComponent<Animator>().SetInteger("InteractionType", 0);


                        // FIndObject의 활성화 탐지 시작
                        gameObject.GetComponent<FindObject>().SetisUseFindObject(true);


                        // TimeBar 관련 UI 모두 삭제
                        gameObject.GetComponent<TimeBar>().DestroyObjects();

                        // 카메라 다시 follow 설정
                        PlayerCamera.SetCameraModeType(PlayerCamera.EnumCameraMode.FOLLOW);


                    }

                    // 벡터를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환한다.
                    MoveDir = transform.TransformDirection(MoveDir);

                    if (Input.GetAxis("Vertical") > 0)
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

            gameObject.GetComponent<Animator>().SetFloat("DirectionX", Input.GetAxisRaw("Horizontal"));
            gameObject.GetComponent<Animator>().SetFloat("DirectionY", Input.GetAxisRaw("Vertical"));

            /*
            if (gameObject.GetComponent<PlayerNotMoveDebuff>() == null)
            {
                // 1. 가로입력
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    // 가로입력 더할때 1이상못나가도록.
                    if (HSpeed + Time.deltaTime * AniSpeed >= 1)
                    {
                        HSpeed = 1;
                    }

                    else
                    {
                        HSpeed += Time.deltaTime * AniSpeed;
                    }
                }

                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (HSpeed - Time.deltaTime * AniSpeed <= -1)
                    {
                        HSpeed = -1;
                    }
                    else
                    {
                        HSpeed -= Time.deltaTime * AniSpeed;
                    }
                }

                // 키값 없을경우 원래로 돌아가야지.
                else
                {
                    if (HSpeed > 0)
                    {
                        if (HSpeed - Time.deltaTime * AniSpeed < 0)
                        {
                            HSpeed = 0;
                        }
                        else
                        {
                            HSpeed -= Time.deltaTime * AniSpeed;
                        }
                    }

                    else if (HSpeed < 0)
                    {
                        if (HSpeed + Time.deltaTime * AniSpeed > 0)
                        {
                            HSpeed = 0;
                        }
                        else
                        {
                            HSpeed += Time.deltaTime * AniSpeed;
                        }
                    }
                }




                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    // 가로입력 더할때 1이상못나가도록.
                    if (VSpeed + Time.deltaTime * AniSpeed >= 1)
                    {
                        VSpeed = 1;
                    }

                    else
                    {
                        VSpeed += Time.deltaTime * AniSpeed;
                    }
                }

                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (VSpeed - Time.deltaTime * AniSpeed <= -1)
                    {
                        VSpeed = -1;
                    }
                    else
                    {
                        VSpeed -= Time.deltaTime * AniSpeed;
                    }
                }

                // 키값 없을경우 원래로 돌아가야지.
                else
                {
                    if (VSpeed > 0)
                    {
                        if (VSpeed - Time.deltaTime * AniSpeed < 0)
                        {
                            VSpeed = 0;
                        }
                        else
                        {
                            VSpeed -= Time.deltaTime * AniSpeed;
                        }
                    }

                    else if (VSpeed < 0)
                    {
                        if (VSpeed + Time.deltaTime * AniSpeed > 0)
                        {
                            VSpeed = 0;
                        }
                        else
                        {
                            VSpeed += Time.deltaTime * AniSpeed;
                        }
                    }
                }

                Animator animator = gameObject.GetComponent<Animator>();
                animator.SetInteger("DirectionX", HSpeed);
                animator.SetInteger("DirectionY", VSpeed);
            }
            */



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

}
