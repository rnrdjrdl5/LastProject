﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerManaPoint : MonoBehaviour{

    private void Update()
    {
        ReTimeMana();
        DecreaseMana();
    }
}