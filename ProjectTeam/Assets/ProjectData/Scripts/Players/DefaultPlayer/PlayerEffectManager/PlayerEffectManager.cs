using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectManager : MonoBehaviour {

    public enum EnumPlayerEffect
    { ATTACK }

    private EnumPlayerEffect playerEffectType;

    public void SetplayerEffectType(EnumPlayerEffect EPE) { playerEffectType = EPE; }
    public EnumPlayerEffect GetplayerEffectType() { return playerEffectType; }

}
