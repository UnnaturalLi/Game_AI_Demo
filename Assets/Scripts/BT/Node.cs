using System;
using System.Collections.Generic;
using UnityEngine;

public enum ENodeState
{
    Running,
    Finished,
    Failed,
}
[Serializable]
public class Node
{
    public Condition condition;
    public List<Node> children = new List<Node>();
    public Node Parent;
    public string Description;
    public Node AddChildren(params Node[] children)
    {
        foreach(Node c in children)
        {
            c.Parent = this;
            this.children.Add(c);
        }
        return this;
    }

    public Node SetCondition(Condition condition)
    {
        this.condition = condition;
        return this;
    }
    public ENodeState Update(AIAgent agent)
    {
        if (condition?.GetCondition(agent) == false)
        {
            return ENodeState.Failed;
        }
        return OnUpdate(agent);
    }
    protected virtual ENodeState OnUpdate(AIAgent agent)
    {
        return ENodeState.Finished;
    }
    protected virtual void OnReset(AIAgent agent){}

    public virtual string GetDescription()
    {
        return "";
    }
}