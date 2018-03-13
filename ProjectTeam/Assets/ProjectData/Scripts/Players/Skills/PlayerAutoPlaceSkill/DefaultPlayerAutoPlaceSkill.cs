using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPlayerAutoPlaceSkill : DefaultSkill
{
    [Header(" - 목표지점 최대거리 설정")]
    [Tooltip(" - 플레이어가 지정할 수 있는 최대 목표의 거리")]
    public float MaxTargetDistance;

    // 레이캐스트로 위치 찾기 위한 용도.
    protected PointToLocation FindLocationScript;

    // 현재 마우스가 가리키는 위치를 받습니다.
    protected Vector3 PlaceVector;
    public void SetPlaceVector(Vector3 PV) { PlaceVector = PV; }
    public Vector3 GetPlaceVector() { return PlaceVector; }


    // 레이캐스트 사용하는 용도로 쓰이는 카메라.
    protected GameObject PlayerCamera;

    override protected void Awake()
    {
        base.Awake();
        FindLocationScript = new PointToLocation();
        PlayerCamera = GameObject.Find("PlayerCamera");
    }


    protected override void Update()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            if (CheckSkillKey())
            {
                if (CheckState())
                {
                    if (UseMouseSkill())
                    {
                        PlaceVector = FindLocationScript.GetPointToLocation(gameObject, PlayerCamera, MaxTargetDistance);
                        UseSkill();
                    }

                }
            }
        }
    }



}
