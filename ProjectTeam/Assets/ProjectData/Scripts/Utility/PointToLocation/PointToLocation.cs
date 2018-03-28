﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************
*
* 코더 : 반재억
* 일자 : 2018-02-21
* 코딩목적 : 마우스로 특정 지점을 편하게 지정할 수 있도록 사용.
* 특이사항 
*  - 해당 기능은 Blink 내부의 함수와 같습니다.
*    Blink 내부의 함수를 이 스크립트의 함수를 사용하도록 교체합시다. 
*    
**********************************************/
public class PointToLocation{

    // 해당 변수는 레이캐스트의 무한 거리를 의미합니다.
    // 마우스 위치로 지정한 거리만큼 레이캐스트를 쐈을 때,
    // 도착하지 않는다면 끝지점부터 수직으로 레이캐스트를 쏩니다.
    // ex) 사이퍼즈의 고각 스킬.

        

    private float           RaycastInfinity             = 10000.0f;

    // 카메라의 시야가 막히는 오브젝트를 찾았을 때입니다.
    // 찾았을 때 y축으로 추가 이동을 사용하지 않습니다.
    bool isFindWall = false;
    public bool GetisFindWall() { return isFindWall; }
    public void SetisFindWall(bool FW) { isFindWall = FW; }


    private GameObject PlayerCamera;

    public GameObject GetPlayerCamera() { return PlayerCamera; }
    public void SetPlayerCamera(GameObject go) { PlayerCamera = go; }

    private PlayerCamera cameraScript;

    public PlayerCamera GetcameraScript() { return cameraScript; }
    public void SetcameraScript(PlayerCamera ps) { cameraScript = ps; }



    public Vector3 GetPointToLocation(GameObject UseObject , GameObject PlayerCamera , float MaxLocationDistance)
    {
        RaycastHit hit;

        Vector3 MouseVector3 = FindMouseCursorPosition(UseObject, PlayerCamera);

        
        Vector3 BlinkVector3 = Vector3.zero;
        if (Physics.Raycast(PlayerCamera.GetComponent<Transform>().position, MouseVector3, out hit, MaxLocationDistance, 1 << LayerMask.NameToLayer("Floor")))
        {
            BlinkVector3 = hit.point;
        }

        else
        {
            // 1. 방향을 거리적용
            Vector3 NotTargetBlink = PlayerCamera.GetComponent<Transform>().position + MouseVector3 * MaxLocationDistance;

            if (Physics.Raycast(NotTargetBlink, Vector3.down, out hit, RaycastInfinity, 1 << LayerMask.NameToLayer("Floor")))
            {
 
                BlinkVector3 = hit.point;
                //   BlinkVector3 += Vector3.up * 0.5f;
            }
            else
            {
                Debug.Log("레이캐스트 못찾았다. 위치를  본인위치로 돌려준다.");
                return UseObject.transform.position;

            }
        }

        return BlinkVector3;
      
    }

    public GameObject FindObject(GameObject UseObject , float MaxLocationDistance)
    { 
        Vector3 MouseVector3 = FindMouseCursorPosition(UseObject, PlayerCamera);

        RaycastHit hit;

        //CameraDistanceTriangle


        if (Physics.Raycast(PlayerCamera.transform.position + MouseVector3.normalized * cameraScript.CameraDistanceTriangle,
            MouseVector3, out hit, MaxLocationDistance, 1 << LayerMask.NameToLayer("MainObject")))
        {
            return hit.collider.gameObject;
            
        }
        else
            return null;
    }

    public Vector3 FindWall(GameObject UseObject , float CameraZommPer)
    {
        // 카메라가 마지막에 위치할 자리입니다.
        Vector3 FindPostCameraPosition = PlayerCamera.transform.position;

        // 1. 마우스 위치 , 시점의 위치를 받아옵니다.
        Vector3 MouseVector3 = FindMouseCursorPosition(UseObject, PlayerCamera);

        // 2. 레이를 쏴버립니다.
        RaycastHit hit;

        // 3 -1 레이를 쏘고 맞는다면?
        // -1 으로 비트연산자를 실시해서
        // 해당 레이어이외에 충돌을 다 받는다.

        int layerMask = 1 << LayerMask.NameToLayer("Player");
        layerMask = ~layerMask;
        if (Physics.Raycast(
            UseObject.transform.position,
            (PlayerCamera.transform.position - Vector3.up * 0.2f - UseObject.transform.position).normalized,
            out hit,
            cameraScript.CameraDistanceTriangle,
            layerMask))

        {

            // 3-2 : 카메라를 레이가 맞은 자리로 당기고,
            //      값을 다시 가공합니다.

            FindPostCameraPosition = UseObject.transform.position + (hit.point - UseObject.transform.position) * CameraZommPer;

            FindPostCameraPosition += Vector3.up * 0.2f;
            isFindWall = true;


        }
        else
        {
            isFindWall = false;
        }

        // 4. 맞지않는다면 원래 값을 리턴합니다.
        // 맞았으면 맞은 값을 리턴합니다.
        return FindPostCameraPosition;
    }
    


    private Vector3 FindMouseCursorPosition(GameObject UseObject , GameObject PlayerCamera)
    {
        Quaternion q2 = Quaternion.Euler(0,
            UseObject.transform.eulerAngles.y, 0);

        Vector3 MouseVector3 = q2 * Vector3.forward;


        Quaternion q3 = Quaternion.Euler(PlayerCamera.GetComponent<PlayerCamera>().CameraRad, 0, 0);

        MouseVector3 = MouseVector3 * (q3 * Vector3.forward).z;
        MouseVector3.y = (q3 * Vector3.forward).y;


        return MouseVector3;
    }

}
