using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverOre : ActionBase
{
    public DeliverOre()
    {
        precon.Add(new KeyValuePair<string, object>("Has Ore", true));
        effect.Add(new KeyValuePair<string, object>("Has Ore", false));
        effect.Add(new KeyValuePair<string, object>("Ore In Chest", true));
    }

    // Start is called before the first frame update
    public override bool CheckPrecon(GameObject actor)
    {
        GameObject[] chests = UnityEngine.GameObject.FindGameObjectsWithTag("Chest");
        GameObject closest = null;

        foreach (GameObject thisChest in chests)
        {
            if (closest == null)
            {
                closest = thisChest;
            }
            else
            {
                if ((thisChest.gameObject.transform.position - actor.transform.position).magnitude < (closest.gameObject.transform.position - actor.transform.position).magnitude)
                {
                    closest = thisChest;
                }
            }
        }

        if (closest == null)
        {
            return false;
        }
        target = closest;
        return true;
    }

    public override bool DoAction(GameObject actor)
    {
        target.GetComponent<Chest>().DeliverOre(actor.GetComponent<Miner>().copperOre, actor.GetComponent<Miner>().tinOre);
        return true;
    }
}
