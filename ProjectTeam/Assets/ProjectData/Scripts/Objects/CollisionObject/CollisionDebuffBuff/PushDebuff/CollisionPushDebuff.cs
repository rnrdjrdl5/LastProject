using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPushDebuff : CollisionDefaultDebuff
{
    private Vector3 MoveDirection;

    public void SetMoveDirection(Vector3 MD) { MoveDirection = MD; }
    public Vector3 GetMoveDirection() { return MoveDirection; }

    private float MoveSpeed;

    public void SetMoveSpeed(float MS) { MoveSpeed = MS; }
    public float GetMoveSpeed() { return MoveSpeed; }
}
