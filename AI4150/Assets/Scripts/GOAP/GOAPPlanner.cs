using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPPlanner : MonoBehaviour
{
    int MAX_DEPTH = 5;
    HashSet<GOAPAction> usedActions = new HashSet<GOAPAction>();

    public Queue<GOAPAction> CreatePlan(NPC agent, Dictionary<Condition, object> goal)
    {
        Dictionary<Condition, object> currentNPCState = agent.GetNPCState();
        //Check to see if goal is already fulfilled
        if (GoalFromState(goal, currentNPCState))
        {
            return null;
        }

        HashSet<GOAPAction> usableActions = GetUsableActions(agent.AllActions, currentNPCState);
        List<GOAPNode> solutionNodes = new List<GOAPNode>();

        GOAPNode root = new GOAPNode(null, null, currentNPCState, 0);
        List<GOAPNode> nodes = new List<GOAPNode>();
        BuildGraph(root, agent.AllActions, goal,0,solutionNodes);

        if (solutionNodes.Count == 0)
        {
            return null;
        }

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
                GOAPNode node = new GOAPNode(root, action, newState, root.costSoFar + action.cost);
                
                if (GoalFromState(goal, newState))
                {
                    solutionNodes.Add(node);
                }
                else
                {
                    usedActions.Add(action);
                    BuildGraph(node, usableActions, goal, searchDepth + 1, solutionNodes);
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
                }
                bool f = state[kvp.Key] == kvp.Value;
                return false;
            }
        }
        return true;
    }
}
