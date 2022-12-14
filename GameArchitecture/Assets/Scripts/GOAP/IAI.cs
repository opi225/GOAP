using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAI
{
    HashSet<KeyValuePair<string, object>> GetWorldState();

    public void NoPlan();
    public void EnactPlan();
    public void CancelPlan();
    public void FinishPlan();
}
