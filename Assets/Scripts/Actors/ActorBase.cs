using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 This is a base class that all actors draw from.
Actors are the NPCs that actually utilize the AI system.
 */

public abstract class ActorBase : MonoBehaviour, IAI
{
    public float moveSpeed = 1000f;

    public HashSet<KeyValuePair<string, object>> goal;

    public int copperOre = 0;
    public int tinOre = 0;
    public int bronzeBar = 0;

    protected List<ActionBase> totalActions;
    protected ActionBase endAction;
    public Queue<ActionBase> plan;

    public Planner actionPlanner;

    //Starting Function that sets up all the necessary acpects of the actor, like the plan queue, the actionPlanner, and the goal
    public virtual void Start()
    {
        goal = new HashSet<KeyValuePair<string, object>>();
        totalActions = new List<ActionBase>();
        plan = new Queue<ActionBase>();
        actionPlanner = this.GetComponent<Planner>();
        AddActions();
    }

    //Moves the actor towards the location of whatever their current goal is
    public void moveTo(Transform goalLoc)
    {
        if(Vector3.Distance(transform.position, goalLoc.position) > 1f)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, goalLoc.position, step);
        }
    }


    private void AddActions()
    {
        ActionBase[] actions = this.GetComponents<ActionBase>();
        foreach (ActionBase a in actions)
        {
            totalActions.Add(a);
        }
    }

    //Simply returns the actors goal
    public HashSet<KeyValuePair<string, object>> GetGoal()
    {
        return goal;
    }

    // Returns the state of the world, which lets the planner know what the character should currently be doing, and if any possible actions aren't possible at this moment.
    // Some of the things it checks are if this actor is holding any ore, if there is any ore in the chests, if there are any minable rocks available, and if there are any unprospected rocks available
    public HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldState = new HashSet<KeyValuePair<string, object>>();
        GameObject[] ore = UnityEngine.GameObject.FindGameObjectsWithTag("Mineable Rock");
        GameObject[] prospect = UnityEngine.GameObject.FindGameObjectsWithTag("Prospect Rock");
        GameObject[] chest = UnityEngine.GameObject.FindGameObjectsWithTag("Chest");
        GameObject[] anvil = UnityEngine.GameObject.FindGameObjectsWithTag("Anvil");
        int oreNum = 0;
        int barNum = 0;
        foreach(GameObject c in chest)
        {
            oreNum += c.GetComponent<Chest>().copperOre;
            oreNum += c.GetComponent<Chest>().tinOre;
            barNum += c.GetComponent<Chest>().BronzeBar;
        }
        List<GameObject> unusedAnvils = new List<GameObject>();
        foreach(GameObject a in anvil)
        {
            if (!a.GetComponent<Anvil>().inUse)
            {
                unusedAnvils.Add(a);
            }
        }

        //Inventory Related Stuff
        worldState.Add(new KeyValuePair<string, object>("Has Ore", tinOre > 0 || copperOre > 0));
        worldState.Add(new KeyValuePair<string, object>("Ore In Chest", oreNum > 0));
        worldState.Add(new KeyValuePair<string, object>("Ore In Chest", barNum > 0));

        //Mining Related Stuff
        worldState.Add(new KeyValuePair<string, object>("Mine Rock Available", ore.Length > 0));
        worldState.Add(new KeyValuePair<string, object>("Rock Available", prospect.Length > 0));

        //Smithing Related Stuff
        worldState.Add(new KeyValuePair<string, object>("Anvil Available", unusedAnvils.Count > 0));


        return worldState;
    }

    /* 
    Update function.
    checks if the actor has a plan, and makes one if they don't
    next, it will check the preconditions of whatever its current action is. If the current plan is no longer possible, it cancels the plan
    next, it will check the distance between the object the actor is trying to move towards, and move them closer if they are too far away
    lastly, it will perform the action, and remove that action from the queue
     */
    protected virtual void Update()
    {
        if (plan.Count == 0)
        {
            plan = actionPlanner.createPlan(gameObject, totalActions, goal, GetWorldState());
            Debug.Log(printPlan(plan));
        }
        else
        {
            if (plan.Peek().CheckPrecon(gameObject))
            {
                if (Vector3.Distance(plan.Peek().target.transform.position, transform.position) > 3f)
                {
                    moveTo(plan.Peek().target.transform);
                }
                else
                {
                    //Debug.Log("Doing Action: " + plan.Peek().GetType().Name);
                    if (plan.Peek().DoAction(gameObject))
                    {
                        plan.Dequeue();
                    }
                }
            }
            else
            {
                CancelPlan();
            }
        }
    }

    //Function to clear out the entire plan queue. Used if at some point while performing the plan, the plan becomes impossible
    public void CancelPlan()
    {
        plan.Clear();
        Debug.Log("Plan Cancelled");
    }

    //Function to print out the current list of actions the current plan so the user can visualize what the actor is going to do
    public static string printPlan(Queue<ActionBase> actions)
    {
        string s = "Plan: ";
        foreach (ActionBase a in actions)
        {
            s += a.GetType().Name;
            s += "-> ";
        }
        s += "GOAL";
        return s;
    }
}
