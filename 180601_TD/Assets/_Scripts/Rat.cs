﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeekBrains;


public class Rat : BaseEnemy {

    [SerializeField] Transform bodyTransform;

    protected override void Start()
    {
        base.Start();
        if (bodyTransform != null)
        {
            enemyTransform = bodyTransform;
        }

    }
}
