using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerMove
{ 
    private Vector3 RecvPosition;
    private Quaternion RecvRotation;

    public void SetRecvPosition(Vector3 RP) { RecvPosition = RP; }

    private float PlayerVertical = 0.0f;
    private float PlayerHorizontal = 0.0f;


    public float PlayerSpeed = 10.0f;       // 캐릭터 이동속도
    public float RotationSpeed = 100.0f;        // 캐릭터 회전속도

   
}
