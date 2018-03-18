using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedDebuff : PlayerDefaultDebuff
{
    protected override void Update()
    {
        base.Update();
    }

    protected override void ExitDebuff()
    {
        gameObject.GetComponent<Animator>().SetInteger("DamageOnOff", 10);
    }
}
