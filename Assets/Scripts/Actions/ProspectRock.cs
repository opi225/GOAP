using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Prospect Rock action.
This will have the actor approach a rock that is generic and replace it with a rock that is either a copper or a tin rock.
*/

public class ProspectRock : ActionBase
{
    public GameObject copperRock;
    public GameObject tinRock;
    public ProspectRock()
    {
        precon.Add(new KeyValuePair<string, object>("Rock Available", true));
        precon.Add(new KeyValuePair<string, object>("Mine Rock Available", false));
        effect.Add(new KeyValuePair<string, object>("Mine Rock Available", true));
        target = null;
        cost = 1f;
    }

    /*Checks the preconditions of this action
     makes a list of all unprospected rocks in the scene, and then finds the closet one.
    If the rock the actor was planning to prospect disappears, it will move onto the next closest rock.
    If there are no rocks left, it will return false, and the plan will be cancelled*/
    public override bool CheckPrecon(GameObject actor)
    {
        GameObject[] rocks = UnityEngine.GameObject.FindGameObjectsWithTag("Prospect Rock");
        GameObject closest = null;

        foreach(GameObject thisRock in rocks)
        {
            if (closest == null)
            {
                closest = thisRock;
            } 
            else
            {
                if((thisRock.gameObject.transform.position - actor.transform.position).magnitude  < (closest.gameObject.transform.position - actor.transform.position).magnitude)
                {
                    closest = thisRock;
                }
            }
        }

        if(closest == null)
        {
            Debug.Log("");
            return false;
        }

        target = closest;
        return true;
    }

    //Causes the effect of this action. When a rock gets prospected, it replaces the original rock with a new rock that can be mined for ore.
    public override bool DoAction(GameObject actor)
    {
        GameObject newRock;
        switch (Random.Range(0, 2))
        {
            case 0:
                newRock = (GameObject)Instantiate(Resources.Load("Copper Rock"), target.transform.position, Quaternion.identity);
                Destroy(target);
                break;
            case 1:
                newRock = (GameObject)Instantiate(Resources.Load("Tin Rock"), target.transform.position, Quaternion.identity);
                Destroy(target);
                break;
        }
        return true;
    }
}
