using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunDebuff : PlayerDefaultDebuff
{
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
    }
}
