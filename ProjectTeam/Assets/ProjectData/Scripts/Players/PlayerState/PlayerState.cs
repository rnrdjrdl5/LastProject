﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerState : MonoBehaviour {

    private void Awake()
    {
        animator = GetComponent<Animator>();

        SetHeadObject();

    }

    private void Update()
    {
    }
    
}