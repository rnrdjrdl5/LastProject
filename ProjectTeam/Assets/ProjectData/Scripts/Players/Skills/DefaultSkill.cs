using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSkill : Photon.PunBehaviour {

    public enum EnumSkillKey { LEFTMOUSE , RIGHTMOUSE , LEFTSHIFT , RIGHTSHIFT , SPACE , Q , E ,
        LEFTSHIFTUP , LEFTSHIFTDOWN , PUSINGLEFTSHIFT}

    [Header(" - 키 설정")]
    [Tooltip(" - 해당 기술 사용 시 키 입력")]
    public EnumSkillKey SkillKeyType;

    [Header(" - 쿨 타임")]
    [Tooltip(" - 스킬 사용 후 쿨타임")]
    public float SkillMaxWaitingTime;

    [Header(" - 소모 마나")]
    [Tooltip(" - 스킬을 사용하는 데 필요한 마나입니다.")]
    public float SkillManaCost;



    protected float SkillNowWaitingTime;

    public float GetSkillNowWaitingTime() { return SkillNowWaitingTime; }
    public void SetSkillNowWaitingTime(float SNWT) { SkillNowWaitingTime = SNWT; }

    // protected DefaultSkill BaseScript = null;


    // Use this for initialization
    virtual protected void Start () {
		
	}

    virtual protected void Awake()
    {
        SkillNowWaitingTime = 0.0f;
    }
	
	// Update is called once per frame
	virtual protected void Update () {
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
                                DecreaseMana();
                                UseSkillWaitingTime();
                                UseSkill();
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
                if(SkillNowWaitingTime <= 0)
                {
                    Debug.Log("스킬쿨타임중." + SkillNowWaitingTime);
                    SkillNowWaitingTime = 0;
                }
            }


        }

	}

    protected void UseSkillWaitingTime()
    {
        SkillNowWaitingTime = SkillMaxWaitingTime;
    }



    protected virtual void DecreaseMana()
    {
        gameObject.GetComponent<PlayerManaPoint>().DecreaseMana(SkillManaCost);
    }


    protected virtual bool HaveManaCost()
    {
        if (gameObject.GetComponent<PlayerManaPoint>().GetNowManaPoint() >= SkillManaCost)
            {
                return true;
            }
            else
                return false;
    }



    // 마우스 클릭이 왼쪽클릭일 때, 혹시나 스킬사용중인지 판단한다.
    // 타겟팅 스킬의 정보를 PlayerState에 저장, 여기서 판단.
    protected bool UseMouseSkill()
    {
        if (SkillKeyType == EnumSkillKey.LEFTMOUSE)
        {
            if (gameObject.GetComponent<PlayerState>().EqualSkillCondition(PlayerState.ConditionEnum.NONE))
            {
                return true;
            }
            else
                return false;
        }
        else
            return true;
    }

    virtual public bool CheckState()
    {
        Debug.Log("부모 스크립트입니다.");
        return false;
    }

    virtual public void UseSkill()
    {
        Debug.Log("부모 스크립트입니다.");
    }

    public bool CheckSkillKey()
    {
        bool ReturnType = false;
        switch (SkillKeyType)
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
            case EnumSkillKey.PUSINGLEFTSHIFT:
                ReturnType = Input.GetKey(KeyCode.LeftShift);
                break;

        }
        return ReturnType;
    }

    public bool EqualState(PlayerState.ConditionEnum CE)
    {
        if (gameObject.GetComponent<PlayerState>().GetPlayerCondition() == CE)
            return true;
        else
            return false;
    }


}
