using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BoxPlayerMove
{
    
    private Vector3 RecvPosition;
    private Quaternion RecvRotation;

    // 이동 좌표를 받아서 애니메이션 블랜드 적용 용도.
    // 다른방법을 알아볼까? 가능할거같은데.
    private float RecvDirectionX;
    private float RecvDirectionY;

    public void SetRecvPosition(Vector3 RP) { RecvPosition = RP; }

    private float PlayerVertical = 0.0f;
    private float PlayerHorizontal = 0.0f;


    public float PlayerSpeed = 10.0f;       // 캐릭터 이동속도
    public float RotationSpeed = 100.0f;        // 캐릭터 회전속도

    public float PlayerBackSpeed = 5.0f;

    // 저장용 스피드.
    private float OriginalPlayerSpeed = 0.0f;
    
    public float GetOriginalPlayerSpeed() { return OriginalPlayerSpeed; }
    public void SetOriginalPlayerSpeed(float SPS) { OriginalPlayerSpeed = SPS; }


    float gravity = 20.0f;

    // idlerun 애니메이션을 위한변수입니다. 
    float HSpeed;
    float VSpeed;


}
