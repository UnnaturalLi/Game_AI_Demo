using System.Collections.Generic;
using UnityEngine;

public class GOAPPlanner
{
    public List<GOAPAction> availableActions;

    
    public void Init(List<GOAPAction> availableActions)
    {
        this.availableActions = availableActions;
    }

    public List<GOAPAction> Plan(WorldState preState, WorldState targetState,AIAgent agent)
    {
        List<GOAPActionNode> candidatePlan = new List<GOAPActionNode>();
        Stack<GOAPActionNode> openNodes = new Stack<GOAPActionNode>();
        openNodes.Push(new GOAPActionNode(null,null,targetState,0));
        while (openNodes.Count > 0)
        {
            var node = openNodes.Pop();
            
            if (preState.IsSatisfied(node.targetState))
            {
                candidatePlan.Add(node);
                continue;
            }
            foreach (var action in availableActions)
            {
                
                if (IsActionExisted(node, action))
                {
                    continue;
                }
                var newCurrentState = preState.Clone().Merge(action.effectState);
                if (newCurrentState.IsSatisfied(node.targetState))
                {
                    openNodes.Push(
                        new GOAPActionNode(action,node,action.preState,node.cost+action.GetCost(agent)));  
                }
            }
        }

        if (candidatePlan.Count > 0)
        {
            return SelectCheapestPlan(candidatePlan);
        }

        return null;
    }
    private List<GOAPAction> SelectCheapestPlan(List<GOAPActionNode> candidatePlans)
    {
        var plan=new List<GOAPAction>();
        GOAPActionNode cheapestNode = null;
        var cheapestCost = float.MaxValue;
        foreach (var candidate in candidatePlans)
        {
            if (candidate.cost < cheapestCost)
            {
                cheapestCost = candidate.cost;
                cheapestNode = candidate;
            }
        }
        
        while (cheapestNode != null)
        {
            if (cheapestNode.action != null)
            {
                plan.Add(cheapestNode.action);
            }
            cheapestNode = cheapestNode.parent;
        }
        return plan;
    }
    private bool IsActionExisted(GOAPActionNode node, GOAPAction action)
    {
        var actionExisted = false;
        while (node != null)
        {
            if (node.action != action)
            {
                node = node.parent;
            }
            else
            {
                actionExisted = true;
                break;
            }
        }
        return actionExisted;
    }
}