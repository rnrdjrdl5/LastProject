using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCdtAct : DefaultConditionAction
{
    public override void InitCondition(DefaultNewSkill DNS)
    {
        // 스킬 조건 스크립트 초기화
        base.InitCondition(DNS);

        // 스킬 스크립트 설정
        SetdefaultNewSkill(DNS);

        // 조건 설정
        skillConditionOption.SetskillCdtOpt(new NormalConditionOption());

        // 자식 스크립트에 skill 설정
        skillConditionOption.InitDefaulSkill(DNS);      

        //조건 설정
        skillCtnCdtOption.SetskillConditionContinueOption(new ChannelingCtnCdtOption());

        // 자식 스크립트에 skill 설정
        skillCtnCdtOption.InitDefaultSkill(DNS);

    }

    public override void ConditionAction()
    {

        // 스킬 시작 조건 판단
        if(skillConditionOption.CheckCondition())
        {

            // 플레이어 상태확인
            if(defaultNewSkill.CheckState())
            {
                
                // 지속성 스킬 여부 확인
                if(skillCtnCdtOption.GetskillConditionContinueOption().GetisUseCtnSkill() == false)
                {

                    // 스킬 사용
                    defaultNewSkill.UseSkill();

                    // 지속성 스킬 사용
                    skillCtnCdtOption.GetskillConditionContinueOption().SetisUseCtnSkill(true);
                }
            }
        }

        // 지속성 스킬 조건 판단
        if(skillCtnCdtOption.CheckContinueCondition())
        {

           if(defaultNewSkill.CheckCtnState())
            {

                // 지속성 스킬을 사용합니다.
                defaultNewSkill.UseCtnSkill();

                // 마나 감소
                defaultNewSkill.GetmanaPoint().CalcManaPoint(defaultNewSkill.CtnManaCost * Time.deltaTime);
                Debug.Log("채널링중");
            }
            else
            {
                // 퇴장스킬
                defaultNewSkill.ExitCtnSkill();

                // 스킬 사용중 해제
                skillCtnCdtOption.GetskillConditionContinueOption().SetisUseCtnSkill(false);
            }
        }

        // 마나 충분한지 여부
        if(defaultNewSkill.GetphotonView().isMine)
        {

            // 2. 스킬 사용중인가?
            if (skillCtnCdtOption.GetskillConditionContinueOption().GetisUseCtnSkill() == true)
            {

                //3. 마나가 다 닳았는가
                if (defaultNewSkill.GetmanaPoint().GetNowManaPoint() < defaultNewSkill.CtnManaCost * Time.deltaTime)
                {

                    defaultNewSkill.ExitCtnSkill();
                    skillCtnCdtOption.GetskillConditionContinueOption().SetisUseCtnSkill(false);

                    defaultNewSkill.coolDown.CalcCoolDown();

                    defaultNewSkill.coolDown.SetisUseCoolDown(true);
                }
            }
        }
        
        //스킬 사용중 체크
        if(skillCtnCdtOption.GetskillConditionContinueOption().GetisUseCtnSkill() == true)
        {

            // 스킬 해제 조건 체크
            if(skillCtnCdtOption.CheckContinueExit())
            {

                //  상태판단
                if(defaultNewSkill.CheckCtnState())
                {

                    defaultNewSkill.ExitCtnSkill();
                    skillCtnCdtOption.GetskillConditionContinueOption().SetisUseCtnSkill(false);
                }
            }
        }
    }
}
