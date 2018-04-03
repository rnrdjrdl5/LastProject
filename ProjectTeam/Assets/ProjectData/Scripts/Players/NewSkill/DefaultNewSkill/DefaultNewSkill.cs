using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 2018. 03. 22 반재억
 *  스킬 개편 스크립트
 *  모든 조건들을 따로 관리합니다.
 *  컴포넌트 화 시킵니다. */

/* 2018. 04.02 반재억
 *  조건문의 복잡함, 이양화가 힘들기 때문에
 *  조건 통째로 빼버립니다. */

public class DefaultNewSkill : MonoBehaviour {


    /**** public ****/

    public CoolDown coolDown;               // 스킬 쿨타임
    public DefaultInput InputKey;               // 키 입력 조건
    public DefaultInput ExitInputKey;           // 키 탈출 조건

    public float ManaCost;              // 스킬 비용
    public float CtnManaCost;               // 스킬 유지 비용

    protected DefaultConditionAction defaultCdtAct;               //조건과 스킬을 가지는 스크립트

    public List<DefaultPlayerSkillDebuff> PlayerSkillDebuffs;               // 디버프 종류


    /**** protected ****/

    protected PhotonView photonView;
    public PhotonView GetphotonView() { return photonView; }
    public void SetphotonView(PhotonView pv) { photonView = pv; }

    protected PlayerState playerState;
    public PlayerState GetplayerState() { return playerState; }
    public void SetplayerState(PlayerState ps) { playerState = ps; }

    protected PlayerManaPoint manaPoint;
    public PlayerManaPoint GetmanaPoint() { return manaPoint; }
    public void SetmanaPoint(PlayerManaPoint mp) { manaPoint = mp; }

    protected Animator animator;






   


    virtual protected void Awake()
    {
        
        // 기본설정
        SettingBaseOption();

        coolDown.DecreaseCoolDown();

    }



    protected void Update()
    {
        defaultCdtAct.ConditionAction();

    }

    // 기본 설정을 합니다.
    void SettingBaseOption()
    {
        // 사용에 필요한 변수들 게임오브젝트로부터 받아옵니다.
        photonView = gameObject.GetComponent<PhotonView>();
        playerState = gameObject.GetComponent<PlayerState>();
        manaPoint = gameObject.GetComponent<PlayerManaPoint>();
        animator = gameObject.GetComponent<Animator>();
    }




    /************************  아래부터는 재정의 해야 할 목록 *******************/

    // 현재 상태를 체크합니다.
    public virtual bool CheckState()
    {
        Debug.Log("부모");
        return false;
    }

    // 현재 지속형 스킬을 위한 상태를 체크합니다.
    public virtual bool CheckCtnState()
    {
        Debug.Log("부모");
        return false;
    }


    // 스킬 사용입니다.
    public virtual void UseSkill()
    {
        Debug.Log("부모");
    }

    // 지속형 스킬 사용입니다.
    public virtual void UseCtnSkill()
    {
        Debug.Log("부모");
    }

    // 지속형 스킬 사용의 해제입니다.
    public virtual void ExitCtnSkill()
    {
        Debug.Log("부모");
    }




    // 디버프를 해당 오브젝트에 추가한다.
    protected void AddDebuffComponent(GameObject CollisionObject)
    {
        for(int i = 0; i < PlayerSkillDebuffs.Count; i++)
        {
            if (PlayerSkillDebuffs[i].EqualSkillDebuffType(DefaultPlayerSkillDebuff.EnumSkillDebuff.STUN))
            {
                CollisionStunDebuff CSD = CollisionObject.AddComponent<CollisionStunDebuff>();
                CSD.SetMaxTime(PlayerSkillDebuffs[i].GetMaxTime());
            }

            else if(PlayerSkillDebuffs[i].EqualSkillDebuffType(DefaultPlayerSkillDebuff.EnumSkillDebuff.DAMAGED))
            {
                CollisionDamagedDebuff CDD = CollisionObject.AddComponent<CollisionDamagedDebuff>();
                CDD.SetMaxTime(PlayerSkillDebuffs[i].GetMaxTime());
            }
                
        }
    }



}
