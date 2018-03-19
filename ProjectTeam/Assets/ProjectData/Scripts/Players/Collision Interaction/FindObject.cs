﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObject : MonoBehaviour {

    private PointToLocation PTL;

    [Header("- 상호작용 UI 표시 거리")]
    [Tooltip(" - 상호작용 기본 UI 거리입니다.")]
    public float MaxLocationDistnace = 0.0f;

    [Header("- 상호작용 액션 거리")]
    [Tooltip(" - 상호작용이 작동할 수 있는 최대거리입니다.")]
    public float MaxInteractionDistance = 0.0f;



    private GameObject ObjectTarget;

    public GameObject GetObjectTarget() { return ObjectTarget; }
    public void SetObjectTarget(GameObject GO) { ObjectTarget = GO; }


    public GameObject PressUIPrefabs;
    private GameObject PressUIObject;



    private GameObject InGameCanvas;


    public GameObject ScoreTimeUIPrefabs;
    private GameObject ScoreTimeUIObject;


    private InteractiveState ObjectState;

    public InteractiveState GetObjectState() { return ObjectState; }
    public void SetObjectState(InteractiveState IS) { ObjectState = IS; }


    // 상호작용이 가능한지 여부, 2m내에서만 승락시킨다.

    // 사용한 곳 : 상호작용 스킬.
    private bool IsInteraction;

    public bool GetIsInteraction() { return IsInteraction; }
    public void SetIsInteraction(bool SI) { IsInteraction = SI; }

    private bool isFindObject = true;

    public bool GetisFindObject() { return isFindObject; }
    public void SetisFindObject(bool FO) { isFindObject = FO; }


    private void Awake()
    {
        IsInteraction = false;
        isFindObject = true;
    }

    void Start() {
        PTL = new PointToLocation();
        PTL.SetPlayerCamera(GameObject.Find("PlayerCamera"));
        InGameCanvas = GameObject.Find("InGameCanvas");
    }


    // 단, 상호작용은 다른곳에서 적용한다.





        // 상호작용과의 거리, 거리가 가까우면 true, 멀면 false
    bool IsInteractionObjectDistance(GameObject Interaction)
    {
        if((Interaction.transform.position - gameObject.transform.position).magnitude <= MaxInteractionDistance)
        {
            return true;
        }
        
        else
        {
            return false;
        }
    }


    void CreateMeleeObject(GameObject Interaction)
    {
        // 상호작용이 가능한 범위라면, 근접 일 때. 
        if (IsInteractionObjectDistance(Interaction) == true)
        {

            // 마우스 버튼 UI가 없는 경우
            if (PressUIObject == null)
            {

                // 생성한다.
                PressUIObject = Instantiate(PressUIPrefabs);

                // 부모설정
                PressUIObject.transform.SetParent(InGameCanvas.transform);

                // 스케일 1로 고정
                PressUIObject.transform.localScale = Vector3.one;



                // 스크린 에 비례해서 UI위치 고정
                Vector3 v3 = new Vector3();

                v3.x = Screen.width / 2;
                v3.y = Screen.height / 2;
                v3.z = 0.0f;

                PressUIObject.transform.position = v3;

            }

        }
    }

    void DestroyMeleeObject()
    {
        if(PressUIObject != null)
        {
            Destroy(PressUIObject);
        }
    }

    void CreateFarObject(GameObject Interaction)
    {
        if (IsInteractionObjectDistance(Interaction) != true)
        {

            if (ScoreTimeUIObject == null)
            {

                ScoreTimeUIObject = Instantiate(ScoreTimeUIPrefabs);

                ScoreTimeUIObject.transform.SetParent(InGameCanvas.transform);

                ScoreTimeUIObject.transform.localScale = Vector3.one;


                Vector3 v3 = new Vector3();

                v3.x = Screen.width / 2;
                v3.y = Screen.height / 2;
                v3.z = 0.0f;

                ScoreTimeUIObject.transform.position = v3;
            }
        }

    }


    void DestroyFarObject()
    {
        if (ScoreTimeUIObject != null)
        {
            Destroy(ScoreTimeUIObject);
        }
    }


    void ChooseIsInteraction(GameObject Interaction)
    {
        if (IsInteractionObjectDistance(Interaction))
        {
            IsInteraction = true;
        }
        else
            IsInteraction = false;
    }


    void ChooseMaterialColor(GameObject Interaction)
    {
        // 초 노 빨
        if (Interaction != null)
        {
            

            MeshRenderer MR = Interaction.GetComponent<MeshRenderer>();

            if (ObjectState.InteractiveTime < 1)
            {
                MR.material.color = Color.green;
            }

            else if (ObjectState.InteractiveTime >= 1 && ObjectState.InteractiveTime < 3)
            {
                MR.material.color = Color.yellow;
            }

            else if (ObjectState.InteractiveTime >= 3 && ObjectState.InteractiveTime < 5)
            {
                MR.material.color = Color.red;
            }

            else
                Debug.Log(" 에러. 시간수치 5초 이후, 색상 정하지 않는다.");
        }
        else
            Debug.Log(" 오브젝트 없음. 에러.");

    }

    void ResetMaterialColor(GameObject Interaction)
    {
        MeshRenderer MR = Interaction.GetComponent<MeshRenderer>();

        MR.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }


    void DestroyPressUIObject()
    {
        Destroy(PressUIObject);
    }

    // Update is called once per frame
    void Update () {
        GameObject Interaction = null;
        if (isFindObject)
        {
            Interaction = PTL.FindObject(gameObject, MaxLocationDistnace);
        }
        else
            Interaction = null;

        // 발견 했을 경우
        if (Interaction != null)
        { 
            // 거리 상황에 따라 UI를 생성한다.
            CreateFarObject(Interaction);

            CreateMeleeObject(Interaction);


            //스킬에서 사용하는 조건문을 설정한다.
            ChooseIsInteraction(Interaction);


            //상호작용 오브젝트에게서 시간을 받아온다.
            SetObjectState(Interaction.GetComponent<InteractiveState>());




            //시간에 비례해서 색상을 정합니다.
            ChooseMaterialColor(Interaction);


            // 레이캐스트로 맞은 상호작용 대상을 ObjectTarget에 저장합니다.
            ObjectTarget = Interaction;
        }


        // 오브젝트를 이미 타겟한 상태
        if(ObjectTarget !=null)
        {

            // 타겟한상태 + 발견하지 못했다면.
            // 사용 한 곳 : 1 갑자기 다른 곳으로 커서를 돌릴때 ( max , melee 상황에서 다른 곳으로 돌릴 때 .)
            //              2 거리가 멀어질 때  ( max 이후로 멀어지는 경우 ) 
            if(Interaction == null)
            {
                // 오브젝트 색상을 원래대로 돌려준다.
                ResetMaterialColor(ObjectTarget);


                // 오브젝트를 삭제하자. 
                // 둘다 삭제하자.
                DestroyMeleeObject();
                DestroyFarObject();

                IsInteraction = false;

            }

            // 타겟한 상태 + 발견했다면.
            else if(Interaction != null)
            {
                // 원거리로 멀어졌다면.
                if(!IsInteractionObjectDistance(Interaction))
                {
                    // 근거리 UI 삭제.
                    DestroyMeleeObject();

                }

                // 근거리로 가까워졌다면.
                else if(IsInteractionObjectDistance(Interaction))
                {
                    DestroyFarObject();

                }

                
            }
                
        }



	}

    public void BackDefault()
    {
        ResetMaterialColor(ObjectTarget);
        DestroyMeleeObject();
        DestroyFarObject();

        ObjectTarget = null;
        IsInteraction = false;

        IsInteraction = false;
    }

    





}
