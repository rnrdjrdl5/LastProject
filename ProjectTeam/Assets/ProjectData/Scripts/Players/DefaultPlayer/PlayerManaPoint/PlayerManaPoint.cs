using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerManaPoint : MonoBehaviour{

    private void Awake()
    {
        SetAwake();   
    }

    private void Update()
    {
        

        if (gameObject.GetPhotonView().isMine)
        {
            ReTimeMana();
            NowMPImage.fillAmount = NowManaPoint / MaxManaPoint;
        }

    }
}
