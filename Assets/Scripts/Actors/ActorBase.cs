using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorBase : MonoBehaviour, IAI
{
    public float moveSpeed = 1000f;

    public HashSet<KeyValuePair<string, object>> goal;

    public int copperOre = 0;
    public int tinOre = 0;
    public int bronzeBar = 0;
    public int tools = 0;

    protected List<ActionBase> totalActions;
    protected ActionBase endAction;
    public Queue<ActionBase> plan;

    public Planner actionPlanner;

    public virtual void Start()
    {
        goal = new HashSet<KeyValuePair<string, object>>();
        totalActions = new List<ActionBase>();
        plan = new Queue<ActionBase>();
        actionPlanner = this.GetComponent<Planner>();
        AddActions();
    }

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

    public HashSet<KeyValuePair<string, object>> GetGoal()
    {
        return goal;
    }

    public HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldState = new HashSet<KeyValuePair<string, object>>();
        GameObject[] ore = UnityEngine.GameObject.FindGameObjectsWithTag("Mineable Rock");
        GameObject[] prospect = UnityEngine.GameObject.FindGameObjectsWithTag("Prospect Rock");
        GameObject[] chest = UnityEngine.GameObject.FindGameObjectsWithTag("Chest");
        int oreNum = 0;
        foreach(GameObject c in chest)
        {
            oreNum += c.GetComponent<Chest>().copperOre;
            oreNum += c.GetComponent<Chest>().tinOre;
        }


        worldState.Add(new KeyValuePair<string, object>("Has Ore", tinOre > 0 || copperOre > 0));
        worldState.Add(new KeyValuePair<string, object>("Ore In Chest", oreNum > 0));
        worldState.Add(new KeyValuePair<string, object>("Mine Rock Available", ore.Length > 0));
        worldState.Add(new KeyValuePair<string, object>("Rock Available", prospect.Length > 0));



        return worldState;
    }

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
                    Debug.Log("Doing Action: " + plan.Peek().GetType().Name);
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

    public void NoPlan()
    {

    }

    public void EnactPlan()
    {

    }

    public void CancelPlan()
    {
        plan.Clear();
        Debug.Log("Plan Cancelled");
    }

    public void FinishPlan()
    {

    }

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
