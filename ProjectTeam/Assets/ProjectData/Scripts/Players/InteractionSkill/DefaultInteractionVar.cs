using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class DefaultInteraction
{
    public enum EnumSkillKey { LEFTMOUSE , F }

    public enum EnumInteractiveKey { TABLE = 1 } ;

    



    [Header(" - 상호작용 Key")]
    [Tooltip("- 해당 키를 눌렀을 때 상호작용이 일어납니다. ")]
    public EnumSkillKey SkillKeyType;

    [Header(" - 상호작용 애니메이션")]
    [Tooltip("- 상호작용 애니메이션을 정하는 함수입니다. ")]
    public EnumInteractiveKey InteractiveKeyType;


    

    // 아래부터는 사용하기 위해서 기본 설정하는 것들입니다. 
    // 예를들어 animator, photonview 등.

    protected PhotonView pv;
    protected Animator animator;
    protected FindObject fo;


    // 타임바 스크립트.
    private TimeBar TimeBarScript;

    public TimeBar GetTimeBarScript() { return TimeBarScript; }
    public void SetTimeBarScript(TimeBar TB) { TimeBarScript = TB; }

    private GameObject InGameCanvas;


    // FindObject 스크립트

    private FindObject FindObjectScript;

    public FindObject GetFindObjectScript() { return FindObjectScript; }
    public void SetFindObjectScript(FindObject FO) { FindObjectScript = FO; }


    // 상호작용 오브젝트
    private GameObject InteractiveObject;
    
    public GameObject GetInteractiveObject() { return InteractiveObject; }
    public void SetInteractiveObject(GameObject IO) { InteractiveObject = IO; }


    protected GameObject PlayerCamera;

    public GameObject GetPlayerCamera() { return PlayerCamera; }
    public void SetPlayerCamera(GameObject GO) { PlayerCamera = GO; }



}

