using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyAnimation : MonoBehaviour {
    Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (animator != null)
        {
            if (gameObject.activeSelf == true)
            {


                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
                {
                    // UsePlayer = null;
                    animator.SetBool("UseAction", false);
                    PoolingManager.GetInstance().PushObject(this.gameObject);
                }
            }
        }
        
	}
}
