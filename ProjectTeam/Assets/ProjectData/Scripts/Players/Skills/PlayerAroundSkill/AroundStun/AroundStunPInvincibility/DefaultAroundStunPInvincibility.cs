using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAroundStunPInvincibility : DefaultAroundStun
{

    [Header(" - 무적 속성")]
    [Tooltip(" - 무적 지속 시간입니다.")]
    public float InvincibilityTime = 0.0f;

    protected PlayerInvincibilityBuff PIB;

    protected virtual void SetInvincibility()
    {
        if (gameObject.GetComponent<PlayerInvincibilityBuff>() == null)
        {
           PIB = gameObject.AddComponent<PlayerInvincibilityBuff>();
            PIB.SetMaxDebuffTime(InvincibilityTime);
            PIB.SetNowDebuffTime(0);
        }
        else
        {
            PIB = gameObject.GetComponent<PlayerInvincibilityBuff>();

            // 현재 무적버프 남은 시간이 새로운 무적버프보다 낮을 경우 갱신함.
            if(PIB.GetMaxDebuffTime() - PIB.GetNowDebuffTime() <= InvincibilityTime)
            {
                PIB.SetMaxDebuffTime(InvincibilityTime);
                PIB.SetNowDebuffTime(0);
                Debug.Log("무적버프 갱신");
            }
            
            // 현재 남은 무적버프 시간이 더 길다면 갱신하지 않고 놔둔다.
            else
            {
                Debug.Log("무적버프 갱신안함.");
            }
        }
        
    }
}
