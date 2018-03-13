using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DefaultPlayer{

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



        /*
        // 본인의 클라이언트면 Run 애니메이션을 막아버린다.
        else
        {


            // 다시 RPC를 받을 이유 없이 클라이언트 스스로 같은 시간 후에 꺼버린다. 
            // 다시 받을 이유도 없어서 레이턴씨로 인한 시간이 단축된다.
            StartCoroutine("KnockBackCoroutineOtherClient");
        }
        */

    }
    /*
    [PunRPC]
    void ApplyDamage(float _damage)
    {
        if (pv.isMine)
        {
           // NowHealth -= _damage;
        }
    }*/
}
