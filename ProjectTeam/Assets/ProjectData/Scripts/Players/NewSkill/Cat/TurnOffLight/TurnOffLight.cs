using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffLight : DefaultNewSkill
{
    // 불 끌때 각종 옵션입니다.
    public TurnOffLightState turnOffLightState;

    // 캔버스입니다. 불 끄는 ui가 들어갈 위치.
    private GameObject InGameCanvas;

    protected override void Awake()
    {
        base.Awake();

        InGameCanvas = GameObject.Find("InGameCanvas").gameObject;

        defaultCdtAct = new NormalCdtAct();
        defaultCdtAct.InitCondition(this);
    }

    public override bool CheckState()
    {
        //이동중이거나 가만히 있을 때 가능합니다.
        if ((
            playerState.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
            playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN)))

        {
            return true;
        }
        else
            return false;
    }

    public override void UseSkill()
    {
        photonView.RPC("RPCTurnOffLight", PhotonTargets.Others);
        Debug.Log("스킬시작");
    }

    // RPC입니다.
    [PunRPC]
    void RPCTurnOffLight()
    {
        CreateTurnOffPanel();
    }

    public void CreateTurnOffPanel()
    {
        GameObject panel = Instantiate(turnOffLightState.TrunOffPanel);

        panel.transform.parent = InGameCanvas.transform;
        
        // 다음으로 설정해주기.
        TurnOffPanel top = panel.GetComponentInChildren<TurnOffPanel>();
        top.transform.localScale = Vector3.one;

        // 데이터 설정
        top.SetTurnOffTime(turnOffLightState.TurnOffTime);

        top.StartCutScene();


    }
}
