using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    // 카메라 뷰의 설정.
    
    public enum EnumCameraMode { FOLLOW , FREE };


    // 플레이어 무브랑 따로 관리합니다.
    private PlayerCamera.EnumCameraMode CameraModeType;

    public PlayerCamera.EnumCameraMode GetCameraModeType() { return CameraModeType; }
    public void SetCameraModeType(PlayerCamera.EnumCameraMode ECM) { CameraModeType = ECM; }

    //카메라 뷰를 2종류로 나눕니다.

    //    1. 플레이어 뒤를 무조건 쫒아갑니다.

    //    2. 플레이어 뒤를 쫒아간다기 보다는, 월드적으로 카메라를 봅니다.
    //       관전자 모드처럼, 위치는 해당 플레이어를 쫒아가지만
    //       회전은 카메라로 결정할 수 있습니다.


    public bool isPlayerSpawn = true;

    public GameObject PlayerObject;

    public float CameraDistanceToPlayer = 10.0f;
    public float CameraHeight = 5.0f;


    public float CameraRad = 20.0f;
    public float MinCameraRad = 20.0f;
    public float MaxCameraRad = 60.0f;


    


    private float CameraDistanceTriangle = 2.4f;

    public float MinCameraDistanceTriangle = 3.0f;
    public float MaxCameraDistanceTriangle = 10.0f;

    public float CameraWheelSpeed = 30000.0f;

    public float CameraHeightFromFloor = 1.0f;



    public float CameraUpDownSpeed = 50.0f;


    // 추가 x회전
    public float CameraRightLeftSpeed = 50.0f;

    //-PlayerObject.transform.eulerAngles.y
    private float CameraRadX = 0.0f;

    public float GetCameraRadX() { return CameraRadX; }
    public void SetCameraRadX(float SCR) { CameraRadX = SCR; }

    public float CameraMaxRadX = 90.0f;
    public float CameraMinRadX = -90.0f;

    public float CameraDistanceX = 5.0f;

    // Use this for initialization
    void Start () {
        CameraModeType = EnumCameraMode.FOLLOW; 

    }

    void SetCameraRad()
    {
        CameraRad += -(Input.GetAxis("Mouse Y")) * Time.deltaTime * CameraUpDownSpeed;

        if (CameraRad > MaxCameraRad) CameraRad = MaxCameraRad;
        else if (CameraRad < MinCameraRad) CameraRad = MinCameraRad;



        if (CameraModeType == EnumCameraMode.FREE)
        {
            CameraRadX += -(Input.GetAxis("Mouse X")) * Time.deltaTime * CameraRightLeftSpeed;

            

            /*if (CameraRadX > CameraMaxRadX) CameraRadX = CameraMaxRadX;
            else if (CameraRadX < CameraMinRadX) CameraRadX = CameraMinRadX;*/

        }

    }
	
	// Update is called once per frame
	void Update () {


        SetCameraRad();

    }

    private void LateUpdate()
    {

        if (isPlayerSpawn)
        {

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
            CameraDistanceTriangle = CameraDistanceTriangle - Time.deltaTime * Input.GetAxis("Mouse ScrollWheel") * CameraWheelSpeed;


            if (CameraDistanceTriangle < MinCameraDistanceTriangle) CameraDistanceTriangle = MinCameraDistanceTriangle;
            else if (CameraDistanceTriangle > MaxCameraDistanceTriangle) CameraDistanceTriangle = MaxCameraDistanceTriangle;


            float CameraPlayerDistanceX = Mathf.Cos(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;
            float CameraPlayerDistanceY = Mathf.Sin(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;

            float LerpAngle = Mathf.LerpAngle(transform.eulerAngles.y, PlayerObject.transform.eulerAngles.y, 1.0f);

            Quaternion QuatTypeLerpAngle = Quaternion.Euler(0, LerpAngle, 0);

            transform.position = PlayerObject.transform.position - (QuatTypeLerpAngle * Vector3.forward * CameraPlayerDistanceX) + (Vector3.up * CameraPlayerDistanceY);

            transform.LookAt(PlayerObject.transform);

            transform.position = new Vector3(transform.position.x, transform.position.y + CameraHeightFromFloor, transform.position.z);
        }
    }

    // 플레이어를 따라가나, 시점은 마음대로 변경이 가능합니다.
    void FreeCamera()
    {
        CameraDistanceTriangle = CameraDistanceTriangle - Time.deltaTime * Input.GetAxis("Mouse ScrollWheel") * CameraWheelSpeed;


        if (CameraDistanceTriangle < MinCameraDistanceTriangle) CameraDistanceTriangle = MinCameraDistanceTriangle;
        else if (CameraDistanceTriangle > MaxCameraDistanceTriangle) CameraDistanceTriangle = MaxCameraDistanceTriangle;

        float CameraPlayerDistanceX = Mathf.Cos(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;
        float CameraPlayerDistanceY = Mathf.Sin(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;

        //CaemraRadX
        float CameraPlayerDistanceX_X = Mathf.Cos(Mathf.Deg2Rad * CameraRadX) * CameraPlayerDistanceX;
        float CameraPlayerDistanceZ =   Mathf.Sin(Mathf.Deg2Rad * CameraRadX) * CameraPlayerDistanceX;

        transform.position = PlayerObject.transform.position - (Vector3.forward * CameraPlayerDistanceX_X) + Vector3.right * CameraPlayerDistanceZ + Vector3.up * CameraPlayerDistanceY;

        transform.LookAt(PlayerObject.transform);

        transform.position = new Vector3(transform.position.x, transform.position.y + CameraHeightFromFloor, transform.position.z);


    }

}
