using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRionRoar : MonoBehaviour {

    private float RionRoarStunTime = 0.0f;

    public float GetRionRoarStunTime() { return RionRoarStunTime; }
    public void SetRionRoarStunTime(float RRST) { RionRoarStunTime = RRST; }


    private float RionRoarDamage = 0.0f;

    public float GetRionRoarDamage() { return RionRoarDamage; }
    public void SetRionRoarDamage(float RRD) { RionRoarDamage = RRD; }

    public string UsePlayer;

    private List<GameObject> HitGameObjects;

    public void AddHitGameObjects(GameObject GO)
    {
        HitGameObjects.Add(GO);
    }
    public bool FindHitGameObjects(GameObject GO)
    {
        foreach (GameObject _game in HitGameObjects)
        {
            if(GO == _game)
            {
                return true;
            }
        }
        return false;

    }

    // Use this for initialization
    private void Awake()
    {
        HitGameObjects = new List<GameObject>();
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
