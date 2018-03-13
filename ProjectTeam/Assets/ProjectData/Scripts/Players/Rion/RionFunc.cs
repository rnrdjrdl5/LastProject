using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RionScript
{

    /*
    public void RionAttackButton()
    {
        if (pv.isMine)
        {
            if (Input.GetMouseButtonDown(0))
            {

                if ((PlayerCondition == DefaultPlayer.ConditionEnum.IDLE ||
                PlayerCondition == DefaultPlayer.ConditionEnum.RUN ||
                PlayerCondition == DefaultPlayer.ConditionEnum.ATTACK) && (isComboAttack == true))

                {
                    PlayerCondition = DefaultPlayer.ConditionEnum.ATTACK;



                    PlayerAnimator.SetBool("isAttack", true);
                    pv.RPC("RionAttackRPC", PhotonTargets.Others);

                    isComboAttack = false;

                    // isComboAttack은 다른곳에서 처리해준다.
                }
            }
        }
    }

    */




    /// <summary>
    /// 공격 충돌 체크 시 , 충돌판정은 손과 머리에서 함.
    /// </summary>
  /*  public void Attack1Player(Collider other)
    {
        if (other.tag == "Players")
        {
            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("테스트성공1");
                Debug.Log("충돌대상 : " + other.tag);
                // RPC 사용해서 해당 소유 클라이언트에서 데미지 처리하도록 함.
                other.gameObject.GetComponent<TigerScript>().pv.RPC("ApplyDamage", PhotonTargets.All, 10.0f);
            }
        }
    }

    public void Attack2Player(Collider other)
    {
        if (other.tag == "Players")
        {
            Debug.Log("테스트성공2");
            Debug.Log("충돌대상 : " + other.tag);
            other.gameObject.GetComponent<TigerScript>().pv.RPC("ApplyDamage", PhotonTargets.All, 10.0f);

            //에러 , 데미지 대상을 잘못 정했음.
        }

    }

    public void Attack3Player(Collider other)
    {
        if (other.tag == "Players")
        {
            Debug.Log("테스트성공3");
            Debug.Log("충돌대상 : " + other.tag);
            other.gameObject.GetComponent<TigerScript>().pv.RPC("ApplyDamage", PhotonTargets.All, 10.0f);
        }
    }*/









    /// <summary>
    /// 아래로부터는 애니메이션 이벤트 전용입니다.
    /// </summary>
  /*  public void ResetComboAttack()
    {
        isComboAttack = true;
    }

    public void StartAttack1Check()
    {
        isCheckAttack1 = true;
    }

    public void EndAttack1Check()
    {
        isCheckAttack1 = false;
    }

    public void StartAttack2Check()
    {
        isCheckAttack2 = true;
    }

    public void EndAttack2Check()
    {
        isCheckAttack2 = false;
    }

    public void StartAttack3Check()
    {
        isCheckAttack3 = true;
    }

    public void EndAttack3Check()
    {
        isCheckAttack3 = false;
    }*/





}

