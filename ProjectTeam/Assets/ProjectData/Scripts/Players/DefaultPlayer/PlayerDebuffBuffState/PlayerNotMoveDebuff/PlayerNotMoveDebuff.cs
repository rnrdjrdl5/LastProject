using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNotMoveDebuff : PlayerDefaultDebuff
{
    private PlayerMove playerMove;

    protected override void Start()
    {
        base.Start();

        playerMove = GetComponent<PlayerMove>();
        playerMove.ResetMoveSpeed();
    }

    protected override void Update()
    {
        base.Update();
        // 속박 하기.
    }

}
