using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSkillCondition{

    protected PhotonView photonView;

    protected DefaultInput InputKey;

    protected float ManaCost;

    protected PlayerManaPoint manaPoint;

    protected CoolDown coolDown;

    public void SettingState(PhotonView pv, DefaultInput di, float mc, PlayerManaPoint mp, CoolDown cd)
    {
        photonView = pv;
        InputKey = di;
        ManaCost = mc;
        manaPoint = mp;
        coolDown = cd;
    }

    public virtual bool SkillCondition()
    {
        Debug.Log("부모");
        return false;
    }

}
