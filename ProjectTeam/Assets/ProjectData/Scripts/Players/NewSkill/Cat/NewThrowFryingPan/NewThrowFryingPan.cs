using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// DefaultNewSkill을 상속받습니다.
public class NewThrowFryingPan : DefaultNewSkill
{
    public OneKeyInput oneKeyInput;

    public ActiveSkillCondition ac;

    // 버프 주기

    // 해당 스킬의 속성을 결정합니다.
    override protected void Awake()
    {
        ac = new ActiveSkillCondition();

        base.Awake();

        //키 인식 방법을 적용합니다.
        InputKey = oneKeyInput;

        //스킬 정보들 중 투사체 정보만 사용합니다.
        skillState.SetSkillRangeType(SkillState.SkillType.Projectile);

        // 스킬 조건 클래스를 등록
        skillCondition = ac;

        //스킬 조건에 필요한 정보를 등록합니다.
        skillCondition.SettingState(photonView, InputKey, ManaCost, manaPoint, coolDown);
        
    }


    // 재정의
    protected override bool CheckState()
    {
        //이동중이거나 가만히 있을 때 가능합니다.
        if ((
            playerState.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
            playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN)))

        {
            return true;
        }
        else
            return false;
    }

    // 재정의
    protected override void UseSkill()
    {
        animator.SetTrigger("ThrowFryingPanTrigger");
    }

    void CreateFryingPan()
    {

        float BulletDistance = 1.0f;
        float CharacterHeight = 1.2f;

        Vector3 BulletDefaultPlace = transform.forward * BulletDistance;

        BulletDefaultPlace.y += CharacterHeight;

        GameObject CharmBullet = Instantiate(skillState.ProjectileObject, transform.position + (BulletDefaultPlace), Quaternion.identity);

        SetObjectData(CharmBullet);
    }

    //[PunRPC]

}
