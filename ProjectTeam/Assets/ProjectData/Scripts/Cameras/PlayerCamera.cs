using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    // 카메라 뷰의 설정.
    public enum EnumCameraMode { FOLLOW , FREE };


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
    private float CameraDistanceTriangle = 2.4f;

    // 최대 , 최소 떨어지는 거리
    public float MinCameraDistanceTriangle = 3.0f;
    public float MaxCameraDistanceTriangle = 10.0f;

    // 카메라 휠의 이동속도
    public float CameraWheelSpeed = 30000.0f;

    // 카메라가 땅에서 떨어져 있는 최소 거리
    public float CameraHeightFromFloor = 1.0f;


    // 카메라 위아래 회전 속도
    public float CameraUpDownSpeed = 50.0f;


    /********* 추가 x회전 ***********/

    // 좌우 회전 속도
    public float CameraRightLeftSpeed = 50.0f;

    // 카메라의 좌우 각도
    private float CameraRadX = 0.0f;

    public float GetCameraRadX() { return CameraRadX; }
    public void SetCameraRadX(float SCR) { CameraRadX = SCR; }

    // Use this for initialization

    private void Awake()
    {
        // 기본 설정.
        CameraModeType = EnumCameraMode.FOLLOW;
    }

    void Start () {

        

    }


    // 입력을 받아 카메라의 각도를 설정하는 함수.
    void SetCameraRad()
    {
        CameraRad += -(Input.GetAxis("Mouse Y")) * Time.deltaTime * CameraUpDownSpeed;

        if (CameraRad > MaxCameraRad) CameraRad = MaxCameraRad;
        else if (CameraRad < MinCameraRad) CameraRad = MinCameraRad;



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
        }

    }


    // 플레이어의 뒤를 무조건 따라갑니다.
    void FollowCamera()
    {
        if (isPlayerSpawn)
        {
            // 휠에 따라서 카메라의 거리를 조절함.
            CameraDistanceTriangle = CameraDistanceTriangle - Time.deltaTime * Input.GetAxis("Mouse ScrollWheel") * CameraWheelSpeed;

            // 최대 거리 , 최소거리 유지
            if (CameraDistanceTriangle < MinCameraDistanceTriangle) CameraDistanceTriangle = MinCameraDistanceTriangle;
            else if (CameraDistanceTriangle > MaxCameraDistanceTriangle) CameraDistanceTriangle = MaxCameraDistanceTriangle;

            // 카메라의 x와 y 위치를 구함.
            float CameraPlayerDistanceX = Mathf.Cos(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;
            float CameraPlayerDistanceY = Mathf.Sin(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;

            // 플레이어의 y축 회전을 받아옴.
             // 보간값을 맞춰서 조절할 예정
            float LerpAngle = Mathf.LerpAngle(transform.eulerAngles.y, PlayerObject.transform.eulerAngles.y, 1.0f);

            
            Quaternion QuatTypeLerpAngle = Quaternion.Euler(0, LerpAngle, 0);

            // 카메라의 위치를 구한 x축 , y축과 y축회전을 이용해서 위치를 선정함.
            transform.position = PlayerObject.transform.position - (QuatTypeLerpAngle * Vector3.forward * CameraPlayerDistanceX) + (Vector3.up * CameraPlayerDistanceY);
            
            // 카메라가 Player를 보도록 함
            transform.LookAt(PlayerObject.transform);

            // 카메라가 기본적으로 가지고 있어야 할 y축 높이를 더해줌
            transform.position = new Vector3(transform.position.x, transform.position.y + CameraHeightFromFloor, transform.position.z);
        }
    }

    // 플레이어를 따라가나, 시점은 마음대로 변경이 가능합니다.
    void FreeCamera()
    {
        // 휠에 따라서 카메라의 거리를 조절함.
        CameraDistanceTriangle = CameraDistanceTriangle - Time.deltaTime * Input.GetAxis("Mouse ScrollWheel") * CameraWheelSpeed;

        // 최대 거리 , 최소거리 유지
        if (CameraDistanceTriangle < MinCameraDistanceTriangle) CameraDistanceTriangle = MinCameraDistanceTriangle;
        else if (CameraDistanceTriangle > MaxCameraDistanceTriangle) CameraDistanceTriangle = MaxCameraDistanceTriangle;

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

}
