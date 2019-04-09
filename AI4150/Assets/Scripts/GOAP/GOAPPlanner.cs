using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPPlanner
{
    int MAX_DEPTH = 5;
    HashSet<GOAPAction> usedActions = new HashSet<GOAPAction>();

    public Queue<GOAPAction> CreatePlan(NPC agent, Dictionary<Condition, object> goal)
    {
        Debug.Log("Planning....");
        Dictionary<Condition, object> currentNPCState = agent.GetNPCState();
        //Check to see if goal is already fulfilled
        if (GoalFromState(goal, currentNPCState))
        {
            Debug.Log("Hey this plan is finished already!");
            return null;
        }

        HashSet<GOAPAction> usableActions = GetUsableActions(agent.AllActions, currentNPCState);
        List<GOAPNode> solutionNodes = new List<GOAPNode>();

        GOAPNode root = new GOAPNode(null, null, currentNPCState, 0);
        List<GOAPNode> nodes = new List<GOAPNode>();
        BuildGraph(root, usableActions, goal,0,solutionNodes);

        Debug.Log("There's are " + solutionNodes.Count + " ways to do this!");
        if (solutionNodes.Count == 0) return null;

        //Found at least one solution
        float minCost = solutionNodes[0].costSoFar;
        GOAPNode cheapestNode = solutionNodes[0];
        //Find the cheapest(best) solution
        for(int i = 1; i < solutionNodes.Count; i++)
        {
            cheapestNode = solutionNodes[i].costSoFar < minCost ? solutionNodes[i] : cheapestNode;
            minCost = Mathf.Min(minCost, solutionNodes[i].costSoFar);
        }

        GOAPNode curNode = cheapestNode;
        List<GOAPAction> path = new List<GOAPAction>();
        while(curNode != null)
        {
            if(curNode.action != null)
            {
                path.Insert(0, curNode.action);
            }

            curNode = curNode.parent;
        }

        Queue<GOAPAction> plan = new Queue<GOAPAction>();
        foreach (GOAPAction a in path)
        {
            plan.Enqueue(a);
        }

        return plan;
    }

    private HashSet<GOAPAction> GetUsableActions(HashSet<GOAPAction> actions, Dictionary<Condition, object> state)
    {
        HashSet<GOAPAction> usableActions = new HashSet<GOAPAction>();
        //Gets actions whose postConditions meet our goal
        foreach (GOAPAction action in actions)
        {
            if (action.CheckPreConditions(state))
            {
                usableActions.Add(action);
            }
        }
        return usableActions;
    }

    //Currently using DFS, goal to switch to A*
    private void BuildGraph(GOAPNode root, HashSet<GOAPAction> usableActions, Dictionary<Condition, object> goal, int searchDepth, List<GOAPNode> solutionNodes)
    {
        if(searchDepth > MAX_DEPTH)
        {
            return;
        }
        //O(N^2)
        foreach (GOAPAction action in usableActions)
        {
            if (usedActions.Contains(action)) continue;

            if (action.CheckPreConditions(root.state))
            {
                Dictionary<Condition, object> newState = action.ApplyPostConditionsToState(root.state);
                GOAPNode node = new GOAPNode(root, action, newState, root.costSoFar + action.Cost);
                
                if (GoalFromState(goal, newState))
                {
                    solutionNodes.Add(node);
                }
                else
                {
                    usedActions.Add(action);
                    BuildGraph(node, usedActions, goal, searchDepth + 1, solutionNodes);
                    usedActions.Remove(action);
                } 
            }
        }
    }

    private bool GoalFromState(Dictionary<Condition, object> goal, Dictionary<Condition, object> state)
    {
        //Iterate through all goal states in goals. If the currentWorld state can fulfill all goals then return true, else return false
        foreach (KeyValuePair<Condition, object> kvp in goal)
        {
            if (state.ContainsKey(kvp.Key) && state[kvp.Key].Equals(kvp.Value)) { continue; }
            else
            {
                bool t = state.ContainsKey(kvp.Key);
                if(t)
                {
                    Debug.Log("State = " + state[kvp.Key]);
                    Debug.Log("KVP = " + kvp.Value);
                }
                bool f = state[kvp.Key] == kvp.Value;
                return false;
            }
        }
        return true;
    }

    private bool GoalFromAction(Dictionary<Condition, object> goals, GOAPAction action)
        {
            //Iterate through all goal states in goals. If the action can fulfill all goals then return true, else return false
            foreach (KeyValuePair<Condition, object> kvp in goals)
            {
                if (!action.PostConditions.ContainsKey(kvp.Key) && action.PostConditions[kvp.Key] != kvp.Value)
                {
                    return false;
                }
            }
        return true;
    }
}
