using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RionRush : DefaultDamageGroggyScript
{

    public override bool CheckState()
    {
        if(EqualState(PlayerState.ConditionEnum.IDLE) ||
          EqualState(PlayerState.ConditionEnum.RUN))
        {
            return true;
        }
        else
            return false;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public override void UseSkill()
    {
        Debug.Log("스킬사용 끝.");
        photonView.RPC("RPCRionRushAttackAnimation", PhotonTargets.All);
    }

    

    /******* 아래부터는 RPC입니다. ******/
    [PunRPC]
    void RPCRionRushAttackAnimation()
    {
        gameObject.GetComponent<Animator>().SetTrigger("RionRushAttackTrigger");
    }

    // 부모 스크립트에 RPC 사용있음. RPC는 상속이 되지 않아 재구현함.
    [PunRPC]
    void RPCStartFirstAnimation()
    {
        gameObject.GetComponent<Animator>().SetTrigger("RionRushTrigger");
    }


    /****** 아래부터는 애니메이션 이벤트 입니다.*****/

    GameObject CheckGameObject;
    public GameObject GetCheckGameObject() { return CheckGameObject; }
    public void SetCheckGameObject(GameObject GO) { CheckGameObject = GO; }

    void CreateRionRush()
    {
        if (gameObject.GetComponent<PlayerState>().EqualPlayerCondition(PlayerState.ConditionEnum.RIONRUSH))
        {
            CheckGameObject = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
            CheckGameObject.GetComponent<MeshRenderer>().enabled = false;
            CheckGameObject.GetComponent<Transform>().localScale = Vector3.one * AroundObjectRadius;
            SetCollisionData(CheckGameObject, this);
            Debug.Log("asdf");
        }

    }

    void DeleteRionRush()
    {
        if(gameObject.GetComponent<PlayerState>().EqualPlayerCondition(PlayerState.ConditionEnum.RIONRUSH))
        {
            Destroy(CheckGameObject);
            Debug.Log("삭제!!");
        }
    }


}
