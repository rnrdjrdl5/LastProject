using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OneKeyInput : DefaultInput
{
    public override bool IsUseKey()
    {
        return EqualSkillKeyType();
    }
}
