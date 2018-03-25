using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPartCollisionCondition : MonoBehaviour {

    // 상호작용 액션을 할 수 있는지의 여부입니다.
    private bool isCanInterAction;
    
    public bool GetisCanInterAction() { return isCanInterAction; }
    public void SetisCanInterAction(bool CIA) { isCanInterAction = CIA; }


    // 이미 상호작용을 했는가?
    static public bool isUseInterAction;


    // 등록한 대상이 누군지 판단합니다.
    private DefaultInteraction defaultInteraction;

    public DefaultInteraction GetDefaultInteraction() { return defaultInteraction; }
    public void SetdefaultInteraction(DefaultInteraction DI) { defaultInteraction = DI; }



    // 상호작용 오브젝트 타입입니다.
    protected DefaultInteraction.EnumInteractiveKey InterObjectType;

    public DefaultInteraction.EnumInteractiveKey GetInterObjectType() { return InterObjectType; }
    public void SetInterObjectType(DefaultInteraction.EnumInteractiveKey EITK) { InterObjectType = EITK; }


    // 초기화
    virtual protected void Awake()
    {
        isCanInterAction = false;
        isUseInterAction = false;
    }
}
