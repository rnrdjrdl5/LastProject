using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour {

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
            pm.Objects = new List<GameObject>();

            // ID지정, 프리팹위치 파악용
            pm.ID = i;




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
        }

        else
            go = Poolings[prefabName].PopObject();



        return go;


    }

    public void PushObject(GameObject go)
    {
        Poolings[go.name].PushObject(go,transform);

    }

    public GameObject CreateObject(string prefabName)
    {
        GameObject go = Instantiate(Prefabs[Poolings[prefabName].ID]);
        go.name = prefabName;

        return go;
    }

    

    
}
