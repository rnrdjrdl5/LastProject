using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillState
{
    // 현재 어떤 거리를 가진 스킬인지 파악하기 위해 사용.
    public enum SkillType { Melee, Projectile }

    protected SkillType SkillRangeType;
    
    public SkillType GetSkillRangeType() { return SkillRangeType; }
    public void SetSkillRangeType(SkillType ESR) { SkillRangeType = ESR; }


    // 모든 스킬은 해당 옵션중 필요한 옵션만 사용하게 됩니다.
    [Header(" - 오브젝트")]
    [Tooltip(" - 투사체의 FBX파일입니다. ")]
    public GameObject ProjectileObject;

    [Header(" - 스킬 속성")]
    [Tooltip(" - 투사체 데미지입니다.")]
    public float ObjectDamage = 10.0f;

    [Tooltip(" - 투사체 데미지 충돌 횟수입니다.")]
    public float ObjectDamageNumber = 10.0f;

    [Tooltip(" - 투사체 충돌 제한 횟수입니다.")]
    public float CollisionNumber = 1.0f;

    [Tooltip(" - 투사체 충돌체크 재사용 시간입니다.")]
    public float ReCheckTime = 1000f;

    [Tooltip(" - 투사체의 자연  소멸 시간입니다.")]
    public float DestroyTime = 3.0f;

    [Tooltip(" - 투사체의 이동속도입니다. 1초당 n 만큼의 속도로 이동합니다.")]
    public float ObjectSpeed = 3.0f;

}
