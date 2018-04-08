using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/********
 *  구현방법
 *  CustomProperty의 해쉬값을 이용해서 각종 로딩 여부 파악 ( 코루틴 사용 ) 
 *  
 * 
 * ******/
public class PhotonManager : Photon.PunBehaviour{

    /**** Private ****/
    IEnumerator CoroCheckGameStart;             // 게임시작체크 코루틴
    IEnumerator CoroCheckCreatePlayer;             // 플레이어생성체크 코루틴
    IEnumerator CoroCheckGameFinish;                // 게임종료체크 코루틴

    
    private GameObject CurrentPlayer;
    private List<GameObject> AllPlayers;


    /**** 접근자 ****/

    public void SetCurrentPlayer(GameObject Go)
    {
        CurrentPlayer = Go;
    }
    public void AddAllPlayer(GameObject Go) {
        AllPlayers.Add(Go);
    }

    public GameObject GetCurrentPlayer() {
        return CurrentPlayer;
    }


    /**** 유니티 함수 ****/

    private void Awake()
    {
        // 코루틴들 지정
        CoroCheckGameStart = CheckGameStart();
        CoroCheckCreatePlayer = CheckCreatePlayer();
        CoroCheckGameFinish = CheckGameFinish();

        // 모든 플레이어 담을 오브젝트 생성
        AllPlayers = new List<GameObject>();
    }

    private void Start()
    {

        // 플레이어 위치 씬 변경
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable { { "Scene", "InGame" } };
        PhotonNetwork.player.SetCustomProperties(ht);

        StartCoroutine(CoroCheckGameStart);
    }

    private void Update()
    {
        for (int i = 0; i < AllPlayers.Count; i++)
        {
            Debug.Log(AllPlayers[i].name);
        }
    }

    /**** 코루틴 ****/

    // 로딩 완료됐는지 판단한다.
    IEnumerator CheckGameStart()
    {
        
        // 루프
        while (true)
        {
            bool isInGame = true;

            // 로딩 안된 클라이언트 찾기
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                if ((string)PhotonNetwork.playerList[i].CustomProperties["Scene"] != "InGame")
                {
                    isInGame = false;
                }

                else
                {
                    Debug.Log(" 모든 플레이어 로딩 대기중... ");
                }
            }

            // 모두 로딩 완료
            if (isInGame)
            {

                // 서버 ++ ) 고양이 쥐 지정, 생성
                if (PhotonNetwork.isMasterClient)
                {

                    isInGame = false;

                    //  랜덤 플레이어 설정
                    int BossPlayer = Random.Range(0, PhotonNetwork.playerList.Length);

                    // 해쉬 생성
                    ExitGames.Client.Photon.Hashtable CatHash = new ExitGames.Client.Photon.Hashtable { { "PlayerType", "Cat" } };
                    ExitGames.Client.Photon.Hashtable MouseHash = new ExitGames.Client.Photon.Hashtable { { "PlayerType", "Mouse" } };

                    // 해쉬 대입
                    for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                    {
                        if (BossPlayer == i)
                        {
                            PhotonNetwork.playerList[i].SetCustomProperties(CatHash);
                        }
                        else
                        {
                            PhotonNetwork.playerList[i].SetCustomProperties(MouseHash);
                        }
                    }

                    Debug.Log("ㅁㅁㅁㅁㅁ플레이어생성ㅁㅁㅁㅁㅁ");
                    // 플레이어 생성
                    photonView.RPC("RPCCreatePlayer", PhotonTargets.All);
                    
                }

                

               //플레이어 생성 확인 코루틴 시작
                StartCoroutine(CoroCheckCreatePlayer);

                yield break;
        
                
            }

            yield return null;
        }
        
    }

    // 플레이어 생성 완료됐는지 판단한다.
    IEnumerator CheckCreatePlayer()
    {
        while (true)
        {
            bool isCreate = true;

            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                string CreatePlayerState = (string)PhotonNetwork.playerList[i].CustomProperties["Offset"];

                if (CreatePlayerState != "CreateComplete")
                {
                    isCreate = false;
                }
                else
                {
                    Debug.Log("  플레이어 생성 대기 .. ");
                }
            }

            if (isCreate)
            {
                Debug.Log("플레이어 생성 완료!");




                

                // 게임 종료조건 코루틴 생성
                StartCoroutine(CoroCheckGameFinish);

                yield break;
            }


            yield return null;
        }
    }

    // 게임 끝나는 지 판단
    IEnumerator CheckGameFinish()
    {
        while (true)
        {
            bool isFinish = true;
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {

                string PlayerType = (string)PhotonNetwork.playerList[i].CustomProperties["PlayerType"];

                if (PlayerType == "Mouse")
                {
                    isFinish = false;
                }
            }

            if (isFinish)
            {
                Debug.Log("게임끝 다죽음");
            }

            yield return null;
        }
    }

    /**** RPC ****/

    // 플레이어 생성
    [PunRPC]
    void RPCCreatePlayer()
    {
        Debug.Log("aa + " + PhotonNetwork.player.ID);


        // 플레이어 생성
        string PlayerType = (string)PhotonNetwork.player.CustomProperties["PlayerType"];

        if (PlayerType == "Cat")
            PhotonNetwork.Instantiate("NewCatBoss", Vector3.zero, Quaternion.identity, 0);

        else if (PlayerType == "Mouse")
            PhotonNetwork.Instantiate("MouseRunner", Vector3.zero, Quaternion.identity, 0);


        // 생성했다는 의미로 오프셋 사용
        PhotonNetwork.player.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable{{"Offset", "CreateComplete" }});
        
    }




}




