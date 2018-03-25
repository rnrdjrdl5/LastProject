using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// 실제로 오브젝트가 밀려나갈 수 있도록 적용시키는 부분입니다.
public class DefaultPushObject
{
    protected float XZPower = 0.0f;

    public float GetXZPower() { return XZPower; }
    public void SetXZPower(float XZP) { XZPower = XZP; }



    protected float YPower = 0.0f;

    public float GetYPower() { return YPower; }
    public void SetYPower(float SYP) { YPower = SYP; }



    protected float TorquePower = 0.0f;

    public float GetTorquePower() { return TorquePower; }
    public void SetTorquePower(float TP) { TorquePower = TP; }


    // 상호작용 대상 오브젝트입니다.
    private GameObject InteractionObject;

    public GameObject GetInteractionObject() { return InteractionObject; }
    public void SetInteractionObject(GameObject GO) { InteractionObject = GO; }


    // 플레이어 오브젝트입니다.
    private GameObject PlayerObject;

    public GameObject GetPlayerObject() { return PlayerObject; }
    public void SetPlayerObject(GameObject playerObject) { PlayerObject = playerObject; }


    // 카메라입니다.
    private GameObject PlayerCamera;

    public GameObject GetPlayerCamera() { return PlayerCamera; }
    public void SetPlayerCamera(GameObject playerCamera) { PlayerCamera = playerCamera; }



    // 플레이어 정보를 새로 적용합니다.
    public void InitData(GameObject playerObject, GameObject playerCamera)
    {
        PlayerObject = playerObject;
        PlayerCamera = playerCamera;
    }

    public void InitInterData(GameObject interObject)
    {
        InteractionObject = interObject;
    }

    virtual public void Action()
    {
        Debug.Log("부모");
    }


}
