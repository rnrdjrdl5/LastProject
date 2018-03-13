using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 코더 : 반재억
// 제작일자 : 2018-02-22
// 사용목적 : 범위공격에 의한 충돌체크 처리용도

public class OnceAttackRange : MonoBehaviour {

    private float Damage;// 데미지.
    public void setDamage(float dmg) { Damage = dmg; }
    public float getDamage() { return Damage; }

    private List<GameObject> Objects;

    private float DestroyTime; // 공격판정 유지시간.
    public void SetDestroyTime(float dt) { DestroyTime = dt; }
    public float GetDestroytime() { return DestroyTime; }

    public void AddHitGameObjects(GameObject GO)
    {
        Objects.Add(GO);
    }
    public bool FindHitGameObjects(GameObject GO)
    {
        foreach (GameObject _game in Objects)
        {
            if (GO == _game)
            {
                return true;
            }
        }
        return false;
    }


    // Use this for initialization
    void Start () {
        Objects = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
