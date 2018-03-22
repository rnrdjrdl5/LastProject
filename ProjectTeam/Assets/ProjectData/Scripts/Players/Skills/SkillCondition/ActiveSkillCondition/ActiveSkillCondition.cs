using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillCondition : DefaultSkillCondition
{
    /* public override bool SkillCondition()
     {
         if (photonView.isMine)
         {
             // 2. 키가 눌린 경우
             if (InputKey.IsUseKey())
             {

                 // 3. 마나가 있는 경우
                 if (manaPoint.GetNowManaPoint() >= ManaCost)
                 {

                     //4. 쿨타임 사용중이지 않을 때
                     if (!coolDown.GetisUseCoolDown())
                     {

                         return true;
                     }
                 }
             }
         }
         return false;
     }*/

    public override bool SkillCondition()
    {
        if(photonView.isMine)
        {
            if(InputKey.IsUseKey())
            {
                if(manaPoint.GetNowManaPoint() >= ManaCost)
                {
                    if(!coolDown.GetisUseCoolDown())
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }



}
