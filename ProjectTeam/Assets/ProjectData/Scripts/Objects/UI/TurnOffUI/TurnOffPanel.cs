using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffPanel : MonoBehaviour {

    public GameObject CartoonLine;

    private float TurnOffTime = 0;

    public float GetTurnOffTime() { return TurnOffTime; }
    public void SetTurnOffTime(float tot) { TurnOffTime = tot; }

    IEnumerator CoroCutScene;

    private void Start()
    {

    }


    private void Update()
    {
        TurnOffTime -= Time.deltaTime;
        if (TurnOffTime <= 0)
        {
            Destroy(gameObject);
        }
    }


    public void StartCutScene()
    {
        Transform[] tr = GetComponentsInChildren<RectTransform>();

        for (int i = 0; i < tr.Length; i++)
        {

            if (tr[i].name == "CartoonLine")
            {
                CartoonLine = tr[i].gameObject;
                Debug.Log("찾음");
            }
            else
            {
                Debug.Log("못찾음.");
            }
        }

        CoroCutScene = CutScene();
        StartCoroutine(CoroCutScene);
    }


    public enum EnumCutScene{MOVE , WAIT , BACKMOVE };

    float ImageFadeInTime = 3.0f;

    IEnumerator CutScene()
    {

        float MinWidth = -CartoonLine.GetComponent<RectTransform>().position.x;
        Debug.Log("MinWidth :" + MinWidth);
        float MaxWidth = 0.0f;

        
        Vector3 cartoonLinePosition = CartoonLine.GetComponent<RectTransform>().position;
        cartoonLinePosition.Set(MinWidth, cartoonLinePosition.y, cartoonLinePosition.z);
        CartoonLine.GetComponent<RectTransform>().position = cartoonLinePosition;

        EnumCutScene CutSceneType = EnumCutScene.MOVE;

        float NowTime = 0.0f;

        float TesTime = 0.0f;
        
        while (true)
        {
            Debug.Log("돌아간다");

            if (CutSceneType == EnumCutScene.MOVE)
            {

                 // 1. 이미지의 다음값을 선정
                 NowTime += 1 / ImageFadeInTime * Time.deltaTime;

                 float NowWidth = Mathf.Lerp(MinWidth, MaxWidth, NowTime);
                Debug.Log("NowWidth :" + NowWidth);

                cartoonLinePosition.x = NowWidth;

                 CartoonLine.transform.localScale = Vector3.one;

                CartoonLine.transform.position = cartoonLinePosition;
                 
                if (NowTime >= 1.0f)
                {
                    CutSceneType = EnumCutScene.WAIT;
                    yield break;
                }

                else
                    yield return null;
            }
            yield return null;
        }
    }
}
