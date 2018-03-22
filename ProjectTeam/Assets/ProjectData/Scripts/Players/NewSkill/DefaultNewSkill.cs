using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 2018. 03. 22 반재억
 *  스킬 개편 스크립트
 *  모든 조건들을 따로 관리합니다.
 *  컴포넌트 화 시킵니다. */

public class DefaultNewSkill : MonoBehaviour {

    // 외부에서 수정할 수 있는 목록들입니다.
    // 모든 스킬에 포함됩니다.

    // 스킬의 쿨타임을 결정합니다.
    public CoolDown coolDown;

    // 스킬의 비용을 결정합니다.
    public float ManaCost;

    // 스킬의 옵션이 결정됩니다.
    // 스킬에서 사용하지 않는 옵션은 조정해봤자 의미 없습니다.
    public SkillState skillState;


    // 스크립트내에서 사용할 변수를 선언합니다.
    public PhotonView photonView;
    public PlayerState playerState;
    public PlayerManaPoint manaPoint;
    public Animator animator;

    // 스킬 조건을 가지는 스크립트입니다.
    protected DefaultSkillCondition skillCondition;



    /****************************************************************************
    *
    * 해당 속성들은 스크립트로 설정해야합니다. 인스펙터에서 설정이 불가능합니다.
    * 키입력의 형태입니다. 하위에서 설정합시다.
    * 
    * ***********************************************************************/
    public DefaultInput InputKey;
    

    protected List<DefaultSkillDebuff> SkillDebuffs;


   


    virtual protected void Awake()
    {
        // 기본설정을 합니다.
        photonView = gameObject.GetComponent<PhotonView>();
        playerState = gameObject.GetComponent<PlayerState>();
        manaPoint = gameObject.GetComponent<PlayerManaPoint>();
        animator = gameObject.GetComponent<Animator>();

        
    }

    protected void Update()
    {
        // 해당 조건에 맞는다면 , 
        if (skillCondition.SkillCondition())
        {

            //5. 상태가 맞을 때
            if (CheckState())
            {
                // 스킬 사용.
                UseSkill();

                /******************************************
                // 스킬 오브젝트에 디버프 적용시키기.
                // 리스트로 관리하자.

                // + 일반 오브젝트 데이터도 리스트로 관리 가능할꺼다. 
                /*********************************************/

                // 쿨타임을 적용합니다.
                // 재사용 대기시간 감소 효과 등을 고려해서 
                // 함수로 사용합니다.

                coolDown.CalcCoolDown();
                coolDown.SetisUseCoolDown(true);

                // 마나 감소를 적용합니다.
                // 마나감소 효과를 고려해서
                // 함수로 사용합니다.
                manaPoint.CalcManaPoint(ManaCost);
            }
        }

        // 쿨타임을 줄여나간다.
        coolDown.DecreaseCoolDown();
    }



    // SkillType에서 필요한 데이터를 골라 오브젝트 데이터를 설정합니다.
    protected void SetObjectData(GameObject CollisionGameObject)
    {
        if (skillState.GetSkillRangeType() == SkillState.SkillType.Projectile)
        {
            CollisionObject CollisionObjectScript = CollisionGameObject.GetComponent<CollisionObject>();

            CollisionObjectDamage CollisionObjectDamageScript = CollisionGameObject.GetComponent<CollisionObjectDamage>();

            CollisionObjectTime CollisionObjectTimeScript = CollisionGameObject.GetComponent<CollisionObjectTime>();

            NumberOfCollisions NumberOfCollisionsScript = CollisionGameObject.GetComponent<NumberOfCollisions>();

            CollisionMove CollisionMoveScript = CollisionGameObject.GetComponent<CollisionMove>();

            if (CollisionObjectScript != null)
            {
                CollisionObjectScript.SetCollisionReCheckTime(skillState.ReCheckTime);
                CollisionObjectScript.SetUsePlayer("Player" + photonView.viewID);
            }

            if (CollisionObjectDamageScript != null)
            {
                CollisionObjectDamageScript.SetObjectDamage(skillState.ObjectDamage);
                CollisionObjectDamageScript.SetObjectDamageNumber(skillState.ObjectDamageNumber);
            }

            if (CollisionObjectTimeScript != null)
            {
                CollisionObjectTimeScript.SetObjectTime(skillState.DestroyTime);
            }

            if (NumberOfCollisionsScript != null)
            {
                NumberOfCollisionsScript.SetNumberOfCollisions(skillState.CollisionNumber);
            }

            if (CollisionMoveScript)
            {
                CollisionMoveScript.SetCollisionMoveSpeed(skillState.ObjectSpeed);
                CollisionMoveScript.SetCollisionMoveDirect(transform.forward.normalized);
            }
        }

        else
            Debug.Log("오류");



    }



    /************************  아래부터는 재정의 해야 할 목록 *******************/

    // 현재 상태를 체크합니다.
    protected virtual bool CheckState()
    {
        Debug.Log("부모");
        return false;
    }


    // 스킬 사용입니다.
    protected virtual void UseSkill()
    {
        Debug.Log("부모");
    }




}
