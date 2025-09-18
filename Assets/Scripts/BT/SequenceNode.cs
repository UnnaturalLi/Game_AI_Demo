public class SequenceNode : Node
{
    public int FailCountThreshold;
    private int _failCount = 0;
    private int _currentChildIndex=0;
    protected override ENodeState OnUpdate(AIAgent agent)
    {
        var state=children[_currentChildIndex].Update(agent);
        switch (state)
        {
            case ENodeState.Running:
                return ENodeState.Running;
            case ENodeState.Finished:
                _currentChildIndex++;
                _failCount = 0;
                if (_currentChildIndex==children.Count)
                {
                    OnReset(agent);
                    return ENodeState.Finished;
                }
                return ENodeState.Running;
            case ENodeState.Failed:
                _failCount++;
                if (_failCount >= FailCountThreshold)
                {
                    OnReset(agent);
                    return ENodeState.Failed;
                }
                return ENodeState.Running;
            default: return ENodeState.Failed;
        }
    }

    protected override void OnReset(AIAgent agent)
    {
        _failCount = 0;
        _currentChildIndex = 0;
    }

    public override string GetDescription()
    {
        return children[_currentChildIndex]?.GetDescription();
    }
}