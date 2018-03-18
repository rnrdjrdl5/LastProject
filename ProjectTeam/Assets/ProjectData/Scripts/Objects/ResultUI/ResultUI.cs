using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour {

    List<PlayerInGameData> PlayerGameDatas;

    int[] PlayerScores;
    string[] PlayerName;

    public GameObject[] ResultScoreObject;




	// Use this for initialization
	void Start () {
        Debug.Log("플레이어 이름 : " + PhotonNetwork.player.NickName);
        PlayerGameDatas = new List<PlayerInGameData>();
        PlayerScores = new int[PhotonNetwork.room.PlayerCount];


        // 스코어 저장.
        for (int i = 0; i < PlayerScores.Length; i++)
        {
            Debug.Log("출력중" + i);
            PlayerScores[i] = PhotonNetwork.playerList[i].GetScore();
            Debug.Log(PlayerScores[i]);
        }


        // 1. 정보저장이 우선.
     
        for(int i = 0; i < PhotonNetwork.room.PlayerCount; i++)
        {

            PlayerInGameData PIGD = new PlayerInGameData(
                PhotonNetwork.playerList[i].NickName,
                PhotonNetwork.playerList[i].GetScore()
                );

            PlayerGameDatas.Add(PIGD);

        }
       


        Debug.Log(" count 수 : " + PlayerGameDatas.Count);
        // 2. 정렬, 내림차순.
        PlayerGameDatas.Sort(delegate (PlayerInGameData PIGD1, PlayerInGameData PIGD2)
        {
            
             if (PIGD1.GetPlayerScore() > PIGD2.GetPlayerScore())
                 return -1;
             else if (PIGD1.GetPlayerScore() < PIGD2.GetPlayerScore())
                 return 1;
             return 0;
        });

        //3. 패널 수 만큼 루프돌리기
        // resultscoreobject : 패널 
        for(int i = 0; i < ResultScoreObject.Length; i++)
        {


            
                // 해당 패널에 있는 text 다 가져오기, 이름이나 점수 text.

            // 하나의 컴포넌트가 가지고 있는 text를 보여줌.

            // 패널이 가진 text를 가져옴.
            Text[] PlayerStateTexts = ResultScoreObject[i].GetComponentsInChildren<Text>();

            // 만약 현재 있는 패널 수가 저장 된 데이터를 넘어서면. 그냥 즉시 나머지를 보이지 않게 처리한다.

            if (i >= PlayerGameDatas.Count)
            {
                // 모든 텍스트를 안보이게 처리.
                for (int z = 0; z < PlayerStateTexts.Length; z++)
                {
                    Debug.Log("공백처리");
                    PlayerStateTexts[z].text = "----------";
                }
            }

            // 넘어서지 않는다면.
            else
            {
                for (int j = 0; j < PlayerStateTexts.Length; j++)
                {
                    
                    if (PlayerStateTexts[j].rectTransform.name.Contains("Name"))
                    {
                        // 이부분.
                        PlayerStateTexts[j].text = PlayerGameDatas[i].GetPlayerName();
                    }
                    else if (PlayerStateTexts[j].rectTransform.name.Contains("Score"))
                    {
                       PlayerStateTexts[j].text = PlayerGameDatas[i].GetPlayerScore().ToString();
                    }
                }
            }
            
                

        }


    }
	
	// Update is called once per frame
	/*void Update () {
		
	}*/
}
