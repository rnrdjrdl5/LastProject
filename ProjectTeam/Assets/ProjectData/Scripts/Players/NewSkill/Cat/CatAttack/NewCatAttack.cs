using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCatAttack : DefaultNewSkill {

    // 공격에 대한 정보가 있는 스크립트입니다.
    public AttackState attackState;


    // 프라이팬 오브젝트를 외부로부터 받아옵니다.
    public GameObject FryPan;

    // 공격 할 수 있는지 판단합니다.
    private bool isCanAttack = true;

    public bool GetisCanAttack() { return isCanAttack; }
    public void SetisCanAttack(bool CA) { isCanAttack = CA; }



    // 프라이팬에 부여한 스크립트입니다.
    private CollisionObject collisionObject;
    private CollisionObjectDamage collisionObjectDamage;



    // 공격 조건을 재정의합니다.
    protected override bool CheckState()
    {

        if ((playerState.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
            playerState.EqualPlayerCondition(PlayerState.ConditionEnum.ATTACK) ||
            playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN))
            && (isCanAttack))

        {
            return true;
        }

        else
            return false;
    }


    // 공격 액션을 재정의합니다.
    protected override void UseSkill()
    {
        isCanAttack = false;
        animator.SetBool("isAttack", true);
        photonView.RPC("RPCisAttack", PhotonTargets.Others);

    }


    // RPC입니다.

    [PunRPC]
    void RPCisAttack()
    {
        animator.SetBool("isAttack", true);
    }


    // 애니메이션 이벤트입니다.
    void ResetCanAttack()
    {
        isCanAttack = true;
    }

    void CreateFryPanOption()
    {
        // 프라이팬에 데미지와 정보 스크립트를 추가합니다.
        collisionObjectDamage = FryPan.AddComponent<CollisionObjectDamage>();
        collisionObject = FryPan.AddComponent<CollisionObject>();

        // 정보 스크립트에 수치를 대입합니다.
        attackState.SetData(collisionObjectDamage, collisionObject, photonView);

        // 디버프를 추가합니다. 공격에서는 경직 디버프가 들어있습니다.
        AddDebuffComponent(FryPan);
    }

    // 프라이팬 정보를 삭제합니다.
    public void DeleteFryPanOption()
    {
        // 데미지가 있다면 삭제합니다.
        if (collisionObjectDamage != null)
            Destroy(collisionObjectDamage);

        // 정보가 있다면 삭제합니다.
        if(collisionObject != null)
            Destroy(collisionObject);

        // 데미지 디버프가 있다면 저장합니다.
        CollisionDamagedDebuff CDD = FryPan.GetComponent<CollisionDamagedDebuff>();

        // 저장한 디버프가존재한다면 
        if(CDD!= null)
        {
            // 삭제합니다.
            Destroy(CDD);
        }

        // 모든 체크 대상을 불러옵니다.
        CollisionReCheck[] collisionRechecks = FryPan.GetComponents<CollisionReCheck>();

        // 루프를 돌면서 삭제합니다.
        for (int i = collisionRechecks.Length-1; i >= 0; i--)
        {
            Destroy(collisionRechecks[i]);
        }
    }
}
