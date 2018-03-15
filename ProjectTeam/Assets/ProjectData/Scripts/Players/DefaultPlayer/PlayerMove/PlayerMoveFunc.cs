using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerMove
{
    void SetAwake()
    {
        RecvPosition = transform.position;
        RecvRotation = transform.rotation;
    }

    void SendTransform(PhotonStream stream)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
    }

    void RecvTransform(PhotonStream stream)
    {
        if (!stream.isWriting)
        {
            RecvPosition = (Vector3)stream.ReceiveNext();
            RecvRotation = (Quaternion)stream.ReceiveNext();

        }
    }

    public void SyncTransform()
    {
        if (!gameObject.GetComponent<PhotonView>().isMine)
        {
            transform.position = Vector3.Lerp(transform.position, RecvPosition, Time.deltaTime * 10.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, RecvRotation, Time.deltaTime * 10.0f);
        }
    }

    void SyncMoveAnimation()
    {

        if (!gameObject.GetComponent<PhotonView>().isMine)
        {
            float Speed = 0.2f;

            if ((RecvPosition - transform.position).magnitude > Speed) // magnitude한 값으로 서로를 계산하면 오류 발생.
            {

                gameObject.GetComponent<Animator>().SetBool("isIdleRun", true);
            }
            else
            {
                gameObject.GetComponent<Animator>().SetBool("isIdleRun", false);
            }
        }
    }

    Vector3 MoveDir = Vector3.zero;
    void PlayerTransform()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            PlayerState ps = gameObject.GetComponent<PlayerState>();

            if ((gameObject.GetComponent<PlayerState>().GetplayerNotMoveDebuff() == null) &&
                       (ps.EqualPlayerCondition(PlayerState.ConditionEnum.RUN) ||
                        ps.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
                        ps.EqualPlayerCondition(PlayerState.ConditionEnum.BLINK) ||
                        ps.EqualPlayerCondition(PlayerState.ConditionEnum.ATTACK)))
            {
                if (gameObject.GetComponent<CharacterController>().isGrounded)
                {
                    // 위, 아래 움직임 셋팅. 
                    MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                    // 벡터를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환한다.
                    MoveDir = transform.TransformDirection(MoveDir);

                    // 스피드 증가.
                    MoveDir *= PlayerSpeed;

                }
                // 캐릭터에 중력 적용.
                MoveDir.y -= gravity * Time.deltaTime;

                // 캐릭터 움직임.
                gameObject.GetComponent<CharacterController>().Move(MoveDir * Time.deltaTime);
            }

            transform.Rotate(Vector3.up * Time.deltaTime * RotationSpeed * Input.GetAxis("Mouse X"));
        
        }

    }

    void PlayerMoveAnimation()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            PlayerState ps = gameObject.GetComponent<PlayerState>();



            if (gameObject.GetComponent<PlayerNotMoveDebuff>() == null)
            {

                if (
                 (ps.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
                 ps.EqualPlayerCondition(PlayerState.ConditionEnum.RUN) ) )
                {
                    if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
                    {
                        gameObject.GetComponent<Animator>().SetBool("isIdleRun", true);
                    }

                    else
                    {
                        gameObject.GetComponent<Animator>().SetBool("isIdleRun", false);
                    }
                }

            }
        }
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.Log("asd");
    }
}
