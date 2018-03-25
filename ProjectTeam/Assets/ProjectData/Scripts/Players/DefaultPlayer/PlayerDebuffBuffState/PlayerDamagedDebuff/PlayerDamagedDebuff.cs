using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedDebuff : PlayerDefaultDebuff
{

    // 다른 곳에서 등록합니다.

    protected override void Update()
    {
        base.Update();
    }

    // 시간이 지나면 DamageOnOff의 값을 10으로 변경시킵니다.
    // 10이 다음으로 넘어가게 해줍니다.
    protected override void ExitDebuff()
    {
        gameObject.GetComponent<Animator>().SetInteger("DamageOnOff", 10);
    }
}
