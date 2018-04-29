using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunDebuff : PlayerDefaultDebuff
{

    PlayerState playerState;

    protected override void Awake()
    {
        playerState = GetComponent<PlayerState>();

    }


    protected override void Start()
    {
    }

   

    protected override void Update()
    {
        base.Update();
        Debug.Log("스턴중!!");
        Debug.Log("Time : " + NowDebuffTime);
    }

    protected override void ExitDebuff()
    {
        gameObject.GetComponent<Animator>().SetBool("StunOnOff", false);
        Debug.Log("***스턴끝 *****");

        if (DebuffEffect != null)
        {
            PoolingManager.GetInstance().PushObject(DebuffEffect);
        }
    }

    public void CreateEffect()
    {
        DebuffEffect = PoolingManager.GetInstance().PopObject("strun_main_01");

        DebuffEffect.transform.position =
            playerState.GetHeadObject().transform.position - transform.up * 0.5f;

        DebuffEffect.transform.SetParent(playerState.GetHeadObject().transform);

    }


}
