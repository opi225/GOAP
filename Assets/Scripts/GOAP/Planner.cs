using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Planner Class
 The Brain of the entire AI system.
This object will take in a list of actions, that a given actor has, as well as their goals and the world state, and create a plan that will be the lowest cost based on the */

public class Planner : MonoBehaviour
{
    public Queue<ActionBase> createPlan(GameObject actor, List<ActionBase> totalActions, HashSet<KeyValuePair<string, object>> goal, HashSet<KeyValuePair<string, object>> introStates)
    {
        Queue<ActionBase> finalQueue = new Queue<ActionBase>();
        //List<ActionBase> readyActions = findReadyActions(actor, totalActions);

        List<ActionNode> goalPaths = new List<ActionNode>();
        ActionNode baseNode = new ActionNode(null, 0, introStates, null);
        goalPaths = FindPath(totalActions, baseNode, goal);

        if(goalPaths.Count == 0)
        {
            return finalQueue;
        }

        //find the best path
        ActionNode bestPath = null;
        foreach(ActionNode node in goalPaths)
        {
            if(bestPath == null)
            {
                bestPath = node;
            }
            else
            {
                if(bestPath.totalCost > node.totalCost)
                {
                    bestPath = node;
                }
            }
        }

        List<ActionBase> finalPath = new List<ActionBase>();
        while(bestPath != null)
        {
            if(bestPath.action != null)
            {
                finalPath.Insert(0, bestPath.action);
            }
            bestPath = bestPath.parentNode;
        }

        foreach(ActionBase act in finalPath)
        {
            finalQueue.Enqueue(act);
        }

        return finalQueue;
    }

    public List<ActionBase> findReadyActions(GameObject actor, List<ActionBase> totalActions)
    {
        List<ActionBase> actions = new List<ActionBase>();
        foreach (ActionBase act in totalActions)
        {
            if (act.CheckPrecon(actor))
            {
                actions.Add(act);
            }
        }
        return actions;
    }

    public bool StateCheck(HashSet<KeyValuePair<string, object>> baseSet, HashSet<KeyValuePair<string, object>> checkSet)
    {
        foreach (KeyValuePair<string, object> b in baseSet)
        {
            bool check = false;
            foreach (KeyValuePair<string, object> c in checkSet)
            {
                if(b.Equals(c))
                {
                    check = true;
                    break;
                }
            }
            if (check == false)
            {
                return false;
            }
        }
        return true;
    }

    public HashSet<KeyValuePair<string, object>> MakeNewStates(HashSet<KeyValuePair<string, object>> currentStates, HashSet<KeyValuePair<string, object>> newStates)
    {
        HashSet<KeyValuePair<string, object>> returnStates = new HashSet<KeyValuePair<string, object>>(currentStates);

        foreach (KeyValuePair<string, object> change in newStates)
        {
            bool what = false;
            foreach (KeyValuePair<string, object> original in returnStates)
            {
                if (original.Key == change.Key)
                {
                    what = true;
                }
            }
            if(what)
            {
                returnStates.RemoveWhere((KeyValuePair<string, object> toRemove) => { return toRemove.Key.Equals(change.Key); });
                returnStates.Add(change);
            }
        }
        return returnStates;
    }

    public List<ActionNode> FindPath(List<ActionBase> actionList, ActionNode baseNode, HashSet<KeyValuePair<string, object>> goal)
    {
        List<ActionNode> returnPath = new List<ActionNode>();
        foreach (ActionBase act in actionList)
        {
            if (StateCheck(act.GetPrecon(), baseNode.states))
            {
                HashSet<KeyValuePair<string, object>> newStates = MakeNewStates(baseNode.states, act.GetEffect());
                ActionNode thisNode = new ActionNode(baseNode, baseNode.totalCost + act.GetCost(), newStates, act);

                //Debug.Log("Goal State Reached? : " + StateCheck(newStates, goal));
                //foreach(KeyValuePair<string, object> g in goal)
                //{
                //    Debug.Log("Goal State : " + g.Key + " " + g.Value);
                //}
                //foreach (KeyValuePair<string, object> n in newStates)
                //{
                //    Debug.Log("New State : " + n.Key + " " + n.Value);
                //}

                if (StateCheck(goal, newStates))
                {
                    returnPath.Add(thisNode);
                }
                else
                {
                    List<ActionBase> shorterList = new List<ActionBase>(actionList);
                    shorterList.Remove(act);
                    returnPath.AddRange(FindPath(shorterList, thisNode, goal));
                }
            }
        }
        return returnPath;
    }

    public class ActionNode
    {
        public ActionNode parentNode;
        public float totalCost;
        public HashSet<KeyValuePair<string, object>> states;
        public ActionBase action;

        public ActionNode(ActionNode _parentNode, float _totalCost, HashSet<KeyValuePair<string, object>> _states, ActionBase _action)
        {
            parentNode = _parentNode;
            totalCost = _totalCost;
            states = _states;
            action = _action;
        }
    }
}
