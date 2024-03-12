using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Base class that all actions draw from.
 Includes basic aspects that all classes have, like preconditions and effects, the target of the action, and the cost of the action
*/

public abstract class ActionBase : MonoBehaviour
{
    public HashSet<KeyValuePair<string, object>> precon;
    public HashSet<KeyValuePair<string, object>> effect;

    public GameObject target;

    public float cost = 1f;

    //Initializes the precon and effect variables
    public ActionBase()
    {
        precon = new HashSet<KeyValuePair<string, object>>();
        effect = new HashSet<KeyValuePair<string, object>>();
    }

    //checks the preconditions of the action against the world state. This is abstract as the preconditions are reliant on what the action is, and this base class does not actually perform any actions
    public abstract bool CheckPrecon(GameObject actor);

    //performs the desired effect of this action. This is abstract as the effect is reliant on what the action is, and this base class does not actually perform any actions
    public abstract bool DoAction(GameObject actor);

    //returns the preconditions of this action
    public HashSet<KeyValuePair<string, object>> GetPrecon()
    {
        return precon;
    }

    //returns the effect of this actor
    public HashSet<KeyValuePair<string, object>> GetEffect()
    {
        return effect;
    }

    //returns the cost of this actor
    public float GetCost()
    {
        return cost;
    }
}
