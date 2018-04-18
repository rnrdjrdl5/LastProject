using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour {

    public GameObject GetScoreImage;

    public enum EffctType
    { ATTACK }

    public GameObject CreateEffect(EffctType effectType)
    {
        GameObject effect = null;
        switch(effectType)
        {
            case EffctType.ATTACK:
                effect = PopObject("Cat_Effect_FryPan_Attack_01");
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

        go.GetComponent<ObjectIDScript>().SetID();

        
        

        return go;


    }

    public void PushObject(GameObject go)
    {
        Poolings[go.name].PushObject(go,transform);

        go.GetComponent<ObjectIDScript>().DeleteID();

    }

    public GameObject CreateObject(string prefabName)
    {
        GameObject go = Instantiate(Prefabs[Poolings[prefabName].ID]);
        go.name = prefabName;

        return go;
    }


    public GameObject FindObjectUseObjectID(int ObjectID)
    {

        /* // 오브젝트 풀링 매니저의 프리팹 수만큼 루프
         for (int i = 0; i < GetInstance().Prefabs.Length; i++)
         {

             // 하나의 리스트부터 불러온다.
             List<GameObject> gos = GetInstance().Poolings[GetInstance().Prefabs[i].name].Objects;

             // 리스트 마다 for 돌림
             for (int j = 0; j < gos.Count; j++)
             {
                 Debug.Log("gos[j].GetComponent<ObjectIDScript>().ID  : " + gos[j].GetComponent<ObjectIDScript>().ID);
                 Debug.Log(ObjectID);
                 // 해당 게임오브젝트의 번호값과 일치하면.
                 if (gos[j].GetComponent<ObjectIDScript>().ID == ObjectID)
                 {
                     return gos[j].gameObject;
                 }

             }
         }
         Debug.Log("못찾음");
         return null;*/

        for (int i = 0; i < GetInstance().Prefabs.Length; i++)
        {

            List<GameObject> gos = GetInstance().Poolings[GetInstance().Prefabs[i].name].ActiveObjects;

            for (int j = 0; j < gos.Count; j++)
            {
                if (gos[j].GetComponent<ObjectIDScript>().ID == ObjectID)
                {
                    return gos[j].gameObject;
                }
            }

            
        }
        Debug.Log(" 또못찾음");
        return null;

    }



}
