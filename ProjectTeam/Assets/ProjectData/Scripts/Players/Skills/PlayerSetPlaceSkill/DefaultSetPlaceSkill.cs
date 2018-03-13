using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSetPlaceSkill : DefaultSkill {

    [Header(" - 목표지점 오브젝트")]
    [Tooltip(" - 플레이어 목표 지점 스킬 사용 시 , 보여 줄 오브젝트")]
    public GameObject PlaceObject;

    [Header(" - 목표지점 최대거리 설정")]
    [Tooltip(" - 플레이어가 지정할 수 있는 최대 목표의 거리")]
    public float MaxTargetDistance;

    protected PointToLocation FindLocationScript;

    // 해당 변수는 카메라에서 레이캐스트를 쏘기 위한 용도입니다.
    private GameObject PlayerCamera;

    // 해당 변수는 현재 스킬이 온,오프를 판단하기 위해 사용됩니다.
    // 지정방법 : 해당 스크립트를 사용하는 스킬의 열거형으로 바꾸기.
    
    // 바꾸면 PlayerState의 열거형과 해당 변수와 판단해서 사용중임을 판단 가능.
    protected PlayerState.ConditionEnum PlayerUsingSkill;


    public void SetPlayerUsingSkill(PlayerState.ConditionEnum CE)
    {
        PlayerUsingSkill = CE;
    }
    public PlayerState.ConditionEnum GetPlayerUsingSkill() { return PlayerUsingSkill; }


    // 만들어지는 게임오브젝트.
    protected GameObject CreatedPlaceObject;

    // 플레이어들이 스킬을 사용할 때 참고할 좌표값, 최종위치에서 결정된다.
    protected Vector3 PlaceVector;

    public void SetPlaceVector(Vector3 V) { PlaceVector = V; }
    public Vector3 GetPlaceVector() { return PlaceVector; }

    // ------   Update  -------
    // 일반 defaultskill하고는 다른 케이스입니다.
    // 온오프 형식이기때문에 재정의합니다.

    override protected void Update () {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            
            if (CheckSkillKey())
            {
                // 해당 스킬중에 같은 키 입력 시 
                if(gameObject.GetComponent<PlayerState>().EqualSkillCondition(PlayerUsingSkill))
                {
                    // NONE으로 돌아가서 해제한다.
                    gameObject.GetComponent<PlayerState>().SetPlayerSkillCondition(PlayerState.ConditionEnum.NONE, CreatedPlaceObject);
                }
                // 처음 시전할 때.
                else
                {
                    // 목표 오브젝트 생성.
                    DuplicatePlaceObject();
                    // 스킬사용중 상태를 해당 스킬로 옮김.
                    gameObject.GetComponent<PlayerState>().SetPlayerSkillCondition(PlayerUsingSkill, CreatedPlaceObject);
                }
            }

            
            if (gameObject.GetComponent<PlayerState>().EqualSkillCondition(PlayerUsingSkill))
            {
                // 목표위치를 갱신합니다.
                UpdatePlaceLocation();
            }


            if(Input.GetMouseButtonUp(0))
            {
                if (gameObject.GetComponent<PlayerState>().EqualSkillCondition(PlayerUsingSkill))
                {
                    // 1. 플레이어가 쓸 최종위치 갱신
                    PlaceVector = CreatedPlaceObject.transform.position;

                    //2. 위치 나타내는 오브젝트삭제(PlayerState에서 처리함), 상태 NONE으로 돌아감.
                    // 이러면 왼쪽 마우스하고 겹쳐서 수정한다. 상태는 애니메이션에서 바꿔주기로.
                    // gameObject.GetComponent<PlayerState>().SetPlayerSkillCondition(PlayerState.ConditionEnum.NONE, CreatedPlaceObject);

                    Destroy(CreatedPlaceObject); // 일단 삭제.
                    // 3. 단, 반응이 일어나는건 상태를 판단하고 나서다.
                    if (CheckState())
                    {
                        UseSkill();
                    }
                    else
                    {
                        // 해당 Place 지정 스킬 사용 불가 시 따로 상태값만 바꿔준다.

                        // 1) 스킬성공 시 ,애니메이션에서 상태 변경
                        // 2) 스킬 실패 시, 여기서 변경. 스킬실패
                        gameObject.GetComponent<PlayerState>().SetPlayerSkillCondition(PlayerState.ConditionEnum.NONE);
                    }
                }
            }


        }
    }


    private void UpdatePlaceLocation()
    {
        Vector3 BlinkObjectPosition = FindLocationScript.GetPointToLocation(gameObject, PlayerCamera, MaxTargetDistance);
        CreatedPlaceObject.transform.position = BlinkObjectPosition;
        CreatedPlaceObject.transform.rotation = transform.rotation;
    }

    private void DuplicatePlaceObject()
    {
        Vector3 BlinkObjectPosition = FindLocationScript.GetPointToLocation(gameObject, PlayerCamera, MaxTargetDistance);
        CreatedPlaceObject = Instantiate(PlaceObject, BlinkObjectPosition + Vector3.up * 0.5f, transform.rotation);
    }



    override protected void Awake()
    {
        base.Awake();
        FindLocationScript = new PointToLocation();
        PlayerCamera = GameObject.Find("PlayerCamera");
        
    }

}
