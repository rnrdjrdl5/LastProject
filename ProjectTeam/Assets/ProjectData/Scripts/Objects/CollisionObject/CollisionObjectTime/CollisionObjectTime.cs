using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObjectTime : MonoBehaviour
{


    // 충돌체의 유지시간 , 0 = 유지시간 무한.
    public float collisionObjectTime;

    public void SetObjectTime(float OT) { collisionObjectTime = OT; }
    public float GetObjectTime() { return collisionObjectTime; }

    public float NowTimer = 0.0f;
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        DestroyTimer();
	}

    void DestroyTimer()
    {
         NowTimer += Time.deltaTime;
        if (collisionObjectTime <= NowTimer)
        {
              Debug.Log("삭제 , 시간 : " + NowTimer);
            NowTimer = 0.0f;
            collisionObjectTime = 0.0f;
            ResetSkillOption();
            PoolingManager.GetInstance().PushObject(gameObject);
        }

    }

    private void ResetSkillOption()
    {
        CollisionObject collisionObject = GetComponent<CollisionObject>();
        CollisionObjectDamage collisionObjectDamage = GetComponent<CollisionObjectDamage>();
        NumberOfCollisions numberOfCollisions = GetComponent<NumberOfCollisions>(); 

        // 정보 초기화 필요
        if (collisionObject != null)
            collisionObject.ResetObject();

        if (collisionObjectDamage != null)
            collisionObjectDamage.ResetObject();

        if (numberOfCollisions != null)
            numberOfCollisions.ResetObject();

        CollisionStunDebuff collisionStunDebuff = GetComponent<CollisionStunDebuff>();
        CollisionNotMoveDebuff collisionNotMoveDebuff = GetComponent<CollisionNotMoveDebuff>();
        CollisionDamagedDebuff collisionDamagedDebuff = GetComponent<CollisionDamagedDebuff>();


        if (collisionStunDebuff != null)
            Destroy(collisionStunDebuff);

        if (collisionNotMoveDebuff != null)
            Destroy(collisionNotMoveDebuff);

        if (collisionDamagedDebuff != null)
            Destroy(collisionDamagedDebuff);

        // ReCheck 스크립트 받아옴
        CollisionReCheck[] CRCs = GetComponents<CollisionReCheck>();

        for (int i = CRCs.Length - 1; i >= 0; i--)
        {
            Destroy(CRCs[i]);
        }



    }

    public void ResetObject()
    {
        Debug.Log("시간리셋돌아간다");
        NowTimer = 0.0f;
        collisionObjectTime = 0.0f;
    }
}
