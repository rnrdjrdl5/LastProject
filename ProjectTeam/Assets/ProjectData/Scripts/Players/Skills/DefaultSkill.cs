using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSkill : Photon.PunBehaviour {

    public enum EnumSkillKey { LEFTMOUSE , RIGHTMOUSE , LEFTSHIFT , RIGHTSHIFT , SPACE , Q , E , PUSHINGLEFTSHIFT}

    [Header(" - 키 설정")]
    [Tooltip(" - 해당 기술 사용 시 키 입력")]
    public EnumSkillKey SkillKeyType;


    // protected DefaultSkill BaseScript = null;
    

    // Use this for initialization
    virtual protected void Start () {
		
	}

    virtual protected void Awake()
    {

    }
	
	// Update is called once per frame
	virtual protected void Update () {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            if (CheckSkillKey())
            {
                if (CheckState())
                {
                    if (UseMouseSkill())
                    {

                        UseSkill();
                    }
                    
                }
            }
        }

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

            case EnumSkillKey.PUSHINGLEFTSHIFT:
                ReturnType = Input.GetKey(KeyCode.LeftShift);
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

    public bool EqualState(PlayerState.ConditionEnum CE)
    {
        if (gameObject.GetComponent<PlayerState>().GetPlayerCondition() == CE)
            return true;
        else
            return false;
    }


}
