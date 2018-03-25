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

    // 스킬의 유지 비용을 결정합니다. 초당 비용입니다.
    public float CtnManaCost;


    // 스크립트내에서 사용할 변수를 선언합니다.
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

    // 스킬 조건을 가지는 스크립트입니다.
    public SkillConditionOption skillConditionOption;

    // 지속형 조건을 가지는 스크립트입니다.
    public SkillContinueConditionOption skillContinueConditionOption;



    // 키 값의 종류를 가지는 스크립트입니다.
    // 조건 스크립트에서 이 변수로 키를 확인합니다.
    public DefaultInput InputKey;

    // 키 값의 종류를 가지는 스크립트입니다.
    // 조건 스크립트에서 이 변수로 키를 확인합니다.
    public DefaultInput ExitInputKey;


    // 디버프 종류입니다.
    // 리스트 내의 디버프에 따라 투사체나 근접공격에 효과를 부여합니다.
    public List<DefaultPlayerSkillDebuff> PlayerSkillDebuffs;


   


    virtual protected void Awake()
    {
        
        // 기본설정을 합니다.
        SettingBaseOption();


        // 스킬 조건을 판단하는 스크립트에 이 스크립트를 전달합니다.
        // ( 조건 문 사용 시 변수 사용 용도 )
        skillConditionOption.InitSkillConditionOption(this);
        skillContinueConditionOption.InitSkillConditionOption(this);


    }



    protected void Update()
    {
        // 해당 조건에 맞는다면 , 
        if (skillConditionOption.CheckCondition())
        {

            //5. 상태가 맞을 때
            if (CheckState())
            {
                // 스킬 사용.
                // 쭉 누르는 스킬같은 경우 UseSkill이 계속 작동됨
                // 그래서 막아버림.
                if (skillContinueConditionOption.GetskillConditionContinueOption().GetisUseCtnSkill() == false)
                {

                    UseSkill();


                    // 지속형 스킬을 on 합니다.
                    // 지속형 스킬 if문을 돌아가게 합니다.
                    // 지속형 스킬인경우만.

                    if ((skillContinueConditionOption.skillContinueConditionType !=
                        SkillContinueConditionOption.EnumSkillContinueConditionOption.NONE))

                    {
                        skillContinueConditionOption.GetskillConditionContinueOption().SetisUseCtnSkill(true);
                    }
                }


                // 쿨타임을 적용합니다.
                // 재사용 대기시간 감소 효과 등을 고려해서 
                // 함수로 사용합니다.

                // 단, 채널링 기술 사용 시 쿨타임이 적용되지 않습니다.
                if (skillContinueConditionOption.skillContinueConditionType ==
                    SkillContinueConditionOption.EnumSkillContinueConditionOption.NONE)
                {
                    coolDown.CalcCoolDown();

                    coolDown.SetisUseCoolDown(true); // 쿨타임 돌아갈지 여부 판단

                    // 마나 감소를 적용합니다.
                    // 마나감소 효과를 고려해서
                    // 함수로 사용합니다.
                    manaPoint.CalcManaPoint(ManaCost);
                }
            }


        }

        // 해당 지속형 조건에 맞는다면 
        if (skillContinueConditionOption.CheckContinueCondition())
        {
            // 지속형 스킬을 사용 하는  데 플레이어 상태가 맞다면.
            if (CheckCtnState())
            {
                // 지속형 스킬을 사용합니다. 
                UseCtnSkill();

                //마나감소
                manaPoint.CalcManaPoint(CtnManaCost * Time.deltaTime);
                Debug.Log("채널링중");
            }
            else
            {
                ExitCtnSkill();
                skillContinueConditionOption.GetskillConditionContinueOption().SetisUseCtnSkill(false);
            }


        }


        //지속형 스킬을 사용할 때 마나가 충분한가
        if (skillContinueConditionOption.skillContinueConditionType !=
             SkillContinueConditionOption.EnumSkillContinueConditionOption.NONE)
        {
            if (photonView.isMine)
            {
                if (!CanChannelingSkillMana())
                {
                    ExitCtnSkill();
                    skillContinueConditionOption.GetskillConditionContinueOption().SetisUseCtnSkill(false);


                    coolDown.CalcCoolDown();

                    coolDown.SetisUseCoolDown(true); // 쿨타임 돌아갈지 여부 판단
                }
            }
        }

        // 해제 조건을 체크합니다.
        // 에러발견

        if (skillContinueConditionOption.GetskillConditionContinueOption().GetisUseCtnSkill() == true)
        {
            if (skillContinueConditionOption.CheckContinueExit())
            {
                if (CheckCtnState())
                {
                    ExitCtnSkill();
                    skillContinueConditionOption.GetskillConditionContinueOption().SetisUseCtnSkill(false);
                }
            }
        }
            // 쿨타임을 줄여나간다.
            coolDown.DecreaseCoolDown();
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


    bool CanChannelingSkillMana()
    {

            // 2. 스킬 사용중인가?
            if (skillContinueConditionOption.GetskillConditionContinueOption().GetisUseCtnSkill() == true)
            {

                //3. 마나가 다 닳았는가
                if (GetmanaPoint().GetNowManaPoint() < CtnManaCost * Time.deltaTime)
                {
                    return false;
                }
            }
        return true;
    }

    /************************  아래부터는 재정의 해야 할 목록 *******************/

    // 현재 상태를 체크합니다.
    protected virtual bool CheckState()
    {
        Debug.Log("부모");
        return false;
    }

    // 현재 지속형 스킬을 위한 상태를 체크합니다.
    protected virtual bool CheckCtnState()
    {
        Debug.Log("부모");
        return false;
    }


    // 스킬 사용입니다.
    protected virtual void UseSkill()
    {
        Debug.Log("부모");
    }

    // 지속형 스킬 사용입니다.
    protected virtual void UseCtnSkill()
    {
        Debug.Log("부모");
    }

    // 지속형 스킬 사용의 해제입니다.
    protected virtual void ExitCtnSkill()
    {
        Debug.Log("부모");
    }




    // 디버프를 해당 오브젝트에 추가한다.

    // 
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
