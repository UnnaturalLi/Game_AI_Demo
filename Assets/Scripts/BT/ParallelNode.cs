using UnityEngine;

public class ParallelNode : Node
{
    
    protected override ENodeState OnUpdate(AIAgent agent)
    {
        if (children.Count == 0)
        {
            return ENodeState.Finished;
        }
        bool running = false;
        foreach (var child in children)
        {
            if (child.Update(agent) == ENodeState.Running)
            {
                running = true;
            }
        }
        if (!running)
        {
            OnReset(agent);
            return ENodeState.Finished;
        }
        return ENodeState.Running;
    }

    protected override void OnReset(AIAgent agent)
    {
        
    }

    public override string GetDescription()
    {
        string description ="";
        foreach (var VARIABLE in children)
        {
            description=string.Concat(description, "  ", VARIABLE.GetDescription());
        }
        return description;
    }
}
