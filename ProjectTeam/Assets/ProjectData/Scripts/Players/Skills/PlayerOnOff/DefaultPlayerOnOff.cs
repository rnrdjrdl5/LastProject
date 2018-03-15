using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPlayerOnOff : DefaultSkill {

    [Header(" - 끄는 키 버튼")]
    [Tooltip(" - 해당 스킬 오프 버튼 설정.")]
    public EnumSkillKey SkillOffKeyType;

    // 스킬 온오프여부.
    protected bool isSkillOnOff = false;


    protected override void Update()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            if (SkillNowWaitingTime <= 0)
            {
                if (HaveManaCost())
                {
                    if (CheckSkillKey())
                    {
                        if (CheckState())
                        {
                            if (UseMouseSkill())
                            {
                                if (isSkillOnOff == false)
                                {
                                    Debug.Log("스킬사용");
                                    UseSkill();
                                    isSkillOnOff = true;
                                }
                                
                            }

                        }
                    }
                }

                else
                {
                    Debug.Log("마나부족 ");
                }
            }

            else
            {
                SkillNowWaitingTime -= Time.deltaTime;
                if (SkillNowWaitingTime <= 0)
                {
                    Debug.Log("스킬쿨타임중." + SkillNowWaitingTime);
                    SkillNowWaitingTime = 0;
                }
            }



            if (isSkillOnOff)
            {
                if (!HaveManaCost() || !CheckState() || CheckSkillOffKey())
                {
                    isSkillOnOff = false;
                    UseOffSkill();
                    if (!HaveManaCost())
                    {
                        UseSkillWaitingTime();
                    }
                }

                else
                {
                    Debug.Log("채널링상태");
                    ChannelingSkill();
                    DecreaseMana();
                }
                // 1. 마나가 없거나
                // 2. 상태값이 달라졌거나.
                // 3. 직접 취소했거나.
            }
           




        }

        // 다른 클라이언트. 채널링 시 같은 효과를 줘야 한다. 모습만?
        else
        {
            if(isSkillOnOff)
            {
                ChannelingSkill();
            }
        }
    }

    protected override bool HaveManaCost()
    {
        if (gameObject.GetComponent<PlayerManaPoint>().GetNowManaPoint() >= SkillManaCost * Time.deltaTime)
        {
            return true;
        }
        else
            return false;
    }

    protected override void DecreaseMana()
    {
        gameObject.GetComponent<PlayerManaPoint>().DecreaseMana(SkillManaCost * Time.deltaTime);
    }

    protected virtual void ChannelingSkill()
    {
        Debug.Log("채널스킬, 부모");
    }


    protected virtual void UseOffSkill()
    {
        Debug.Log("꺼짐 부모");
    }


    protected virtual bool CheckSkillOffKey()
    {
        bool ReturnType = false;
        switch (SkillOffKeyType)
        {
            case EnumSkillKey.LEFTMOUSE:
                ReturnType = Input.GetMouseButtonUp(0);
                break;

            case EnumSkillKey.RIGHTMOUSE:
                ReturnType = Input.GetMouseButtonUp(1);
                break;

            case EnumSkillKey.LEFTSHIFT:
                ReturnType = Input.GetKeyUp(KeyCode.LeftShift);
                break;

            case EnumSkillKey.LEFTSHIFTDOWN:
                ReturnType = Input.GetKeyDown(KeyCode.LeftShift);
                break;

            case EnumSkillKey.LEFTSHIFTUP:
                ReturnType = Input.GetKeyUp(KeyCode.LeftShift);
                break;

            case EnumSkillKey.SPACE:
                ReturnType = Input.GetKeyUp(KeyCode.Space);
                break;
            case EnumSkillKey.Q:
                ReturnType = Input.GetKeyUp(KeyCode.Q);
                break;
            case EnumSkillKey.E:
                ReturnType = Input.GetKeyUp(KeyCode.E);
                break;

        }
        return ReturnType;
    }

}



