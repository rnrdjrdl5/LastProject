﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillConditionOption{

    // 스킬 조건을 외부에서 정할 수 있도록 합니다.
    public enum EnumSkillConditionOption { NORMAL }

    public EnumSkillConditionOption SkillConditionType;



    // 실제로 스킬 조건을 판단합니다.
    private DefaultSkillConditionOption skillConditionOption;



    // 처음으로 열거형에 따라서 스킬 조건 스크립트를 초기화합니다.
    public void InitSkillConditionOption(DefaultNewSkill DS)
    {
        // 1. 조건 스크립트를 초기화합니다.
        if (EqualSkillConditionType(EnumSkillConditionOption.NORMAL))
            skillConditionOption = new NormalConditionOption();
        else
            Debug.Log("에러");

        // 2. 스크립트의 정보값을 대입합니다.
        skillConditionOption.SettingDefaultNewSkill(DS);
    }



    // 조건을 체크합니다.
    // 조건은 열거형에 따라 달라집니다.
    public bool CheckCondition()
    {
        return skillConditionOption.CheckCondition();
    }

    //  조건 체크 시 사용되는 함수입니다.
    bool EqualSkillConditionType(EnumSkillConditionOption SCO)
    {
        if (SkillConditionType == SCO)
            return true;
        else
            return false;
    }
}
