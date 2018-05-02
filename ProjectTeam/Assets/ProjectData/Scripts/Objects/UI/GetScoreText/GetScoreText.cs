using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetScoreText : MonoBehaviour {

    public float GetScoreWaitTime = 0.5f;
    public float GetScoreMoveTime = 0.2f;

    IEnumerator EnumCoro;

    public void CreateScore(int Score)
    {
        // 풀링의 텍스트 이름 수정
        GetComponent<Text>().text = "+" + Score.ToString();

        Vector3 v3 = new Vector3 { x = Screen.width * 0.7f, y = Screen.height * 0.55f, z = 90.0f };

        transform.localScale = Vector3.one;
        transform.position = v3;

        // 4. 이미지를 움직인다. 
        EnumCoro = MoveScoreImage();

        StartCoroutine(EnumCoro);
    }

    enum EnumMoveScore { WAIT, MOVE };
    IEnumerator MoveScoreImage()
    {
        // 1. 이미지 원래 위치 받아온다.
        Vector3 OriginalPosition = transform.position;

        // 2. 이미지 마지막위치 선정
        Vector3 FinalPosition = new Vector3 { x = Screen.width * 0.9f, y = Screen.height * 0.9f, z = 0.0f };

        // 3. Lerp에 쓰일 초기 값을 정한다.
        float NowTime = 0.0f;


        EnumMoveScore MoveScoreType = EnumMoveScore.WAIT;
        // 2가지 단계로 나눈다.
        // 1. 머무르는 시간
        // 2. 날라가는 시간



        while (true)
        {

            if (MoveScoreType == EnumMoveScore.WAIT)
            {
                if (NowTime >= GetScoreWaitTime)
                {
                    MoveScoreType = EnumMoveScore.MOVE;
                    NowTime = 0.0f;

                }
                else
                {
                    NowTime += Time.deltaTime;
                    yield return null;
                }
            }


            if (MoveScoreType == EnumMoveScore.MOVE)
            {

                // 1. 이미지의 다음값을 선정
                NowTime += 1 / GetScoreMoveTime * Time.deltaTime;

                // 2. 다음 위치로 이동
                transform.position = Vector3.Lerp(OriginalPosition, FinalPosition, NowTime);

                // 3. 위치가 1 이상이면 오브젝트 풀에 다시 넣고 끝낸다.
                if (NowTime >= 1.0f)
                {
                    GameObject hit = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.STAR_HIT);
                    //hit.transform.position = camera.worldto

                    Camera c = Camera.main;
                    Vector3 v3 = c.WorldToScreenPoint(transform.position);

                    hit.transform.position = v3;

                    PoolingManager.GetInstance().PushObject(gameObject);
                    yield break;
                }

                else
                    yield return null;
            }




        }

    }
}
