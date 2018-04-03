using System.Collections;
using System.Collections.Generic;   
using UnityEngine;

public class NormalConditionOption : DefaultSkillConditionOption {
    public override bool CheckCondition()
    {
        {
            if (defaultSkill.GetphotonView().isMine)
            {
                // 2. 키가 눌린 경우
                if (defaultSkill.InputKey.IsUseKey())
                {
                    
                    // 3. 마나가 있는 경우
                    if (defaultSkill.GetmanaPoint().GetNowManaPoint() >= defaultSkill.ManaCost)
                    {

                        //4. 쿨타임 사용중이지 않을 때

                      /*  if (!defaultSkill.coolDown.GetisUseCoolDown())
                        {*/

                            return true;

                        /*}
                        else
                        {
                            Debug.Log("쿨타임중" + defaultSkill.coolDown.GetNowCoolDown());
                            Debug.Log("쿨타임중" + defaultSkill.coolDown.MaxCoolDown);

                        }*/
                    }
                    else
                        Debug.Log("마나부족");
                }
                Debug.Log("키 안눌림");

            }
            
            return false;
        }

    }

    
}
