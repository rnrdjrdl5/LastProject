using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class DefaultPlayer
{
    
   /* void SetAwake()
    {
        pv = GetComponent<PhotonView>();

        RecvPosition = transform.position;
        RecvRotation = transform.rotation;

        PlayerAnimator = GetComponent<Animator>();

        if (pv.isMine)
        {
            PlayerCamera = GameObject.Find("PlayerCamera");
            PlayerCamera.GetComponent<PlayerCamera>().PlayerObject = gameObject;
            PlayerCamera.GetComponent<PlayerCamera>().isPlayerSpawn = true;
        }
    }*/


  /*  void PlayerTransform()
    {
        if (pv.isMine)
        {


            if ((!isKnockBackMoving) && (
                        PlayerCondition == ConditionEnum.RUN ||
                        PlayerCondition == ConditionEnum.IDLE ||
                        PlayerCondition == ConditionEnum.BLINK))
            {
                transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * Time.deltaTime * PlayerSpeed, Space.Self);
            }



            transform.Rotate(Vector3.up * Time.deltaTime * RotationSpeed * Input.GetAxis("Mouse X"));
        }
    }
    */
    /*
    void PlayerMoveAnimation()
    {
        if (pv.isMine)
        {
            if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && (!isKnockBackMoving)
                && (PlayerCondition == ConditionEnum.IDLE || PlayerCondition == ConditionEnum.RUN))
            {

                PlayerAnimator.SetBool("isIdleRun", true);
            }
            else
            {
                PlayerAnimator.SetBool("isIdleRun", false);
            }
        }
    }
    */

    /*void SyncMoveAnimation()
    {

        if (!pv.isMine)
        {
            float Speed = 0.2f;

            if (!isKnockBackMovingOther)
            {
                if ((RecvPosition - transform.position).magnitude > Speed) // magnitude한 값으로 서로를 계산하면 오류 발생.
                {
                    PlayerAnimator.SetBool("isIdleRun", true);
                }
                else
                {
                    PlayerAnimator.SetBool("isIdleRun", false);
                }
            }
        }
    }*/
    /*
    void SetSyncData()
    {
        if (!pv.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, RecvPosition, Time.deltaTime * 10.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, RecvRotation, Time.deltaTime * 10.0f);
            //NowHealth = RecvHealth;
        }
    }*/

    void KnockingBackPlayer()
    {
        if (isKnockBackMoving)
        {
            GetComponent<Rigidbody>().AddForce((KnockbackDistance * KnockbackSpeed * Time.fixedDeltaTime), ForceMode.Impulse);
        }
    }


    IEnumerator KnockBackCoroutine()
    {
        Debug.Log("넉백코루틴 실행");
        PlayerAnimator.SetBool("DamageOnOff", true);
            isKnockBackMoving = true;

        yield return new WaitForSeconds(KnockbackTime);




        PlayerAnimator.SetBool("DamageOnOff", false);
            isKnockBackMoving = false;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        yield return null;
    }

    IEnumerator KnockBackCoroutineOther()
    {
        Debug.Log("다른 코루틴 사용");
        PlayerAnimator.SetBool("DamageOnOff", true);
        isKnockBackMovingOther = true;

        yield return new WaitForSeconds(KnockbackTime);

        PlayerAnimator.SetBool("DamageOnOff", false);
        isKnockBackMovingOther = false;
    }

    /*void OnDeadZone(Collider other)
    {
        if (other.tag == "DeadZone")
        {
            PlayerDead();
        }
    }*/

        /*
    void PlayerDead()
    {
        PlayerCamera.GetComponent<PlayerCamera>().isPlayerSpawn = false;
        PhotonNetwork.Destroy(gameObject);
        gameObject.GetComponent<PlayerUI>().GetUIObject();
    }
    */

   /* void CheckTriggerObject(Collider other)
    {
            if ((other.tag == "Bullet") &&
                (other.gameObject.GetComponent<Bullet>().ShootPlayer != "Player" + pv.viewID))
            {
                Destroy(other.gameObject);

                if (PhotonNetwork.isMasterClient && other.gameObject.GetComponent<Bullet>().isBulletCheck == false)
                {
                    // 데미지 받는 건 꺼둔다. pv.RPC("ApplyDamage", PhotonTargets.All, 10.0f);
                    pv.RPC("RPCKnockBack", PhotonTargets.All, other.gameObject.GetComponent<Bullet>().BulletDistance);

                    //Destroy 가 늦어서 총알충돌 인식이 여러번 되는 경우가 있다. 
                    //총알의 체크를 한번만 하도록 설정해서 막는다.
                    other.gameObject.GetComponent<Bullet>().isBulletCheck = true;


                }
            }
    }*/



    public virtual void OtherStart() { }

    public virtual void OtherAwake() { }

    public virtual void OtherUpdate() { }

    public virtual void OtherFixedUpdate() { }

    public virtual void OtherOnTriggerEnter(Collider other) { }

    public virtual void OtherOnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }

}
