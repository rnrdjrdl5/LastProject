using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class DefaultPlayer
{

    public enum ConditionEnum { IDLE, RUN, WINDBLAST, BLINK, DAMAGE
    , ATTACK};

    public PhotonView pv; // PhotonView로 통신하기 위해 변수로 선언 해 둔다.
    


    //protected Vector3 RecvPosition = Vector3.zero; // 통신 한 Position 값을 받아서 저장하기 위한 변수
    //private Quaternion RecvRotation = Quaternion.identity; // 통신 한 Position 값을 받아서 저장하기 위한 변수
    //float RecvHealth = 0.0f;        //통신 한 Health 값을 받아서 저장하기 위한 변수




    protected Animator PlayerAnimator;        // 애니메이터를 저장해서 사용하기 위한 변수


  
    /*private float MaxHealth = 100.0f;   // 최대체력
    public float NowHealth = 100.0f;   // 현재체력
    */
    /*
    public float PlayerSpeed = 10.0f;       // 캐릭터 이동속도
    public float RotationSpeed = 100.0f;        // 캐릭터 회전속도
    */
    protected GameObject PlayerCamera;      // 외부에서 카메라를 받아와서 이 스크립트로 다룬다.
    /*
    public Image HPBar;                     // HP 이미지를 받아서 Amont를 조정하기 위한 변수   
    */

    

    public ConditionEnum PlayerCondition = ConditionEnum.IDLE;      // 현재 condition을 나타내기 위한 열거형 변수


    private bool isKnockBackMoving = false;      // 넉백 중 이동 여부를 결정하는 변수
    private bool isKnockBackMovingOther = false;       //본인이 아닐 때, 넉백 중 이동 여부를 결정하는 변수

    protected Vector3 KnockbackDistance = Vector3.zero; // 넉백의 방향을 정하기 위한 변수

    public float KnockbackSpeed = 10.0f; // 넉백 밀려나가는 속도
    public float KnockbackTime = 1.0f;      // 넉백 시 밀려나가는 시간


}
