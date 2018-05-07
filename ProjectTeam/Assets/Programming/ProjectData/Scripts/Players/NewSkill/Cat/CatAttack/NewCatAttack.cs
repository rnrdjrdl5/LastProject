using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*************************************
 * 주의사항
 * 이펙트 사용시 한칸 씩 밀리도록 설게되어 있으니
 * 애니메이션 이벤트 사용 시 
 * 이펙트 출력 하려면 +1 위치를 출력시키도록. 
 * ****************************************/



public class NewCatAttack : DefaultNewSkill{

    // 공격에 대한 정보가 있는 스크립트입니다.
    public AttackState attackState;


    // 프라이팬 오브젝트를 외부로부터 받아옵니다.
    public GameObject FryPan;

    // 프라이팬에 부여한 스크립트입니다.
    private CollisionObject collisionObject;
    private CollisionObjectDamage collisionObjectDamage;

    // 공격 이펙트 타입입니다. AttackAnimator에서 만집니다.
    private PoolingManager.EffctType AttackEffectType = PoolingManager.EffctType.ATTACKLINE1;

    public PoolingManager.EffctType GetAttackEffectType() { return AttackEffectType; }
    public void SetAttackEffectType(PoolingManager.EffctType ET)
    {
        AttackEffectType = ET;
    }



    protected override void Update()
    {
        base.Update();

        if (isUseAttackKey == true &&
            isCanNextAttackAni == true)
        {

            // 여기에 USESKILL 사용하자.
            Debug.Log("3");

            // 1. 현재 공격중이라고 변경합니다.
            playerState.SetisUseAttack(true);


            // 2. 공격  키 입력을 취소
            isUseAttackKey = false;

            // 3. 공격 애니메이션 이벤트를 꺼버림.
            isCanNextAttackAni = false;



            // 다음 애니메이션으로 이동 , 이펙트 사용.
            photonView.RPC("RPCisAttack", PhotonTargets.All , (int)AttackEffectType);
        }

    }

    private bool isUseAttackKey = false;                // 공격을 한지 여부입니다. 애니메이션 이벤트에서 판단합니다.

    public bool GetisUseAttackKey() { return isUseAttackKey; }
    public void SetisUseAttackKey(bool UAK)
    {
        isUseAttackKey = UAK;
    }

    private bool isCanNextAttackAni = true;             // 다음 애니메이션으로 넘어갈 수 있는지에 대한 여부.

    public bool GetisCanNextAttackAni() { return isCanNextAttackAni; }
    public void SetisCanNextAttackAni(bool CNAA)
    {
        isCanNextAttackAni = CNAA;
    }







    protected override void Awake()
    {
        base.Awake();

        defaultCdtAct = new NormalCdtAct();
        defaultCdtAct.InitCondition(this);
    }
    
    


    // 공격 조건을 재정의합니다.
    public override bool CheckState()
    {

        // 공격했다면.
        if ((playerState.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
        playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN) ||
        playerState.EqualPlayerCondition(PlayerState.ConditionEnum.JUMP)))
        {
            return true;
        }
        return false;

    }


    // 공격 액션을 재정의합니다.
    // Update로 대신해서 처리합니다.
    public override void UseSkill()
    {
        isUseAttackKey = true;

    }

    // 상황에 맞는 이펙트 출력




    // RPC입니다.
    [PunRPC]
    void RPCisAttack(int attackCount)
    {
        Debug.Log("4");
        // 다음 공격 애니메이션 진행
        animator.SetBool("isAttack", true);



        // 상태에 따라 오브젝트 풀에서 빼오기.
        Debug.Log(" 이펙트 숫자 : "  + attackCount);
        GameObject effectObject = PoolingManager.GetInstance().CreateEffect((PoolingManager.EffctType)attackCount);

        // 이펙트 위치 지정
        effectObject.transform.position = transform.position;
        effectObject.transform.rotation = transform.rotation;
        effectObject.transform.SetParent(transform);

        // 이펙트 진행
        effectObject.GetComponent<Animator>().SetBool("UseAction",true);
    }





    // 이벤트

    void CreateFryPanOption()
    {

        // 모두다 실행한다. 하지만  특정 사람만 준다. 

        // 프라이팬에 데미지와 정보 스크립트를 추가합니다.
        collisionObjectDamage = FryPan.AddComponent<CollisionObjectDamage>();
        collisionObject = FryPan.AddComponent<CollisionObject>();

        // 정보 스크립트에 수치를 대입합니다.
        attackState.SetData(collisionObjectDamage, collisionObject, photonView , PhotonNetwork.player.ID,gameObject);

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

    public void OnNextAttackAni(int i)
    {
        // 실질적으로 다음 애니메이션에 적용되는 이펙트임.
        Debug.Log(" 이펙트 숫자 설정 , 값 : " + i);

        // 여기서 처음 ON으로 잡아놨을때 겹쳐버려서 .
        isCanNextAttackAni = true;
        AttackEffectType = (PoolingManager.EffctType)i;
    }

    
}
