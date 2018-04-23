using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {


    // 플레이어 지면과 닿을 때 지면까지의 최소 거리

    // 1. 카메라 레이캐스트를 쓰기 위해서
    private PointToLocation PTL;

    private PlayerMove playerMove;
    public PlayerMove GetPlayerMove() { return playerMove; }
    public void SetPlayerMove(PlayerMove pm) { playerMove = pm; }


    // 카메라 뷰의 설정.
    public enum EnumCameraMode { FOLLOW, FREE, SPEEDRUN };


    // 카메라 뷰의 설정
    private PlayerCamera.EnumCameraMode CameraModeType;

    public PlayerCamera.EnumCameraMode GetCameraModeType() { return CameraModeType; }
    public void SetCameraModeType(PlayerCamera.EnumCameraMode ECM)
    {
        CameraModeType = ECM;
    }

    // 카메라 보간 사용 여부 결정

    // 액션 이  끝난 후에 보간여부

    // 액션이 들어갈 때 플레이어 강제이동으로 인한 카메라변경. Lerp 이용.
    public bool ISPreUseLerp { set; get; }



    //카메라 뷰를 2종류로 나눕니다.

    //    1. 플레이어 뒤를 무조건 쫒아갑니다.

    //    2. 플레이어 뒤를 쫒아간다기 보다는, 월드적으로 카메라를 봅니다.
    //       관전자 모드처럼, 위치는 해당 플레이어를 쫒아가지만
    //       회전은 카메라로 결정할 수 있습니다.




    // 플레이어 관전 가능 여부
    public bool isObserver = false;

    // 플레이어 오브젝트
    public GameObject PlayerObject;

    // 카메라의 위아래 각도
    public float CameraRad = 20.0f;



    // 카메라의 최대, 최소 위아래 각도
    public float MinCameraRad = 20.0f;
    public float MaxCameraRad = 60.0f;




    // 카메라와 플레이어가 떨어져 있는 거리
    // 바꾸면 ptl에 있는 distance도 같은 값으로 유지시켜주기.
    public float CameraDistanceTriangle = 3.5f;

    // 최대 , 최소 떨어지는 거리
    public float MinCameraDistanceTriangle = 3.0f;
    public float MaxCameraDistanceTriangle = 10.0f;


    // 카메라가 땅에서 떨어져 있는 최소 거리
    public float CameraHeightFromFloor = 1.0f;


    // 카메라 위아래 회전 속도
    public float CameraUpDownSpeed = 15.0f;


    /********* 추가 x회전 ***********/

    // 좌우 회전 속도
    // 일반 follow에서는 플레이어의 회전에 영향
    public float CameraRightLeftSpeed = 50.0f;

    // 카메라의 좌우 각도
    public float CameraRadX = 0.0f;

    public float GetCameraRadX() { return CameraRadX; }
    public void SetCameraRadX(float SCR) { CameraRadX = SCR; }

    // 보간용 rad ,. 액션 들어갈 때 사용 ( 애니메이션 ) 
    public float PreCameraRadX { set; get; }



    // 자연스럽게 흘러가는 시간.
    [Header(" - 보간 수치 - ")]
    [Tooltip(" - 카메라가 자연스럽게 흘러가는 수치 ")]
    public float NaturePlayeCameraRotation;

    [Header(" - 최소 보간 수치 " )]
    [Tooltip(" - 보간 사용 여부를 결정짓는 수치")]
    public float PlayerRotationMinLerp;


    // 관전용 변수들
    private PhotonManager photonManager;
    private int SeePlayerNumber;
    private GameObject SeePlayer;

    // Use this for initialization

    private void Awake()
    {
        // 기본 설정.
        CameraModeType = EnumCameraMode.FOLLOW;
        PTL = new PointToLocation();
        PTL.SetPlayerCamera(gameObject);
        PTL.SetcameraScript(this);

        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
        SeePlayerNumber = 0;

        ISPreUseLerp = false;
        PreCameraRadX = 0.0f;
    }

    void Start () {

        

    }


    // 입력을 받아 카메라의 각도를 설정하는 함수.
    void SetCameraRad()
    {
        // 마우스 y축입력을 받습니다.
        CameraRad += -(Input.GetAxis("Mouse Y")) * Time.deltaTime * CameraUpDownSpeed;

        // 마우스 y축 입력에 제한을 둡니다.
        if (CameraRad > MaxCameraRad) CameraRad = MaxCameraRad;
        else if (CameraRad < MinCameraRad) CameraRad = MinCameraRad;


        // 상호작용 도중에는 마우스의 x축 회전도 받습니다.
        if (CameraModeType == EnumCameraMode.FREE)
        {
            CameraRadX += -(Input.GetAxis("Mouse X")) * Time.deltaTime * CameraRightLeftSpeed;
        }

    }
	
	// Update is called once per frame
	void Update () {

        // 키 입력에 따라 각도를 바꾼다.
        SetCameraRad();

        // 옵저버 사용
        if (isObserver)
        {

            if (Input.GetMouseButtonDown(0))
            {
                FindSeePlayer();
            }

            
        }


    }

    private void LateUpdate()
    {
        // 1. 초기 카메라 위치를 잡습니다.
        // 플레이어 생성 시
        if (PlayerObject)
        {



            // 카메라 상태에 따라 카메라를 이동시킨다.
            if (CameraModeType == PlayerCamera.EnumCameraMode.FOLLOW)
            {
                FollowCamera();
            }


            else if (CameraModeType == PlayerCamera.EnumCameraMode.FREE)
            {
                FreeCamera();
            }

            

        }

       



    }

    public void FindSeePlayer()
    {
        // 추가
        SeePlayerNumber += 1;

        // 오버됬는지 판단
        if (OverSeePlayer())
        {
            SeePlayerNumber = 0;
        }

        PlayerObject = photonManager.AllPlayers[SeePlayerNumber];
    }

    public bool OverSeePlayer()
    {
        if (SeePlayerNumber >= photonManager.AllPlayers.Count)
            return true;
        else
            return false;
    }


    // 플레이어의 뒤를 무조건 따라갑니다.
    void FollowCamera()
    {

        // 각도로 위치 얻어내기
        float CameraPlayerDistanceX = Mathf.Cos(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;
        float CameraPlayerDistanceY = Mathf.Sin(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;

        // GetPlayerRotateEuler : 플레이어 y축 변수
        // 카메라 플레이어 위치로 이동

        // 보간 값 설정
        float LerpAngle = Mathf.LerpAngle(transform.eulerAngles.y, playerMove.GetPlayerRotateEuler(), 1);




        // 쿼터니언 변경
        Quaternion QuatTypeLerpAngle = Quaternion.Euler(0, LerpAngle, 0);

        // 카메라 위치 설정
        transform.position = PlayerObject.transform.position - (QuatTypeLerpAngle * Vector3.forward * CameraPlayerDistanceX) + (Vector3.up * CameraPlayerDistanceY);

        // 플레이어방향으로 전환
        transform.LookAt(PlayerObject.transform);

        // y축 오프셋 추가
        transform.position = new Vector3(transform.position.x, transform.position.y + CameraHeightFromFloor, transform.position.z);

        // 오브젝트에 카메라 시야가 가려지면 카메라 위치 재조정
        // transform.position = PTL.FindWall(PlayerObject);




    }

    // 플레이어를 따라가나, 시점은 마음대로 변경이 가능합니다.
    void FreeCamera()
    {

        // 각도로 위치 얻어내기
        float CameraPlayerDistanceX = Mathf.Cos(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;
        float CameraPlayerDistanceY = Mathf.Sin(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;


        float LerpRad = SetPreLerpAngle();
        PreCameraRadX = LerpRad;
        CheckPreLerp();


        float CameraPlayerDistanceX_X = Mathf.Cos(Mathf.Deg2Rad * LerpRad) * CameraPlayerDistanceX;
        float CameraPlayerDistanceZ =   Mathf.Sin(Mathf.Deg2Rad * LerpRad) * CameraPlayerDistanceX;

        // 카메라 위치 설정
        transform.position = PlayerObject.transform.position - (Vector3.forward * CameraPlayerDistanceX_X) + Vector3.right * CameraPlayerDistanceZ + Vector3.up * CameraPlayerDistanceY;

        // 플레이어방향으로 전환
        transform.LookAt(PlayerObject.transform);

        // y축 오프셋 추가
        transform.position = new Vector3(transform.position.x, transform.position.y + CameraHeightFromFloor, transform.position.z);

        
    }



    // follow > free 갈 때 보간  
    private float SetPreLerpAngle()
    {
        float LerpAngle = 0.0f;
        if (!ISPreUseLerp)
        {
            LerpAngle = Mathf.LerpAngle(PreCameraRadX, CameraRadX, 1);

        }
        else
        {
            LerpAngle = Mathf.LerpAngle(PreCameraRadX, CameraRadX, NaturePlayeCameraRotation);
        }

        return LerpAngle;
    }



    // free 보간 해제 조건
    private void CheckPreLerp()
    {
        // 1. 계산
        float LerpRad = PreCameraRadX - CameraRadX;

        // 2. 각도 제한
        if (LerpRad > 180)
            LerpRad -= 360;

        // 3. 절대값으로
        LerpRad = Mathf.Abs(LerpRad);

        if (LerpRad < PlayerRotationMinLerp)
        {
            ISPreUseLerp = false;
        }
    }

}
