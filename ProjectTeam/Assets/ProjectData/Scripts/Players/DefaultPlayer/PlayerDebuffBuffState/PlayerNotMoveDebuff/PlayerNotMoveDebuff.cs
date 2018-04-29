using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNotMoveDebuff : PlayerDefaultDebuff
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        // 속박 하기.
    }

    protected override void ExitDebuff()
    {
        gameObject.GetComponent<Animator>().SetBool("isNotMove", false);
    }

}
