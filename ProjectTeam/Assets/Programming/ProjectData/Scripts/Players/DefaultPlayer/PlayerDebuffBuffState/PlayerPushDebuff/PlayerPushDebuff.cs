using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPushDebuff : PlayerDefaultDebuff {
    private float MovePushSpeed;

    public void SetMovePushSpeed(float MPS) { MovePushSpeed = MPS; }
    public float GetmovePushSpeed() { return MovePushSpeed; }

    private Vector3 MoveDirection;

    public void SetMoveDirection(Vector3 SMD) { MoveDirection = SMD; }
    public Vector3 GetMoveDirection() { return MoveDirection; }

    Rigidbody rd;
    PhotonView pv;
    void Awake()
    {
        rd = gameObject.GetComponent<Rigidbody>();
        pv = gameObject.GetComponent<PhotonView>();
    }
    override protected void Update()
    {
        
        base.Update();
        if (pv.isMine)
        {
            if (NowDebuffTime < GetMaxDebuffTime())
            {
                rd.velocity = MoveDirection * MovePushSpeed;
                Debug.Log(rd.velocity);
            }
            
        }
    }

    protected override void ExitDebuff()
    {
        base.ExitDebuff();
        rd.velocity = Vector3.zero;
        
    }


}
