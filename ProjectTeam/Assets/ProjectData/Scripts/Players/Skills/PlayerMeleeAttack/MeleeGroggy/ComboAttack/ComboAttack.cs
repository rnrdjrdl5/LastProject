using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack : DefaultMeleeGroggy
{

    private string OneAttack = "R Hand";
    private string TwoAttack = "L Hand";
    private string ThreeAttack = "Head";

    private GameObject OneAttackObject;
    private GameObject TwoAttackObject;
    private GameObject ThreeAttackObject;



    // 미작성부분 : 
    // 피격 시 해당 ComboAttack 을 true로 교체해줘야함.
    // + 공격 설정 관련된 collision 들 삭제시켜야 함.
    private bool isCanComboAttack = true;
    public bool GetisCanComboAttack() { return isCanComboAttack; }
    public void SetisCanComboAttack( bool CCA) { isCanComboAttack = CCA; }

    protected override void Awake()
    {
        base.Awake();

        Transform[] FindAttacks = gameObject.GetComponentsInChildren<Transform>();

        foreach(Transform child in FindAttacks)
        {
            if(child.name.Contains(OneAttack))
            {
                OneAttackObject = child.gameObject;
            }
            else if(child.name.Contains(TwoAttack))
            {
                TwoAttackObject = child.gameObject;
            }
            else if(child.name.Contains(ThreeAttack) && child.tag == "CheckCollision")
            {
                ThreeAttackObject = child.gameObject;
            }
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    override public bool CheckState()
    {
        if ((EqualState(PlayerState.ConditionEnum.RUN) ||
            EqualState(PlayerState.ConditionEnum.IDLE) ||
            EqualState(PlayerState.ConditionEnum.ATTACK)) &&
            (isCanComboAttack))
        {
            return true;
        }
        else
            return false;
    }

    public override void UseSkill()
    {
        gameObject.GetComponent<Animator>().SetBool("isAttack", true);
        gameObject.GetComponent<PhotonView>().RPC("RionAttackRPC", PhotonTargets.Others);
    }

    /*********************************************************************
    * ************* 아래부터는 RPC 함수 입니다. *************************
    * *******************************************************************/


    [PunRPC]
    void RionAttackRPC()
    {
        gameObject.GetComponent<Animator>().SetBool("isAttack", true);
    }



    /*********************************************
     * 아래부터는 애니메이션 이벤트 입니다.
     ******************************************/



    private CollisionObject CO;
    private CollisionObjectDamage COD;
    private CollisionDamagedDebuff CDD;

    public CollisionObject GetCO() { return CO;}
    public CollisionObjectDamage GetCOD() { return COD; }
    public CollisionDamagedDebuff GetCDD() { return CDD; }

    void ResetInputAttack()
    {
        isCanComboAttack = true;
    }

    void CreateComboAttack(int type)
    {

        if (gameObject.GetComponent<PlayerState>().EqualPlayerCondition(PlayerState.ConditionEnum.ATTACK))
        {
            if (type == 1)
            {
                COD = OneAttackObject.AddComponent<CollisionObjectDamage>();
                CO = OneAttackObject.AddComponent<CollisionObject>();
                CDD = OneAttackObject.AddComponent<CollisionDamagedDebuff>();

            }
            else if (type == 2)
            {
                COD = TwoAttackObject.AddComponent<CollisionObjectDamage>();
                CO = TwoAttackObject.AddComponent<CollisionObject>();
                CDD = TwoAttackObject.AddComponent<CollisionDamagedDebuff>();
            }
            else if (type == 3)
            {
                COD = ThreeAttackObject.AddComponent<CollisionObjectDamage>();
                CO = ThreeAttackObject.AddComponent<CollisionObject>();
                CDD = ThreeAttackObject.AddComponent<CollisionDamagedDebuff>();
            }

            COD.SetObjectDamage(MeleeAttackDamage);
            COD.SetObjectDamageNumber(-1);

            CO.SetUsePlayer("Player" + gameObject.GetComponent<PhotonView>().viewID);
            CO.SetCollisionReCheckTime(1000);

            CDD.SetMaxTime(GroggyTime);
        }
    }

    void DeleteComboAttack(int type)
    {
        if (gameObject.GetComponent<PlayerState>().EqualPlayerCondition(PlayerState.ConditionEnum.ATTACK))
        {
            Debug.Log("해제!");
            DeleteReCheckList(type);

            Destroy(COD);
            Destroy(CO);
            Destroy(CDD);
        }
        
        
    }
    
    void DeleteReCheckList(int type)
    {
        CollisionReCheck[] CRs;
        if (type == 1)
        {
            CRs = OneAttackObject.GetComponents<CollisionReCheck>();
        }
        else if (type == 2)
        {
            CRs = TwoAttackObject.GetComponents<CollisionReCheck>();
        }
        else if (type == 3)
        {
            CRs = ThreeAttackObject.GetComponents<CollisionReCheck>();
        }
        else
        {
            Debug.Log(" ThreeAttack 에러");
            CRs = new CollisionReCheck[1];
        }

        for (int i = 0; i < CRs.Length; i++)
        {
            Destroy(CRs[i]);
        }
    }



}
