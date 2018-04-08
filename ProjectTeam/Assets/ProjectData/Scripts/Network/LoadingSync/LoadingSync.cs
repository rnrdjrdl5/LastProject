using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class LoadingSync : Photon.PunBehaviour ,IPunObservable {




    /**** private ****/

    private IEnumerator CoroLoading;                // 로딩 코루틴

    private LoadingPhoton loadingPhoton;            //로딩매니저
    private AsyncOperation async;               // 로딩용

    private float LoadingData;              // 로딩 퍼센트 데이터

    private bool isFinish;                  // 로딩 끝났는지 파악하기 위한 용도
    private bool isOnceLoading;     // Update로 인한 CallRPC 과다호출 막기 위해서

    /**** 접근자 ****/
    public float GetLoadingData()
    {
        return LoadingData;
    }

    public void SetLoadingData(float SLD)
    {
        LoadingData = SLD;
    }




    private void Awake()
    {
        isFinish = false;
        isOnceLoading = false;
}



    void Start()
    {
        loadingPhoton = GameObject.Find("LoadingPhoton").GetComponent<LoadingPhoton>();
        loadingPhoton.AddloadingSync(this);


        // 코루틴 시작
        if (photonView.isMine)
        {
            CoroLoading = Loading();
            StartCoroutine(CoroLoading);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(LoadingData);
        }
        else if (stream.isReading)
        {
            LoadingData = (float)stream.ReceiveNext();
        }
    }


    /**** 함수 ****/

    public void CallFinishLoading()
    {
        if (isOnceLoading == false)
        {
            photonView.RPC("RPCFinishLoading", PhotonTargets.All);
        }
    }

    /**** 코루틴 ****/


    //로딩 Sync
    IEnumerator Loading()
    {
        
        // 비동기 로딩 사용
        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;

        while (true)
        {

            if (LoadingData < 0.9f)
            {
                LoadingData = async.progress;
            }

            else
            {
                LoadingData = 1.0f;
            }

            // 서버에서 로딩이 끝남
            if (isFinish)
            {
                async.allowSceneActivation = true;
                Debug.Log("게임전환");
            }



            yield return null;
        }
    }


    /**** RPC ****/

    // 서버로부터 로딩이 끝났는지 받음
    [PunRPC]
    void RPCFinishLoading()
    {
        if (isOnceLoading == false)
        {
            isFinish = true;
            isOnceLoading = true;
        }
    }
}
