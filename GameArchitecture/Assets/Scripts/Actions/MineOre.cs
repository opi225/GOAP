using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineOre : ActionBase
{
    public float mineTime = 0;
    public MineOre()
    {
        precon.Add(new KeyValuePair<string, object>("Mine Rock Available", true));
        precon.Add(new KeyValuePair<string, object>("Has Ore", false));
        effect.Add(new KeyValuePair<string, object>("Has Ore", true));
    }

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
        Debug.Log("rocks found!");
        target = closest;
        return true;
    }

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
