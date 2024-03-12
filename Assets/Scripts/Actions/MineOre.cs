using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Mine Ore Action.
 This action will have the actor approiach the rock that either contains copper or tin, mine the rock for 2 seconds, destroy the rock, and add 1 copper or tin to their inventory
*/

public class MineOre : ActionBase
{
    public float mineTime = 0;
    public MineOre()
    {
        precon.Add(new KeyValuePair<string, object>("Mine Rock Available", true));
        precon.Add(new KeyValuePair<string, object>("Has Ore", false));
        effect.Add(new KeyValuePair<string, object>("Has Ore", true));
    }

    /*Checks the preconditions of this action
    makes a list of all minable rocks in the scene, and then finds the closet one.
    If the rock the actor was planning to mine disappears, it will move onto the next closest rock.
    If there are no rocks left, it will return false, and the plan will be cancelled*/
    public override bool CheckPrecon(GameObject actor)
    {
        GameObject[] ore = UnityEngine.GameObject.FindGameObjectsWithTag("Mineable Rock");
        GameObject closest = null;

        foreach (GameObject thisOre in ore)
        {
            if (closest == null)
            {
                closest = thisOre;
            }
            else
            {
                if ((thisOre.gameObject.transform.position - actor.transform.position).magnitude < (closest.gameObject.transform.position - actor.transform.position).magnitude)
                {
                    closest = thisOre;
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

    /**/ 
    public override bool DoAction(GameObject actor)
    {
        if (mineTime >= 2)
        {
            if (target.GetType() == typeof(CopperRock))
            {
                actor.GetComponent<Miner>().copperOre++;
            }
            if (target.GetType() == typeof(TinRock))
            {
                actor.GetComponent<Miner>().tinOre++;
            }

            mineTime = 0f;
            Destroy(target);
            Instantiate(Resources.Load("Rock"), new Vector3(Random.Range(-45, -10), 0, Random.Range(-30, 0)), Quaternion.identity);
            return true;
        }
        else
        {
            mineTime += Time.deltaTime;
        }
        return false;
    }
}
