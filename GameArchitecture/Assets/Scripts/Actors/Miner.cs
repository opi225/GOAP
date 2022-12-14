using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : ActorBase
{
    public override void Start()
    {
        base.Start();
        moveSpeed = 10f;
        goal.Add(new KeyValuePair<string, object>("Ore In Chest", true));
    }

    protected override void Update()
    {
        base.Update();
    }
}
