using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Smith Class
 This actor will take copper and tin from chests, and make bronze ingots out of them*/

public class Smith : ActorBase
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        goal.Add(new KeyValuePair<string, object>("Ore In Chest", true));
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
