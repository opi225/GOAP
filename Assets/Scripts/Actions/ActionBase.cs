using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBase : MonoBehaviour
{
    public HashSet<KeyValuePair<string, object>> precon;
    public HashSet<KeyValuePair<string, object>> effect;

    public GameObject target;

    public float cost = 1f;

    public ActionBase()
    {
        precon = new HashSet<KeyValuePair<string, object>>();
        effect = new HashSet<KeyValuePair<string, object>>();
    }

    public abstract bool CheckPrecon(GameObject actor);

    public abstract bool DoAction(GameObject actor);

    public HashSet<KeyValuePair<string, object>> GetPrecon()
    {
        return precon;
    }

    public HashSet<KeyValuePair<string, object>> GetEffect()
    {
        return effect;
    }

    public float GetCost()
    {
        return cost;
    }
}
