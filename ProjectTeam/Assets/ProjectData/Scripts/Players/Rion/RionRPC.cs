using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RionScript{

    [PunRPC]
    void RPCKnockBack(Vector3 Vec3)
    {
        // 본인의 클라이언트면 움직임과 MoveStart를 사용해준다.
        if (pv.isMine)
        {
            Debug.Log("backCharacter사용");
            Debug.Log("Vec3 : " + Vec3);
            KnockbackDistance = Vec3;
            StartCoroutine("KnockBackCoroutine");
        }
        else
        {
            StartCoroutine("KnockBackCoroutineOther");
        }

       
    }

    [PunRPC]
    void RionAttackRPC()
    {
        PlayerAnimator.SetBool("isAttack", true);
    }

    [PunRPC]
    void ApplyDamage(float _damage)
    {
        if(pv.isMine)
        {
        //    NowHealth -= _damage;
        }
    }
}
