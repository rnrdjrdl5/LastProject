using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CoolDown
{

    // 쿨타임 돌아가는지 여부를 판단합니다.
    private bool isUseCoolDown = false;

    public void SetisUseCoolDown(bool cd) { isUseCoolDown = cd; }
    public bool GetisUseCoolDown() { return isUseCoolDown; }


    // 현재 쿨타임을 나타내줍니다.
    private float NowCoolDown;
    
    public float GetNowCoolDown() { return NowCoolDown; }
    public void SetNowCoolDown(float NCD) { NowCoolDown = NCD; }


    // 최대 쿨타임을 나타냅니다.
    public float MaxCoolDown;

    // 시간 당 쿨타임 감소
    public void DecreaseCoolDown()
    {
        // 쿨타임이 줄어드는 상태라면
        if(isUseCoolDown)
        {
            // 다음 프레임 때 쿨타임이 모두 줄어든다면
            if(NowCoolDown - Time.deltaTime <= 0)
            {
                isUseCoolDown = false;
                NowCoolDown = 0;
            }

            // 아니라면
            else
            {
                NowCoolDown -= Time.deltaTime;
            }
        }
    }


    public void CalcCoolDown()
    {
        NowCoolDown = MaxCoolDown;
    }

}
