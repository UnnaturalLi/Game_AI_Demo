using UnityEngine;

public class RepeatNode : Node
{
    public int RepeatTimes;
    private int _repeatCounter;
    protected override ENodeState OnUpdate(AIAgent agent)
    {
        if (children[0] != null&& (_repeatCounter<RepeatTimes|| RepeatTimes==-1))
        {
            
            var state=children[0].Update(agent);
            switch (state)
            {
                case ENodeState.Running:
                    return ENodeState.Running;
                case ENodeState.Finished:
                    _repeatCounter++;
                    return ENodeState.Running;
                case ENodeState.Failed:
                    OnReset(agent);
                    return ENodeState.Failed;
            }
        }
        OnReset(agent);
        return ENodeState.Finished;
    }

    protected override void OnReset(AIAgent agent)
    {
        _repeatCounter = 0;
    }

    public override string GetDescription()
    {
        return children[0]?.GetDescription();
    }
}