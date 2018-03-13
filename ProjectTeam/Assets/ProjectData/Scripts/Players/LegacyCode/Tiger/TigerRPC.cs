using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TigerScript{

    [PunRPC]
    void RPCKnockBack(Vector3 Vec3)
    {
        // 본인의 클라이언트면 움직임과 MoveStart를 사용해준다.
        if (pv.isMine)
        {
            Debug.Log("backCharacter사용");
            Debug.Log("Vec3 : " + Vec3);
            KnockbackDistance = Vec3;
        }
        StartCoroutine("KnockBackCoroutine");

    }

    [PunRPC]
    void BlinkPosition(Vector3 newPosition)
    {
        transform.position = newPosition;

        // 시리얼라이즈에서 이미 받고 나서 
        // update 로 가기 전에
        // RPC가 들어오면,
        // RPC로 인한 위치이동 > update로 인한 보간  이 순서대로 적용되서
        // RPC로 위치 이동했다가 보간위치로 이동함.
        // 그래서 시리얼라이즈로 받는 값도 바꿔준다.
   //     RecvPosition = newPosition;

        // 도착 후 생성? Instantiate(BlinkEffect, transform.position, Quaternion.identity);
    }

   /* [PunRPC]
    void WindBlastAnimation()
    {
        PlayerAnimator.SetTrigger("WindBlastTrigger");
    }
    */
    [PunRPC]
    void ApplyDamage(float _damage)
    {
        if (pv.isMine)
        {
          //  NowHealth -= _damage;
        }
    }

}
