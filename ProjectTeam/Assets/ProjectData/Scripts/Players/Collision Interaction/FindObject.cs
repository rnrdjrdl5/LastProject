using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObject : MonoBehaviour {

    private PointToLocation PTL;

    [Header("- 상호작용 최대 거리")]
    [Tooltip(" - 상호작용이 작동할 수 있는 최대거리입니다.")]
    public float MaxLocationDistnace = 0.0f;

    private GameObject ObjectTarget;

    public GameObject PressUIPrefabs;
    private GameObject PressUIObject;

    private GameObject InGameCanvas;

    [Header("- UI 추가 y축 위치")]
    [Tooltip(" - 기본 위치는 상호작용 오브젝트의 중앙, y축을 추가 더함")]
    public float UILocation = 0.0f;
	// Use this for initialization

    
    private void Awake()
    {

    }

    void Start () {
        PTL = new PointToLocation();
        PTL.SetPlayerCamera(GameObject.Find("PlayerCamera"));
        InGameCanvas = GameObject.Find("InGameCanvas");
    }
	
	// Update is called once per frame
	void Update () {


        GameObject Interaction = PTL.FindObject(gameObject, MaxLocationDistnace);

        if(Interaction != null)
        {
            ObjectTarget = Interaction;

            Debug.Log("충돌성공");



            MeshRenderer MR = Interaction.GetComponent<MeshRenderer>();

            MR.material.color = Color.black;

            // 생성하고 자식으로 변경시키기.

            if (PressUIObject == null)
            {
                PressUIObject = Instantiate(PressUIPrefabs);
                PressUIObject.transform.SetParent(InGameCanvas.transform);

            }

            Vector3 v3 = Interaction.transform.position;
             v3.y += UILocation;
            PressUIObject.transform.position = Camera.main.WorldToScreenPoint(v3);
            


        }


        // 오브젝트를 이미 타겟한 상태
        if(ObjectTarget !=null)
        {
            // 타겟한상태 + 발견하지 못했다면.
            if(Interaction == null)
            {
                // 색상의 색을 다시 바꿔준다.
                ObjectTarget.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                // 해제시킨다.
                ObjectTarget = null;

                // UI 제거
                Destroy(PressUIObject);
            }
                
        }
	}
}
