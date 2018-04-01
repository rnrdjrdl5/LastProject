using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndInterObject : MonoBehaviour
{

    public NewInteractionSkill newInteractionSkill;

    private PhotonView pv;

    struct StructInteraction
    {
        public InteractiveState InterScript;
        public Material OriginalMaterial;
        public MeshRenderer meshRenderer;
    }


    public Material HideMaterial;
    private Material OriginalMaterial;

    List<StructInteraction> Inters;


    public float FindRad;
    private void Awake()
    {
        Inters = new List<StructInteraction>();
        gameObject.GetComponent<SphereCollider>().radius = FindRad;
        pv = GetComponentInParent<PhotonView>();
    }

    int a = 0;

    private void OnTriggerStay(Collider other)
    {

        // 상호작용 인 경우
        if (other.tag == "Interaction")
        {
            // 해당 스크립트를 받아옵니다.
            InteractiveState IS = other.gameObject.GetComponent<InteractiveState>();


            // 충돌물체가 viewid와 같다면 &&
            // 본인이 소유한게 아니라면 &&

            // 문제점 : 
            // 
            if ((newInteractionSkill.GetinterViewID() == IS.photonView.viewID) &&
                (!newInteractionSkill.photonView.isMine))
            {
                Debug.Log("asdF");
                // 물체 등록
                newInteractionSkill.SetinteractiveObject(other.gameObject);
                newInteractionSkill.SetinteractiveState(IS);

                
            }


            // 사용 불가능하면
            if (!IS.GetCanUseObject())
            {
                // 리스트에 없다면 
                if (!HaveInteraction(IS))
                {
                    MeshRenderer mr = IS.gameObject.GetComponent<MeshRenderer>();

                    // 기존 원래 재질을 받아옵니다.
                    Material mt = mr.material;

                    // 새 재질을 적용시킵니다.
                    mr.material = HideMaterial;

                    // 구조체화 시키기
                    StructInteraction SI = new StructInteraction();
                    SI.InterScript = IS;
                    SI.OriginalMaterial = mt;
                    SI.meshRenderer = mr;

                    Debug.Log("@1422151'");


                    // 리스트를 추가합니다.
                    Inters.Add(SI);

                }

            }


        }

    }

    private void Update()
    {
        if (pv.isMine)
        {
            for (int i = Inters.Count - 1; i >= 0; i--)
            {
                if ((Inters[i].InterScript.transform.position - transform.position).magnitude > FindRad)
                {
                    Inters[i].meshRenderer.material = Inters[i].OriginalMaterial;
                    Debug.Log("2131'");
                    Inters.Remove(Inters[i]);
                }
            }
        }

       /* for (int i = Inters.Count - 1; i >= 0; i--)
        {
            Debug.Log(Inters[i]);
        }*/
    }

    bool HaveInteraction(InteractiveState IS)
    {
        for(int i = 0; i < Inters.Count; i++)
        {
            if(Inters[i].InterScript == IS)
            {
                return true;
            }
        }
        return false;
    }

}