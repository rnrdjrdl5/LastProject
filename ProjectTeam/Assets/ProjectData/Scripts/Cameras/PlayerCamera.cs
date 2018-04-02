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
    public enum EnumCameraMode { FOLLOW , FREE ,SPEEDRUN};


    // 카메라 뷰의 설정
    private PlayerCamera.EnumCameraMode CameraModeType;

    public PlayerCamera.EnumCameraMode GetCameraModeType() { return CameraModeType; }
    public void SetCameraModeType(PlayerCamera.EnumCameraMode ECM) { CameraModeType = ECM; }



    //카메라 뷰를 2종류로 나눕니다.

    //    1. 플레이어 뒤를 무조건 쫒아갑니다.

    //    2. 플레이어 뒤를 쫒아간다기 보다는, 월드적으로 카메라를 봅니다.
    //       관전자 모드처럼, 위치는 해당 플레이어를 쫒아가지만
    //       회전은 카메라로 결정할 수 있습니다.



    // 플레이어 생성 여부
    public bool isPlayerSpawn = true;

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
    private float CameraRadX = 0.0f;

    public float GetCameraRadX() { return CameraRadX; }
    public void SetCameraRadX(float SCR) { CameraRadX = SCR; }

    // 자연스럽게 흘러가는 시간.
    public float NaturePlayeCameraRotation;
    // Use this for initialization

    private void Awake()
    {
        // 기본 설정.
        CameraModeType = EnumCameraMode.FOLLOW;
        PTL = new PointToLocation();
        PTL.SetPlayerCamera(gameObject);
        PTL.SetcameraScript(this);
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

    }

    private void LateUpdate()
    {
        // 1. 초기 카메라 위치를 잡습니다.
        // 플레이어 생성 시
        if (isPlayerSpawn)
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

            else if (CameraModeType == PlayerCamera.EnumCameraMode.SPEEDRUN)
            {
                SpeedRunCamera();
            }
        }

        



    }


    // 플레이어의 뒤를 무조건 따라갑니다.
    void FollowCamera()
    {
        if (isPlayerSpawn)
        {

          /*  float CameraPlayerDistanceX = Mathf.Cos(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;
            float CameraPlayerDistanceY = Mathf.Sin(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;

            // 보간 사용, 카메라 x,z 위치를 보간으로 서서히 조절
            //float LerpAngle = Mathf.LerpAngle(transform.eulerAngles.y, PlayerObject.transform.eulerAngles.y, NaturePlayeCameraRotation * Time.deltaTime);
            float LerpAngle = Mathf.LerpAngle(transform.eulerAngles.y, PlayerObject.transform.eulerAngles.y, 1);
            //Quaternion 형태로 전환
            Quaternion QuatTypeLerpAngle = Quaternion.Euler(0, LerpAngle, 0);

            // 보간으로 구한 값과
            // 카메라 각도로 구한 값으로
            // 카메라의 위치를 결정함.
            transform.position = PlayerObject.transform.position - (QuatTypeLerpAngle * Vector3.forward * CameraPlayerDistanceX) + (Vector3.up * CameraPlayerDistanceY);
            

            // 카메라가 Player를 보도록 함
            transform.LookAt(PlayerObject.transform);

            // 카메라 위치에서 y값을 추가로 더함
                transform.position = new Vector3(transform.position.x, transform.position.y + CameraHeightFromFloor, transform.position.z);

            // 오브젝트에 카메라 시야가 가려지면 카메라 위치 재조정
            transform.position = PTL.FindWall(PlayerObject);
            */


        }
    }

    // 플레이어를 따라가나, 시점은 마음대로 변경이 가능합니다.
    void FreeCamera()
    {

        // 카메라의 x와 y 위치를 구함.
        float CameraPlayerDistanceX = Mathf.Cos(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;
        float CameraPlayerDistanceY = Mathf.Sin(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;

        /* X축을 추가 연산합니다.
         * X축 길이를 사용해서 Z축과 X축의 길이를 구해옵니다. */
        float CameraPlayerDistanceX_X = Mathf.Cos(Mathf.Deg2Rad * CameraRadX) * CameraPlayerDistanceX;
        float CameraPlayerDistanceZ =   Mathf.Sin(Mathf.Deg2Rad * CameraRadX) * CameraPlayerDistanceX;

        // 카메라의 위치를 구한 x,y,z 축을 이용해서 위치를 선정합니다.
        transform.position = PlayerObject.transform.position - (Vector3.forward * CameraPlayerDistanceX_X) + Vector3.right * CameraPlayerDistanceZ + Vector3.up * CameraPlayerDistanceY;

        // 카메라가 Player를 보도록 함.
        transform.LookAt(PlayerObject.transform);

        // 카메라가 기본적으로 가지고 있어야 할 y축 높이를 더해줌.
        transform.position = new Vector3(transform.position.x, transform.position.y + CameraHeightFromFloor, transform.position.z);


    }

    void SpeedRunCamera()
    {
        float CameraPlayerDistanceX = Mathf.Cos(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;
        float CameraPlayerDistanceY = Mathf.Sin(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;

        // 보간 사용, 카메라 x,z 위치를 보간으로 서서히 조절
        float LerpAngle = Mathf.LerpAngle(transform.eulerAngles.y, PlayerObject.transform.eulerAngles.y, 1);

        //Quaternion 형태로 전환
        Quaternion QuatTypeLerpAngle = Quaternion.Euler(0, LerpAngle, 0);

        // 보간으로 구한 값과
        // 카메라 각도로 구한 값으로
        // 카메라의 위치를 결정함.
        transform.position = PlayerObject.transform.position - (QuatTypeLerpAngle * Vector3.forward * CameraPlayerDistanceX) + (Vector3.up * CameraPlayerDistanceY);


        // 카메라가 Player를 보도록 함
        transform.LookAt(PlayerObject.transform);

        // 카메라 위치에서 y값을 추가로 더함
        transform.position = new Vector3(transform.position.x, transform.position.y + CameraHeightFromFloor, transform.position.z);

        // 오브젝트에 카메라 시야가 가려지면 카메라 위치 재조정
        transform.position = PTL.FindWall(PlayerObject);

    }
}
