using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefaultInput{

    // 키의 상태를 파악하기 위한 변수입니다.
    public enum EnumSkillKey
    {
        LEFTMOUSE, RIGHTMOUSE, LEFTSHIFT, RIGHTSHIFT, SPACE, Q, E,
        LEFTSHIFTUP, LEFTSHIFTDOWN, PUSINGLEFTSHIFT , NOTPUSHINGLEFTSHIFT
    }

    public EnumSkillKey SkillKeyType;


    // 키 코드가 현재 변수값이랑 같은지 판단.
    protected bool EqualSkillKeyType()
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
            case EnumSkillKey.NOTPUSHINGLEFTSHIFT:
                ReturnType = !Input.GetKey(KeyCode.LeftShift);
                break;

        }
        return ReturnType;
    }

    //키를 눌렀는지 판단합니다.
    virtual public bool IsUseKey()
    {
        return EqualSkillKeyType();
    }
}
