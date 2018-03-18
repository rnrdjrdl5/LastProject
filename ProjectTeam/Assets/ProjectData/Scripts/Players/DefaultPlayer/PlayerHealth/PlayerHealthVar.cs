using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerHealth
{
    private float MaxHealth = 100.0f;   // 최대체력
    private float NowHealth = 100.0f;   // 현재체력

    private float RecvHealth = 0.0f;      // 다른 클라이언트로부터 받아온 체력




    public void SetMaxHealth(float MH) { MaxHealth = MH; }
    public void SetNowHealth(float NH) { NowHealth = NH; }

    public float GetMaxHealth() { return MaxHealth; }
    public float GetNowHealth() { return NowHealth; }



    private GameObject EnemyObject;

    public GameObject GetEnemyObject() { return EnemyObject; }
    public void SetEnemyObject(GameObject s) { EnemyObject = s; }
    /* 아래부터는 인스펙터에서 편집 가능한 public형 변수입니다.*/




}
