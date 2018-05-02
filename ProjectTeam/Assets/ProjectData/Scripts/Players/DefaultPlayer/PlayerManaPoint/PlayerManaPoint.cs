using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerManaPoint : MonoBehaviour{

    private void Awake()
    {
        if(gameObject.GetPhotonView().isMine)
            SetAwake();   
    }

    private void Update()
    {
        

        if (gameObject.GetPhotonView().isMine)
        {
            ReTimeMana();
        }

    }
}
