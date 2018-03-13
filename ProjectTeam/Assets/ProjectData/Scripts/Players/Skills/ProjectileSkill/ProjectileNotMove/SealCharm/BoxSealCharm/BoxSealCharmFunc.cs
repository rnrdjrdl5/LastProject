using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BoxSealCharm
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
        gameObject.GetComponent<Animator>().SetTrigger("SealCharmTrigger");
        gameObject.GetComponent<PhotonView>().RPC("SealCharmAnimation", PhotonTargets.Others);
    }


    /************************************************************************************************
     * Math 스크립트를 따로 만들어서 사용해야 합니다. 
     * 겹치는 부분이 많습니다. 
     * 해당 Math 스크립트는 컴포넌트에 포함되는 방식이 아닌 새롭게 객체형태로 생성해서 사용합시다.
     * ************************************************************************************************/
    void CreateCharm()
    {
        
            Debug.Log("asdf");
            float BulletDistance = 1.0f;
            float CharacterHeight = 1.2f;

            Vector3 BulletDefaultPlace = transform.forward * BulletDistance;

            BulletDefaultPlace.y += CharacterHeight;

            GameObject CharmBullet = Instantiate(BulletPrefab, transform.position + (BulletDefaultPlace), Quaternion.identity);

            SetCollisionData(CharmBullet, this);
    }

    /***********************************
    * 여기서부터는 RPC입니다. *************
    * ***************************************/


    [PunRPC]
    void SealCharmAnimation()
    {
        gameObject.GetComponent<Animator>().SetTrigger("SealCharmTrigger");
    }
}


