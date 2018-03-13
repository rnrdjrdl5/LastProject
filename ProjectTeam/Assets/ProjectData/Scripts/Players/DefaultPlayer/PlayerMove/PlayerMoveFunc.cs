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


    void PlayerTransform()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            PlayerState ps = gameObject.GetComponent<PlayerState>();

            if ( (gameObject.GetComponent<PlayerState>().GetplayerNotMoveDebuff() == null) && 
                       ( ps.EqualPlayerCondition(PlayerState.ConditionEnum.RUN) ||
                        ps.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
                        ps.EqualPlayerCondition(PlayerState.ConditionEnum.BLINK) || 
                        ps.EqualPlayerCondition(PlayerState.ConditionEnum.ATTACK ) ))
            {
                PlayerHorizontal = Input.GetAxisRaw("Horizontal");
                PlayerVertical = Input.GetAxisRaw("Vertical");
                transform.Translate(new Vector3(PlayerHorizontal, 0, PlayerVertical) * Time.deltaTime * PlayerSpeed, Space.Self);
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

                if ((PlayerHorizontal != 0 || PlayerVertical != 0) &&
                 (ps.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
                 ps.EqualPlayerCondition(PlayerState.ConditionEnum.RUN)))
                {
                    if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
                    {
                        gameObject.GetComponent<Animator>().SetBool("isIdleRun", true);
                    }
                }

                else
                {
                    gameObject.GetComponent<Animator>().SetBool("isIdleRun", false);
                }
            }
        }
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.Log("asd");
    }
}
