﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 주의점. 커플링이 너무  심해질 수 있으니
// 읽기 전용으로만 관리합시다.

public class InGameManager{

    private static InGameManager Instance;


    private GameObject PlayerCamera;

    public GameObject GetPlayerCamera()
    {
        if (PlayerCamera == null)
        {
            PlayerCamera = GameObject.Find("PlayerCamera");
            if (PlayerCamera == null)
                Debug.Log("찾는데 실패함. 싱글톤.");
        }
        return PlayerCamera;
    }

    public static InGameManager GetInstance()
    {
        if (Instance == null)
            Debug.Log("에러 , 싱글톤에서 instance가 awake에서 안만들어짐.");
        return Instance;

    }



     
}
