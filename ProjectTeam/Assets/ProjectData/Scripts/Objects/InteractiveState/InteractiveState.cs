using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveState : Photon.MonoBehaviour , IPunObservable {

    [Header(" - 상호작용 시전시간")]
    [Tooltip(" - 상호작용이 완료되기 까지의 시간입니다.(게이지에 사용)")]
    public float InteractiveTime = 0.0f;

    [Header(" - 상호 오브젝트 타입")]
    [Tooltip(" - 테이블 등 오브젝트의 타입.")]
    public DefaultInteraction.EnumInteractiveKey InterObjectType;
   

    
    // 사용자가 이미 한번 뒤집었는지 판단하는 용도.
    // 한번 뒤집었다면 반투명으로 보여줍니다.
    public bool CanUseObject;

    public bool GetCanUseObject()
    {
        return CanUseObject;
    }

    public void SetCanUseObject(bool s)
    {
        CanUseObject = s;
    }


    // 다른 사용자가 뒤집고 있는지 판단하는 용도.
    private bool isCanFirstStart = false;

    public bool GetisCanFirstStart() { return isCanFirstStart; }
    public void SetisCanFirstStart(bool CFS) { isCanFirstStart = CFS; }



    [PunRPC]
    private void RPCOffCanUseObject()
    {
        CanUseObject = false;
    }

    public void CallOffCanUseObject()
    {
        if (PhotonNetwork.isMasterClient)
        {
            CanUseObject = false;
            Debug.Log("123");
        }
        else
        {
            gameObject.GetComponent<PhotonView>().RPC("RPCOffCanUseObject", PhotonTargets.MasterClient);
            Debug.Log("456");
        }
    }

    // Use this for initialization

    private void Awake()
    {
        CanUseObject = true;
    }
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
            if (stream.isWriting)
            {
                stream.SendNext(CanUseObject);
            }

            if(stream.isReading)
            {   
                CanUseObject = (bool)stream.ReceiveNext();
            }
    }
}
