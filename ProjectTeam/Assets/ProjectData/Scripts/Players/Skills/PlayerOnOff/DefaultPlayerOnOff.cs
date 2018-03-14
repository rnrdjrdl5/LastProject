using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPlayerOnOff : DefaultSkill {

    protected bool isSkillOff = false;

    protected override void Update()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            if (CheckSkillKey())
            {
                if (CheckState())
                {
                    if (UseMouseSkill())
                    {
                        isSkillOff = true;
                        UseSkill();
                    }

                }

                // 도중에 피격맞으면 꺼지도록.
            }
            else
            {
                if (isSkillOff)
                {
                    Debug.Log("스킬해제시작");
                    OffUseSkill();
                    isSkillOff = false;
                }
            }


        }
    }

    protected virtual void OffUseSkill()
    {

    }
}
