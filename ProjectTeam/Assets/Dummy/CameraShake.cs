using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}

    public KeyCode k;

    public Vector3 CameraOriginalPosition;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(k) && !IsCameraShaking)
        {
            CameraOriginalPosition = transform.position;
            SetCameraShake(insShakeTime, insShakeTick, insShakePower);
        }

        if(IsCameraShaking)
            UseShake();
	}



    // 카메라 쉐이크
    public float insShakeTick = 0.016f;
    public float insShakeTime = 1.0f;
    public float insShakePower = 0.5f;


    private float CameraShakePower = 0.1f;

    public bool IsCameraShaking { get; set; }

    public float MaxCameraShakingTime { get; set; }

    public float NowCameraShakingTime { get; set; }

    public float NowCameraShakeTick { get; set; }
    public float MaxCameraShakeTick { get; set; }


    // 카메라 쉐이크
    public void SetCameraShake(float _shakeTime, float _shakeTickTime = 0.016f, float _shakePower = 0.5f)
    {

        CameraShakePower = _shakePower;

        MaxCameraShakingTime = _shakeTime;
        NowCameraShakingTime = 0.0f;

        MaxCameraShakeTick = _shakeTickTime;
        NowCameraShakeTick = 0.0f;

        IsCameraShaking = true;
    }

    void UseShake()
    {

        // 틱 돌리고 틱당 쉐이크 주기
        if (NowCameraShakeTick >= MaxCameraShakeTick)
        {
            NowCameraShakeTick = 0.0f;
            CameraRandomPosition();

        }

        else
        {
            NowCameraShakeTick += Time.deltaTime;


            transform.position = CameraOriginalPosition;
        }


        // 지속시간 체크
        if (NowCameraShakingTime >= MaxCameraShakingTime)
        {
            NowCameraShakingTime = 0.0f;
            MaxCameraShakingTime = 0.0f;

            NowCameraShakeTick = 0.0f;
            MaxCameraShakeTick = 0.0f;

            CameraShakePower = 1.0f;

            transform.position = CameraOriginalPosition;

            IsCameraShaking = false;


        }

        else
        {
            // 3. 카메라 시간 계산
            NowCameraShakingTime += Time.deltaTime;
        }
    }

    public void CameraRandomPosition()
    {
        // 1. 카메라 흔들기
        Vector2 XYCamera = Random.insideUnitCircle * CameraShakePower;

        // 2. 카메라 흔든 뒤에 적용  
        transform.position += new Vector3(XYCamera.x, XYCamera.y, 0);
    }
}
