using UnityEngine;

public class SelectNode : Node
{
    private Node selectedNode=null;
    protected override ENodeState OnUpdate(AIAgent agent)
    {
        ENodeState state = ENodeState.Failed;
        if (selectedNode != null)
        {
            state=selectedNode.Update(agent);
            if (state != ENodeState.Failed)
            {
                if (state == ENodeState.Finished)
                {
                    OnReset(agent);
                }
                return state;
            }
            selectedNode=null;
        }
        foreach (var child in children)
        {
            var runState = child.Update(agent);
            if ( runState!=ENodeState.Failed)
            {
                selectedNode = child;
                state = runState;
                break;
            }
        }
        if (state == ENodeState.Finished|| state == ENodeState.Failed)
        {
            OnReset(agent);
        }
        return state;
    }

    protected override void OnReset(AIAgent agent)
    {
        selectedNode=null;
    }

    public override string GetDescription()
    {
        return selectedNode?.GetDescription();
    }
}