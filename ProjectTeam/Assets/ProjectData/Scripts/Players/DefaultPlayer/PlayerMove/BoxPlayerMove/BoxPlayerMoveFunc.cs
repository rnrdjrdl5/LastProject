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

        SavePlayerSpeed = PlayerSpeed;
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
            //float Speed = 0.2f;

            /*if ((RecvPosition - transform.position).magnitude > Speed) // magnitude한 값으로 서로를 계산하면 오류 발생.
            {

                gameObject.GetComponent<Animator>().SetBool("isIdleRun", true);
            }
            else
            {
                gameObject.GetComponent<Animator>().SetBool("isIdleRun", false);
            }*/

            Animator animator = GetComponent<Animator>();

            Vector3.Lerp(transform.position, RecvPosition, Time.deltaTime * 10.0f);

            float BeforeDirectionX = animator.GetFloat("DirectionX");
            float BeforeDirectionY = animator.GetFloat("DirectionY");

            float NewDirectionX = Mathf.Lerp(BeforeDirectionX, RecvDirectionX, Time.deltaTime * 10.0f);
            float NewDirectionY = Mathf.Lerp(BeforeDirectionY, RecvDirectionY, Time.deltaTime * 10.0f);

            animator.SetFloat("DirectionX", NewDirectionX);
            animator.SetFloat("DirectionY", NewDirectionY);
        }
    }


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
                gameObject.GetComponent<PlayerNotMoveDebuff>();
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




            /*if ((PlayerHorizontal != 0 || PlayerVertical != 0) &&
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
            }*/

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

    public override void OnDisconnectedFromPhoton()
    {
        Debug.Log("asd");
    }
}
