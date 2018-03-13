using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public bool isPlayerSpawn = true;

    public GameObject PlayerObject;

    public float CameraSpeed = 5.0f;

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
    

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {


        CameraRad += -(Input.GetAxis("Mouse Y")) * Time.deltaTime * CameraUpDownSpeed;

        if (CameraRad > MaxCameraRad) CameraRad = MaxCameraRad;
        else if (CameraRad < MinCameraRad) CameraRad = MinCameraRad;

    }
    private float testDistance = 0.0f;
    private void LateUpdate()
    {
        if (isPlayerSpawn)
        {
           // if(Input.GetAxis("Mouse ScrollWheel") != 0) Debug.Log(Input.GetAxis("Mouse ScrollWheel"));

            
            CameraDistanceTriangle = CameraDistanceTriangle - Time.deltaTime * Input.GetAxis("Mouse ScrollWheel") * CameraWheelSpeed;

            if (testDistance < Input.GetAxis("Mouse ScrollWheel"))
                testDistance = Input.GetAxis("Mouse ScrollWheel");

            if (CameraDistanceTriangle < MinCameraDistanceTriangle) CameraDistanceTriangle = MinCameraDistanceTriangle;
            else if (CameraDistanceTriangle > MaxCameraDistanceTriangle) CameraDistanceTriangle = MaxCameraDistanceTriangle;


            /*
            float LerpAngle = Mathf.LerpAngle(transform.eulerAngles.y, PlayerObject.transform.eulerAngles.y, CameraSpeed * Time.deltaTime);

            Quaternion QuatTypeLerpAngle = Quaternion.Euler(0, LerpAngle, 0);

            transform.position = PlayerObject.transform.position - (QuatTypeLerpAngle * Vector3.forward * CameraDistanceToPlayer) + (Vector3.up * CameraHeight);

            transform.LookAt(PlayerObject.transform);

    */

            float CameraPlayerDistanceX = Mathf.Cos(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;
            float CameraPlayerDistanceY = Mathf.Sin(Mathf.Deg2Rad * CameraRad) * CameraDistanceTriangle;

            float LerpAngle = Mathf.LerpAngle(transform.eulerAngles.y, PlayerObject.transform.eulerAngles.y,1.0f);

            Quaternion QuatTypeLerpAngle = Quaternion.Euler(0, LerpAngle, 0);

            transform.position = PlayerObject.transform.position - (QuatTypeLerpAngle * Vector3.forward * CameraPlayerDistanceX) + (Vector3.up * CameraPlayerDistanceY);

            transform.LookAt(PlayerObject.transform);

            transform.position = new Vector3(transform.position.x, transform.position.y + CameraHeightFromFloor, transform.position.z);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), testDistance.ToString());
    }
}
