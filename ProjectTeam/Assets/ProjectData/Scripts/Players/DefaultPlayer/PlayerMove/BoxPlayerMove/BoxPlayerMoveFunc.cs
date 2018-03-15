﻿using System.Collections;
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
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");

                Animator animator = gameObject.GetComponent<Animator>();
                animator.SetFloat("DirectionX", h);
                animator.SetFloat("DirectionY", v);
            
             }

        }
    }
}
