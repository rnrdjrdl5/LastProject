using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMeleeAttack : DefaultSkill
{

    // 투사체와는 다른 구조를 가지고 있습니다.
    // 왜냐하면, 투사체가 가지는 오브젝트가 필요 없기 때문입니다.

    [Header(" - 근접공격 설정")]
    [Tooltip(" - 근접공격 공격력")]
    public float MeleeAttackDamage;
    public void SetMeleeAttackDamage(float MAD) { MeleeAttackDamage = MAD; }
    public float GetMeleeAttackDamage() { return MeleeAttackDamage; }

    


    override protected void Update()
    {
        base.Update();
    }

    
}
