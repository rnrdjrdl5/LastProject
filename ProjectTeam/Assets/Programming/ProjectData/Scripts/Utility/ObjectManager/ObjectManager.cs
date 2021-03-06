﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//다른 클라이언트에서 오브젝트를 찾기 위해서 사용 , 
// 점수를 확인하기 위해서도 사용
public class ObjectManager : MonoBehaviour {

    private static ObjectManager objectManager;
    public static ObjectManager GetInstance() { return objectManager; }

    public int MaxInterObj { get; set; }
    public List<GameObject> InterObj;
    public void AddInterObj(GameObject go) { InterObj.Add(go); }
    public PhotonManager photonManager;

    private void Awake()
    {
        objectManager = this;
    }

    private void Start()
    {
        MaxInterObj = InterObj.Count;
        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
    }

    public GameObject FindObject(int vID)
    {
        for (int i = 0; i < InterObj.Count; i++)
        {


            if (InterObj[i].GetPhotonView().viewID == vID)
            {
                return InterObj[i];
            }
        }
        return null;
    }

    public void RemoveObject(int vID)
    {

        for (int i = 0; i < InterObj.Count; i++)
        {

            if (InterObj[i].GetPhotonView().viewID == vID)
            {
                // 오브젝트에서 삭제
                InterObj.Remove(InterObj[i]);

                // 쥐 플레이어 인 경우
                if ((string)PhotonNetwork.player.CustomProperties["PlayerType"] == "Cat")
                {
                    // 오브젝트 개수에 비례해서 감소 할 값 지정
                    int DecreaseCatScore = photonManager.MaxCatScore / MaxInterObj;

                    Debug.Log(DecreaseCatScore);

                    int NowCatScore = (int)PhotonNetwork.player.CustomProperties["CatScore"];

                    PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "CatScore", NowCatScore - DecreaseCatScore } });


                }


            }
        }

    }
}
