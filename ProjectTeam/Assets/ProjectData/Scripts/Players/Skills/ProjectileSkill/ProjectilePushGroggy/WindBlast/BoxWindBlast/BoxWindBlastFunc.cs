using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BoxWindBlast
{

    override public bool CheckState()
    {
        if ((EqualState(PlayerState.ConditionEnum.RUN) ||
            EqualState(PlayerState.ConditionEnum.IDLE)))
        {
            return true;
        }
        else
            return false;
    }


    public override void UseSkill()
    {
        gameObject.GetComponent<Animator>().SetTrigger("WindBlastTrigger");
        gameObject.GetComponent<PhotonView>().RPC("WindBlastAnimation", PhotonTargets.Others);
    }







    /******************** 아래 부터는 RPC 입니다. ***************************/

    [PunRPC]
    void WindBlastAnimation()
    {
        gameObject.GetComponent<Animator>().SetTrigger("WindBlastTrigger");
    }



    /******************** 아래 부터는 애니메이션 이벤트 입니다. ************************/

    void CreateWindBlast()
    {
            float BulletDistance = 1.0f;
            float CharacterHeight = 1.2f;

            Vector3 BulletDefaultPlace = transform.forward * BulletDistance;
            BulletDefaultPlace.y += CharacterHeight;

            GameObject TGO = Instantiate(BulletPrefab, transform.position + (BulletDefaultPlace), Quaternion.identity);

            SetCollisionData(TGO, this);
    }

}
