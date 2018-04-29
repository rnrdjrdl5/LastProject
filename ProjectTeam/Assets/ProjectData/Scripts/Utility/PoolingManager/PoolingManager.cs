using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour {

    public enum EffctType
    {
        ATTACK ,

        ATTACKLINE1 , ATTACKLINE2, ATTACKLINE3,

        BIGDUST_BIG, BIGDUST_SMALL, MIDDLEDUST_BIG , MIDDLEDUST_SMALL , SMALL_DUST_SMALL,

        MOUSE_START_DASH
    }

    public GameObject CreateEffect(EffctType effectType)
    {
        GameObject effect = null;
        switch(effectType)
        {
            case EffctType.ATTACK:
                effect = PopObject("Cat_Effect_FryPan_Attack_01");
                 break;

            case EffctType.ATTACKLINE1:
                effect = PopObject("Hit_Line_Effect_01");
                break;
            case EffctType.ATTACKLINE2:
                effect = PopObject("Hit_Line_Effect_02");
                break;
            case EffctType.ATTACKLINE3:
                effect = PopObject("Hit_Line_Effect_03");
                break;

            case EffctType.BIGDUST_BIG:
                effect = PopObject("FX_YSK_Prefab_Dust_Big_Big");
                break;
            case EffctType.BIGDUST_SMALL:
                effect = PopObject("FX_YSK_Prefab_Dust_Big_Small");
                break;
            case EffctType.MIDDLEDUST_BIG:
                effect = PopObject("FX_YSK_Prefab_Dust_Middle_Big");
                break;
            case EffctType.MIDDLEDUST_SMALL:
                effect = PopObject("FX_YSK_Prefab_Dust_Middle_Small");
                break;
            case EffctType.SMALL_DUST_SMALL:
                effect = PopObject("FX_YSK_Prefab_Dust_Small_Small");
                break;
            case EffctType.MOUSE_START_DASH:
                effect = PopObject("FX_KDH_Prefab_MouseStartDash");
                break;




        }

        return effect;
    }

    static private PoolingManager poolingManager;

    static public PoolingManager GetInstance()
    {
        return poolingManager;
    }


    public Dictionary<string, PoolingObject> Poolings;

    public GameObject[] Prefabs;



    private void Awake()
    {
        Poolings = new Dictionary<string, PoolingObject>();

        poolingManager = this;
    }

    // Use this for initialization
    void Start () {

        for (int i = 0; i < Prefabs.Length; i++)
        {
            // 오브젝트 풀링 인스턴스화
            PoolingObject pm = new PoolingObject();

            pm.Objects = new List<GameObject>();        // 오브젝트 
            pm.ActiveObjects = new List<GameObject>();
           // ID지정, 프리팹위치 파악용
            pm.ID = i;

            // 상위 접근용
            pm.poolingManager = this;



            // 오브젝트 풀링 최소 오브젝트 생성
            for (int j = 0; j < 5; j++)
            {
                // 1. 게임 오브젝트 생성
                GameObject go = Instantiate(Prefabs[i]);
                go.name = Prefabs[i].name;

                // 2. 기본 속성 정의
                pm.InitProp(go, transform);

                // 3. 오브젝트 풀링
                pm.Objects.Add(go);
            }

            // 등록
            Poolings.Add(Prefabs[i].name, pm);
        }
	}
	
	// Update is called once per frame
	void Update () {

	}

    public GameObject PopObject(string prefabName)
    {
        GameObject go;
        if (Poolings[prefabName].Objects.Count == 0)
        {
            go = CreateObject(prefabName);
            Poolings[prefabName].ActiveObjects.Add(go);
        }

        else
            go = Poolings[prefabName].PopObject();


        ObjectIDScript objectIDScript = go.GetComponent<ObjectIDScript>();

        // objectID가 존재하면 지급, 
        if (objectIDScript != null)
        {
            // 고유 값 적용
            objectIDScript.SetID();
        }

        
        

        return go;


    }

    public void PushObject(GameObject go)
    {
        Poolings[go.name].PushObject(go,transform);

        ObjectIDScript objectIDScript = go.GetComponent<ObjectIDScript>();

        if(objectIDScript != null)
            objectIDScript.DeleteID();

    }

    public GameObject CreateObject(string prefabName)
    {
        GameObject go = Instantiate(Prefabs[Poolings[prefabName].ID]);
        go.name = prefabName;

        return go;
    }


    // 맞았을 때 RPC 이용해서 다른 곳의 ID 검색
    public GameObject FindObjectUseObjectID(int ObjectID)
    {

        for (int i = 0; i < GetInstance().Prefabs.Length; i++)
        {

            List<GameObject> gos = GetInstance().Poolings[GetInstance().Prefabs[i].name].ActiveObjects;

            for (int j = 0; j < gos.Count; j++)
            {
                ObjectIDScript objectIDScript = gos[j].GetComponent<ObjectIDScript>();
                if (objectIDScript != null)
                {

                    if (objectIDScript.ID == ObjectID)
                    {
                        return gos[j].gameObject;
                    }
                }
            }

            
        }
        Debug.Log(" 또못찾음");
        return null;

    }



}
