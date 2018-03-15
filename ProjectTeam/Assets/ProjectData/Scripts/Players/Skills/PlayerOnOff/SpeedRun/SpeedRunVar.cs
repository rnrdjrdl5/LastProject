using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SpeedRun
{
    [Header(" - 대쉬 속도")]
    [Tooltip(" - 대쉬 속도량입니다. ")]
    public float SpeedRunSpeed = 0.0f;


    [Header(" - 이펙트")]
    [Tooltip(" - 대쉬 이펙트입니다. ")]
    public GameObject SpeedRunEffectPrefab;

    private GameObject SpeedRunEffectObject;
}
