using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*AI interface class
 Creates some functions that are meant to interact with the planning system to allow for the creation and editing of plans
Some Functions present here are not used, and will either be used later or deleted*/
public interface IAI
{
    HashSet<KeyValuePair<string, object>> GetWorldState();

    //public void NoPlan();
    //public void EnactPlan();
    public void CancelPlan();
    //public void FinishPlan();
}
