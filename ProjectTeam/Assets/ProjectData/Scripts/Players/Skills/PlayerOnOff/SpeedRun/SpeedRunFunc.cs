using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SpeedRun
{
    

    public override bool CheckState()
    {
        if ((EqualState(PlayerState.ConditionEnum.RUN)))
        {
            return true;
        }
        else
            return false;
    }

    public override void UseSkill()
    {
        gameObject.GetComponent<BoxPlayerMove>().PlayerSpeed = SpeedRunSpeed;
        gameObject.GetComponent<Animator>().SetFloat("SpeedRun",
            SpeedRunSpeed / gameObject.GetComponent<BoxPlayerMove>().GetOriginalPlayerSpeed());

        SpeedRunEffectObject = Instantiate(SpeedRunEffectPrefab, transform.position, transform.rotation);
        SpeedRunEffectObject.transform.Rotate(0, 180, 0);

        photonView.RPC("RPCCreateEffect", PhotonTargets.Others);



    }

    protected override void UseOffSkill()
    {
        gameObject.GetComponent<BoxPlayerMove>().PlayerSpeed =
    gameObject.GetComponent<BoxPlayerMove>().GetOriginalPlayerSpeed();

        gameObject.GetComponent<Animator>().SetFloat("SpeedRun", 1.0f);

        Destroy(SpeedRunEffectObject);

        photonView.RPC("RPCDestroyEffect", PhotonTargets.Others);
    }

    protected override void ChannelingSkill()
    {
        if (SpeedRunEffectObject != null)
        {
            SpeedRunEffectObject.transform.position = transform.position;
            SpeedRunEffectObject.transform.rotation = transform.rotation;
            SpeedRunEffectObject.transform.Rotate(0, 180, 0);
        }
    }

    // 아래부터는 RPC입니다.
    [PunRPC]
    void RPCCreateEffect()
    {
        gameObject.GetComponent<Animator>().SetFloat("SpeedRun",
            SpeedRunSpeed / gameObject.GetComponent<BoxPlayerMove>().GetOriginalPlayerSpeed());

        SpeedRunEffectObject = Instantiate(SpeedRunEffectPrefab, transform.position, transform.rotation);
        SpeedRunEffectObject.transform.Rotate(0, 180, 0);

        isSkillOnOff = true;
    }

    [PunRPC]
    void RPCDestroyEffect()
    {
        gameObject.GetComponent<Animator>().SetFloat("SpeedRun", 1.0f);
        Destroy(SpeedRunEffectObject);
    }
}
