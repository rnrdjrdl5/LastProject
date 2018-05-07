using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class LoadingPhoton : Photon.PunBehaviour
{
    /**** public ****/

    [System.Serializable]
    public struct PlayerPanel
    {
        public Text playerName;
        public Text playerLoading;
    }

    public PlayerPanel[] playerPanel;                   // 플레이어 UI 패널


    /**** private ****/

    private LoadingSync mineLoadingSync;
    
    private List<LoadingSync> loadingSync;              // 플레이어 동기화 용 스크립트



    /**** 유니티 함수 ****/
    private void Awake()
    {

        // 동기화 스크립트 List 인스턴스화
        loadingSync = new List<LoadingSync>();

        mineLoadingSync = new LoadingSync();

    }

    private void Start()
    {
        // 동기화용 스크립트 멀티 생성
        PhotonNetwork.Instantiate("LoadingSync", Vector3.zero, Quaternion.identity, 0);

        // 플레이어 위치 씬 변경
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable { { "Scene", "Loading" } };
        PhotonNetwork.player.SetCustomProperties(ht);

    }

    private void Update()
    {

        // Player 패널을 그린다.
        DrawPlayerPanel();


        // 로딩조건 판단

        bool isFinishLoading = false;

        if (PhotonNetwork.isMasterClient && loadingSync.Count == PhotonNetwork.playerList.Length)
        {
            for (int i = 0; i < loadingSync.Count; i++)
            {

                if (loadingSync[i].GetLoadingData() >= 0.9f)
                {
                    isFinishLoading = true;
                }
                else
                {
                    Debug.Log("여기서.");
                    isFinishLoading = false;
                    break;
                }

            }

            if (isFinishLoading == true)
            {
                mineLoadingSync.CallFinishLoading ();
              //  PhotonNetwork.isMessageQueueRunning = false;
              //  PhotonNetwork.LoadLevel(1);
                
            }
            else
            {
                Debug.Log("에러");
                for (int j = 0; j < loadingSync.Count; j++)
                {
                    Debug.Log(" 값  : "+loadingSync[j].GetLoadingData());
                }
            }
        }

    }

    /**** 포톤 함수 ****/

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        base.OnPhotonPlayerDisconnected(otherPlayer);

        // 나간 오브젝트 삭제
        for (int i = loadingSync.Count -1; i >=0; i--)
        {
            if (loadingSync[i].photonView.ownerId == otherPlayer.ID)
            {
                loadingSync.Remove(loadingSync[i]);
            }
        }
    }


    /**** 함수 ****/

    // 외부에서 스크립트를 생성, 사용한 곳 : LoadingSync
    public void AddloadingSync(LoadingSync LS)
    {
        
        // 본인 인 경우 추가 등록
        if (LS.photonView.isMine)
            mineLoadingSync = LS;

        loadingSync.Add(LS);

        loadingSync.Sort(
            (LoadingSync One, LoadingSync Two) =>
            {
                if (One.photonView.ownerId > Two.photonView.ownerId)
                    return 1;
                else if (One.photonView.ownerId < Two.photonView.ownerId)
                    return -1;
                return 0;
            });
    }

    // Player 패널을 그린다.
    private void DrawPlayerPanel()
    {

        
        for (int i = 0; i < playerPanel.Length; i++)
        {

            //유저 접속 패널
            if (i < loadingSync.Count)
            {
                playerPanel[i].playerName.text = loadingSync[i].photonView.owner.NickName;
                playerPanel[i].playerLoading.text = loadingSync[i].GetLoadingData().ToString();
            }


            // 유저가 접속 안한 곳
            else
            {
                playerPanel[i].playerLoading.text = "X";
                playerPanel[i].playerName.text = "[ ]";
            }
        }
    }

    


}



